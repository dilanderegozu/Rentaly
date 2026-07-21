using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUI.Areas.Admin.Models;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var allCategories = await _categoryService.TGetListAsync();

            var allCars = allCategories.SelectMany(c => c.Cars ?? new List<Car>()).ToList();

            int pageSize = 5;
            int totalCount = allCategories.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedRows = allCategories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryRowViewModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description,
                    IconName = string.IsNullOrEmpty(c.IconName) ? "category" : c.IconName,
                    IsActive = c.IsActive,
                    BasePrice = c.Cars.Any() ? c.Cars.Min(car => car.DailyPrice) : 0,
                    CapacityDisplay = c.Cars.Any()
                        ? $"{c.Cars.Min(car => car.SeatCount)}-{c.Cars.Max(car => car.SeatCount)}"
                        : "—",
                    CarCount = c.Cars.Count,
                    ImageUrl = c.Cars.FirstOrDefault()?.ImageUrl
                })
                .ToList();

            var model = new CategoryListViewModel
            {
                Rows = pagedRows,
                TotalCategoryCount = totalCount,
                TotalActiveCarCount = allCars.Count(car => car.IsAvailable),
                AverageDailyPrice = allCars.Any() ? allCars.Average(car => car.DailyPrice) : 0,
                MostPopularCategoryName = allCategories.OrderByDescending(c => c.Cars.Count).FirstOrDefault()?.CategoryName ?? "—",
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(new CategoryFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            ModelState.Remove("category.Cars");

            if (!ModelState.IsValid)
                return View(new CategoryFormViewModel { Category = category });

            await _categoryService.TInsertAsync(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryService.TGetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(new CategoryFormViewModel { Category = category });
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            ModelState.Remove("category.Cars");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var keys = string.Join(" | ", ModelState.Keys);
                return Content("KEYS: " + keys + " || ERRORS: " + string.Join(" | ", errors));
            }

            await _categoryService.TUpdateAsync(category);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}