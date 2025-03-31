using E_CommerceMVC.Data;
using E_CommerceMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm));
            }

            var products = await query.ToListAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                ViewBag.SearchTerm = searchTerm;
                ViewBag.IsSearch = true;
            }

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Electronics()
        {
            var electronicsProducts = await _context.Products
                .Where(p => p.Category.Name == "Electronics")
                .ToListAsync();

            ViewBag.CategoryName = "Electronics";
            return View("CategoryView", electronicsProducts);
        }


        public async Task<IActionResult> Clothing()
        {
            var clothingProducts = await _context.Products
                .Where(p => p.Category.Name == "Clothing")
                .ToListAsync();

            ViewBag.CategoryName = "Clothing";
            return View("CategoryView", clothingProducts);
        }

        public async Task<IActionResult> OtherCategories()
        {
            var otherProducts = await _context.Products
                .Where(p => p.Category.Name != "Electronics" &&
                           p.Category.Name != "Clothing")
                .ToListAsync();

            ViewBag.CategoryName = "Other Categories";
            return View("CategoryView", otherProducts);
        }

    }
}