using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;

namespace Rentaly.WebUI.ViewComponents.DefaultViewComponentPartial
{
    public class _DefaultCarComponentPartial : ViewComponent
    {
        private readonly RentalyContext _context;

        public _DefaultCarComponentPartial(RentalyContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Cars
                .Include(x => x.Brand)     
                .Include(x => x.CarModel)   
                .Include(x => x.Branch)     
                .Where(x => x.IsAvailable == true) 
                .OrderByDescending(x => x.CarId)   
                .Take(10)                          
                .ToListAsync();

            return View(values);
        }
    }
}