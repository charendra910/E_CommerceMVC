﻿using E_CommerceMVC.Data;
using E_CommerceMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceMVC.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Dashboard/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Dashboard/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image == null)
                {
                    ModelState.AddModelError(nameof(Product.Image), "Image is required.");
                    return View(category);
                }

                var imageName = Guid.NewGuid() + Path.GetExtension(Image.FileName);

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Categories")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Categories"));
                }

                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Categories", imageName);

                await using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                category.Image = $"/img/Categories/{imageName}";

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Dashboard/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Dashboard/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile Image)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                    if (oldCategory == null)
                    {
                        return NotFound();
                    }

                    if (Image != null)
                    {

                        var imageName = Guid.NewGuid() + Path.GetExtension(Image.FileName);

                        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Categories")))
                        {
                            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Categories"));
                        }

                        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Categories", imageName);

                        await using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        oldCategory.Image = $"/img/Categories/{imageName}";

                    }

                    oldCategory.Name = category.Name;
                    oldCategory.Description = category.Description;

                    _context.Update(oldCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Dashboard/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Dashboard/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
