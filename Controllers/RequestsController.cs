using HomeServices.Data;
using HomeServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Linq;

namespace HomeServices.Controllers
{
    [Authorize] // السماح لأي مستخدم مسجل دخول مبدئياً
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // عرض الطلبات الخاصة بالعميل فقط
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myRequests = await _context.Requests
                .Include(r => r.Category)
                .Include(r => r.Provider)
                .Where(r => r.CustomerId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            ViewBag.CompletedCount = myRequests.Count(r => r.Status == "Completed");
            ViewBag.InProgressCount = myRequests.Count(r => r.Status == "Pending" || r.Status == "Accepted");

            return View(myRequests);
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,PreferredSchedule,CategoryId")] Request request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                request.CustomerId = userId;
                request.Status = "Pending";
                request.CreatedAt = DateTime.Now;

                ModelState.Remove("CustomerId");
                ModelState.Remove("Customer");
                ModelState.Remove("Category");

                if (ModelState.IsValid)
                {
                    _context.Add(request);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", request.CategoryId);
            return View(request);
        }

        // التعديل الأهم: متاح للعميل والفني لرؤية تفاصيل الطلب وتقديم العروض
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var request = await _context.Requests
                .Include(r => r.Category)
                .Include(r => r.Provider)
                .Include(r => r.Customer) // أضفنا الكاستمر عشان نعرف مين صاحب الطلب
                .Include(r => r.Offers).ThenInclude(o => o.Provider)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (request == null) return NotFound();

            return View(request);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            var offer = await _context.ServiceOffers
                .Include(o => o.Request).ThenInclude(r => r.Customer)
                .Include(o => o.Provider)
                .FirstOrDefaultAsync(o => o.Id == offerId);

            if (offer == null) return NotFound();

            var request = offer.Request;
            var customer = request.Customer;
            var provider = offer.Provider;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (customer.Id != userId) return Forbid();

            if (customer.WalletBalance < offer.Amount)
            {
                TempData["Error"] = "Insufficient balance in your wallet.";
                return RedirectToAction(nameof(Details), new { id = request.Id });
            }

            // الخصم من العميل وتغيير الحالة
            customer.WalletBalance -= offer.Amount;

            request.Status = "Accepted";
            request.ProviderId = provider.Id;
            request.FinalPrice = offer.Amount;
            offer.IsAccepted = true;

            // ملحوظة: البروفيدر هياخد الـ 90% لما يدوس Complete من الكنترولر بتاعه

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Offer accepted! {offer.Amount:C} has been deducted from your wallet.";
            }
            catch (Exception)
            {
                TempData["Error"] = "Transaction failed.";
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var request = await _context.Requests.FindAsync(id);
            if (request == null) return NotFound();
            if (request.Status != "Pending") return BadRequest("Cannot edit.");

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", request.CategoryId);
            return View(request);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,PreferredSchedule,CategoryId")] Request request)
        {
            if (id != request.Id) return NotFound();

            ModelState.Remove("Customer");
            ModelState.Remove("Category");
            ModelState.Remove("Provider");
            ModelState.Remove("CustomerId");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRequest = await _context.Requests.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                    request.CustomerId = existingRequest.CustomerId;
                    request.Status = existingRequest.Status;
                    request.CreatedAt = existingRequest.CreatedAt;

                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) { }
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        [HttpPost] // الأفضل يكون Post للحماية
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Cancel(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request != null && request.Status == "Pending")
            {
                request.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}