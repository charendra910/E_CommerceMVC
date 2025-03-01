using E_CommerceMVC.Data;
using E_CommerceMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceMVC.Controllers
{
    public class CartController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<IdentityUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(int productId, int qty = 1)
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var product = await _context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();

            if (product == null)
            {
                return BadRequest();
            }

            var cart = new Cart { ProductId = productId, Qty = qty };

            return View(cart);
        }


    }
}
