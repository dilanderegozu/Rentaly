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
        private readonly ICategoryService _categoryService;

        public CarModelController(ICarModelService carModelService, IBrandService brandService, ICategoryService categoryService)
        {
            _carModelService = carModelService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string searchTerm, int? brandId, int? categoryId, bool? isActive, int page = 1)
        {
            var allModels = await _carModelService.TGetListAsync();

            var summaryModels = allModels; // özet kartlar tüm veriye göre, filtreye göre değil

            var filtered = allModels.AsEnumerable();

            if (!string.IsNullOrEmpty(searchTerm))
                filtered = filtered.Where(m => m.ModelName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (brandId.HasValue)
                filtered = filtered.Where(m => m.BrandId == brandId.Value);

            if (categoryId.HasValue)
                filtered = filtered.Where(m => m.Cars.Any(c => c.CategoryId == categoryId.Value));

            if (isActive.HasValue)
                filtered = filtered.Where(m => m.IsActive == isActive.Value);

            var filteredList = filtered.ToList();

            int pageSize = 5;
            int totalCount = filteredList.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedRows = filteredList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new CarModelRowViewModel
                {
                    CarModelId = m.CarModelId,
                    ModelName = m.ModelName,
                    BrandName = m.Brand?.BrandName,
                    ImageUrl = m.Cars.FirstOrDefault()?.ImageUrl,
                    CategoryDisplay = m.Cars
                        .GroupBy(c => c.Category?.CategoryName)
                        .OrderByDescending(g => g.Count())
                        .FirstOrDefault()?.Key ?? "—",
                    YearRangeDisplay = m.Cars.Any()
                        ? $"{m.Cars.Min(c => c.Year)}-{m.Cars.Max(c => c.Year)}"
                        : "—",
                    CarCount = m.Cars.Count,
                    IsActive = m.IsActive
                })
                .ToList();

            var model = new CarModelListViewModel
            {
                Rows = pagedRows,
                Brands = await _brandService.TGetListAsync(),
                Categories = await _categoryService.TGetListAsync(),
                TotalModelCount = summaryModels.Count,
                ActiveModelCount = summaryModels.Count(m => m.IsActive),
                MostPopularModelName = summaryModels.OrderByDescending(m => m.Cars.Count).FirstOrDefault()?.ModelName ?? "—",
                NewThisMonthCount = summaryModels.Count(m => m.CreatedDate.Month == DateTime.Now.Month && m.CreatedDate.Year == DateTime.Now.Year),
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(model);
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
            ModelState.Remove("carModel.Cars");
            ModelState.Remove("carModel.Brand");

            if (!ModelState.IsValid)
            {
                var model = new CarModelFormViewModel
                {
                    CarModel = carModel,
                    Brands = await _brandService.TGetListAsync()
                };
                return View(model);
            }

            carModel.CreatedDate = DateTime.Now;
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
            ModelState.Remove("carModel.Cars");
            ModelState.Remove("carModel.Brand");

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