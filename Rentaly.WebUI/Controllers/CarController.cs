using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rentaly.Businesslayer.Abstract;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly IBranchService _branchService;
        private readonly IVehicleTypeService _vehicleTypeService;

        public CarController(ICarService carService, ICategoryService categoryService, IBranchService branchService, IVehicleTypeService vehicleTypeService)
        {
            _carService = carService;
            _categoryService = categoryService;
            _branchService = branchService;
            _vehicleTypeService = vehicleTypeService;
        }

        public async Task<IActionResult> Index(CarFilterViewModel filter)
        {
            var cars = await _carService.TGetListAsync();
            var query = cars.Where(x => x.IsActive && x.IsAvailable).AsQueryable();

            if(filter.VehicleTypeIds.Any())
            {
                query = query.Where(x => filter.VehicleTypeIds.Contains(x.VehicleTypeId));
            }

            if(filter.CategoryIds.Any())
            {
                query = query.Where(x => filter.CategoryIds.Contains(x.CategoryId));
            }

            if(filter.SeatCounts.Any())
            {
                query = query.Where(x => filter.SeatCounts.Contains(x.SeatCount));
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(c => c.DailyPrice >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(c => c.DailyPrice <= filter.MaxPrice.Value);
            }

            var allCars = await _carService.TGetListAsync();

            decimal maxPossiblePrice = allCars.Any() ? allCars.Max(c => c.DailyPrice) : 0;

            var model = new CarListViewModel
            {
                Cars = query.ToList(),
                VehicleTypes = await _vehicleTypeService.TGetListAsync(),
                Categories = await _categoryService.TGetListAsync(),
                AvailableSeatCounts = allCars.Select(c => c.SeatCount).Distinct().OrderBy(s => s).ToList(),
                MaxPossiblePrice = maxPossiblePrice,
                Filter = filter
            };
            return View(model);
        }
        public async Task<IActionResult> CarList()
        {
            var values = await _carService.TGetListAsync();
            return View(values);
     }
        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
            ViewBag.Categories =new SelectList(await _categoryService.TGetListAsync(),"CategoryId","CategoryName");
            ViewBag.Branches = new SelectList(await _branchService.TGetListAsync(), "BranchId", "BranchName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(Car car)
        {
            await _carService.TInsertAsync(car);
            return RedirectToAction("CarList");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetCarWithDetailsByIdAsync(id);
            if (car == null)
                return NotFound();

            var model = new CarDetailsViewModel
            {
                Car = car,
                Branches = await _branchService.TGetListAsync()
            };

            return View(model);
        }
    }
}

