using E_CommerceMVC.Data;
using E_CommerceMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<IdentityUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var cart = await _context.Carts
                .Where(x => x.UserId == currentuser.Id)
                .ToListAsync();

            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int qty = 1)
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var product = await _context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();

            if (product == null)
            {
                return BadRequest();
            }

            var cart = new Cart { ProductId = productId, Qty = qty, UserId = currentuser.Id };

            _context.Add(cart);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
