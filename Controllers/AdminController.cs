using HomeServices.Data;
using HomeServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // 1. وظيفة اعتماد الفنيين (Provider Approval)
    public IActionResult PendingProviders()
    {
        // عرض الفنيين اللي لسه متوافقش عليهم
        var pending = _context.Users.Where(u => u.IsApproved == false).ToList();
        return View(pending);
    }

    [HttpPost]
    public async Task<IActionResult> ApproveProvider(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            user.IsApproved = true;
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction(nameof(PendingProviders));
    }

    // 2. وظيفة إدارة الفئات (Categories Management)
    public IActionResult Categories()
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    // 3. وظيفة مراقبة الطلبات (Requests Monitoring)
    public IActionResult AllRequests()
    {
        var requests = _context.Requests.OrderByDescending(r => r.CreatedAt).ToList();
        return View(requests);
    }
}