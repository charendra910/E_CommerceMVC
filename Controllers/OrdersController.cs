using E_CommerceMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceMVC.Controllers
{
    public class OrdersController : Controller
    {
        public readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(x => x.OrderProducts)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            return View(order);
        }
    }
}
