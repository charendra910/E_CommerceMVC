using E_CommerceMVC.Data;
using E_CommerceMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceMVC.Controllers
{
    public class ContactusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactusController(ApplicationDbContext context)
        {
            _context = context;
        }



        public IActionResult AddContactUs()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddContactUs(ContactModel contact)
        {

            if (ModelState.IsValid)
            {
                _context.ContactUs.Add(contact);
                _context.SaveChanges();
                return RedirectToAction("AddContactUs");
            }
            return View(contact);

        }



    }
}
