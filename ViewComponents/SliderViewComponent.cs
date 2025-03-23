using E_CommerceMVC.Data; // Replace with your namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace E_CommerceMVC.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public SliderViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var sliderImages = await _context.SliderImages.ToListAsync();
        //    return View("Default", sliderImages);
        //}


        public async Task<IViewComponentResult> InvokeAsync(string category)
        {
            var sliderImages = await _context.SliderImages
                .Where(img => img.Category == category) // Filter by category
                .ToListAsync();

            return View(sliderImages);
        }

    }
}


