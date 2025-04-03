using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceMVC.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")] // Secure entire dashboard
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
