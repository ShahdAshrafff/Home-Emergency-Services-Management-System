using HomeServices.Data;
using HomeServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeServices.Controllers
{
    [Authorize(Roles = "ServiceProvider")] // تأكدي أن اسم الرول مطابق لما في قاعدة البيانات
    public class ProviderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProviderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // 1. عرض الطلبات المتاحة (بدون تغيير كبير هنا)
        public async Task<IActionResult> AvailableRequests()
        {
            var requests = await _context.Requests
                .Include(r => r.Category)
                .Where(r => r.Status == "Pending")
                .ToListAsync();

            return View(requests);
        }

        // 2. [جديد] وظيفة تقديم عرض سعر (Submit Offer)
        [HttpPost]
        public async Task<IActionResult> SubmitOffer(int RequestId, decimal Amount)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // التأكد من أن البروفيدر لم يقدم عرضاً مسبقاً على نفس الطلب
            var existingOffer = await _context.ServiceOffers
                .FirstOrDefaultAsync(o => o.RequestId == RequestId && o.ProviderId == userId);

            if (existingOffer != null)
            {
                TempData["Error"] = "You have already submitted an offer for this request.";
                return RedirectToAction("Details", "Requests", new { id = RequestId });
            }

            var offer = new ServiceOffer
            {
                RequestId = RequestId,
                ProviderId = userId,
                Amount = Amount,
                CreatedAt = DateTime.Now
            };

            _context.ServiceOffers.Add(offer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your offer has been submitted successfully!";
            return RedirectToAction("Details", "Requests", new { id = RequestId });
        }

        public async Task<IActionResult> History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myHistory = await _context.Requests
                .Include(r => r.Category)
                .Where(r => r.ProviderId == userId)
                .ToListAsync();

            return View(myHistory);
        }

        // 3. [تعديل] وظيفة إتمام الطلب وتوزيع الأرباح (90% للبروفيدر)
        [HttpPost]
        public async Task<IActionResult> CompleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            var provider = await _userManager.GetUserAsync(User);

            if (request != null && request.Status == "Accepted" && request.FinalPrice.HasValue)
            {
                // 1. تغيير حالة الطلب
                request.Status = "Completed";

                // 2. حسبة الأرباح (البروفيدر يأخذ 90% من السعر النهائي)
                decimal providerShare = request.FinalPrice.Value * 0.90m;

                // 3. إضافة المبلغ لمحفظة البروفيدر
                provider.WalletBalance += providerShare;

                _context.Update(provider);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Job completed! {providerShare:C} has been added to your wallet.";
            }
            else
            {
                TempData["Error"] = "Could not complete the request. Please check final price.";
            }

            return RedirectToAction(nameof(History));
        }
    }
}