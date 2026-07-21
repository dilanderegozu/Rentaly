using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.TGetListAsync();
            return View(brands);
        }

        [HttpGet]
        public IActionResult CreateBrand()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(Brand brand)
        {
            ModelState.Remove("Cars");

            if (!ModelState.IsValid)
                return View(brand);

            await _brandService.TInsertAsync(brand);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditBrand(int id)
        {
            var brand = await _brandService.TGetByIdAsync(id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }
        [HttpPost]
        public async Task<IActionResult> EditBrand(Brand brand)
        {
            ModelState.Remove("Cars");

            if (!ModelState.IsValid)
                return View(brand);

            await _brandService.TUpdateAsync(brand);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            await _brandService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}