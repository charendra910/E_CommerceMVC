using E_CommerceMVC.Data;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceMVC.Controllers
{
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AboutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var aboutUsData = _context.AboutUs.ToList();
            return View(aboutUsData);
        }
    }
}
