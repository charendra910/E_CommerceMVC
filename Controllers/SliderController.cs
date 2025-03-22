using E_CommerceMVC.Data;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceMVC.Controllers
{
    public class SliderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SliderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Sliderimage = _context.SliderImages.ToList();
            return View();
        }

    }
}
