using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUI.Areas.Admin.Models;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarModelController : Controller
    {
        private readonly ICarModelService _carModelService;
        private readonly IBrandService _brandService;

        public CarModelController(ICarModelService carModelService, IBrandService brandService)
        {
            _carModelService = carModelService;
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var carModels = await _carModelService.TGetListAsync();
            return View(carModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CarModelFormViewModel
            {
                Brands = await _brandService.TGetListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarModel carModel)
        {
            if (!ModelState.IsValid)
            {
                var model = new CarModelFormViewModel
                {
                    CarModel = carModel,
                    Brands = await _brandService.TGetListAsync()
                };
                return View(model);
            }

            await _carModelService.TInsertAsync(carModel);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var carModel = await _carModelService.TGetByIdAsync(id);
            if (carModel == null)
                return NotFound();

            var model = new CarModelFormViewModel
            {
                CarModel = carModel,
                Brands = await _brandService.TGetListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarModel carModel)
        {
            if (!ModelState.IsValid)
            {
                var model = new CarModelFormViewModel
                {
                    CarModel = carModel,
                    Brands = await _brandService.TGetListAsync()
                };
                return View(model);
            }

            await _carModelService.TUpdateAsync(carModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _carModelService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}