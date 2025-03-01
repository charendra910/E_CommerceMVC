using Microsoft.AspNetCore.Mvc;

namespace E_CommerceMVC.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
