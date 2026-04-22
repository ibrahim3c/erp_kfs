using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyERP.Web.Models;
using erp_kfs.Host.Models;
using Identity.Domain;

namespace MyERP.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly UserManager<AppUser> _userManager;

    public HomeController(
        ILogger<HomeController> logger,
        //UserManager<AppUser> userManager)
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        //  لو المستخدم عامل Login
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Admin أو HR
                if (roles.Contains("Admin") || roles.Contains("HR"))
                    return Redirect("/Admin/Dashboard");
            }

            // باقي المستخدمين
            return Redirect("/Employees/MyProfile");
        }

        //  مش عامل Login
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}