using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUI.Areas.Admin.Models;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ICarModelService _carModelService;
        private readonly IBranchService _branchService;
        private readonly IVehicleTypeService _vehicleTypeService;

        public CarController(ICarService carService, ICategoryService categoryService, IBrandService brandService, ICarModelService carModelService, IBranchService branchService, IVehicleTypeService vehicleTypeService)
        {
            _carService = carService;
            _categoryService = categoryService;
            _brandService = brandService;
            _carModelService = carModelService;
            _branchService = branchService;
            _vehicleTypeService = vehicleTypeService;
        }

        public async Task<IActionResult> Index(string searchTerm, int? brandId, int? carModelId, int? categoryId, int page = 1)
        {
            var cars = await _carService.TGetListAsync();
            var categories = await _categoryService.TGetListAsync();
            var brands = await _brandService.TGetListAsync();
            var carModels = await _carModelService.TGetListAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                cars = cars.Where(c =>
                    c.Brand.BrandName.Contains(searchTerm) ||
                    c.CarModel.ModelName.Contains(searchTerm) ||
                    c.PlateNumber.Contains(searchTerm)
                ).ToList();
            }

            if (brandId.HasValue)
            {
                cars = cars.Where(c => c.BrandId == brandId.Value).ToList();
            }

            if (carModelId.HasValue)
            {
                cars = cars.Where(c => c.CarModelId == carModelId.Value).ToList();
            }

            if (categoryId.HasValue)
            {
                cars = cars.Where(c => c.CategoryId == categoryId.Value).ToList();
            }

            int pageSize = 5;
            int totalCount = cars.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedCars = cars
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new VehicleListViewModel
            {
                TotalCar = totalCount,
                Cars = pagedCars,
                Brands = brands,
                CarModels = carModels,
                Categories = categories,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CarDelete(int id)
        {
           await _carService.TDeleteAsync(id);
            return RedirectToAction("Index");
          
        }
        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
            var model = new CarCreateViewModel
            {
                Brands = await _brandService.TGetListAsync(),
                CarModels = await _carModelService.TGetListAsync(),
                Categories = await _categoryService.TGetListAsync(),
                VehicleTypes = await _vehicleTypeService.TGetListAsync(),
                Branches = await _branchService.TGetListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(Car car, IFormFile? ImageFile, string? ImageUrlInput)
        {
            if (!ModelState.IsValid)
            {
                var model = new CarCreateViewModel
                {
                    Car = car,
                    Brands = await _brandService.TGetListAsync(),
                    CarModels = await _carModelService.TGetListAsync(),
                    Categories = await _categoryService.TGetListAsync(),
                    VehicleTypes = await _vehicleTypeService.TGetListAsync(),
                    Branches = await _branchService.TGetListAsync()
                };
                return View(model);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var extension = Path.GetExtension(ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var savePath = Path.Combine("wwwroot", "images", "cars", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                car.ImageUrl = $"/images/cars/{fileName}";
            }
            else if (!string.IsNullOrWhiteSpace(ImageUrlInput))
            {
                car.ImageUrl = ImageUrlInput.Trim();
            }
            else
            {
                car.ImageUrl = "/images/cars/no-image.png";
            }

            car.IsActive = true;
            car.CreatedDate = DateTime.Now;

            await _carService.TInsertAsync(car);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CarDetails(int id)
        {
            var car = await _carService.GetCarWithDetailsByIdAsync(id); 
            if (car == null)
                return NotFound();

            return View(car);
        }
        [HttpGet]
        public async Task<IActionResult> CarEdit(int id)
        {
            var car = await _carService.GetCarWithDetailsByIdAsync(id);
            if (car == null)
                return NotFound();

            var model = new CarEditViewModel
            {
                Car = car,
                Brands = await _brandService.TGetListAsync(),
                CarModels = await _carModelService.TGetListAsync(),
                Categories = await _categoryService.TGetListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CarEdit(Car car, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var model = new CarEditViewModel
                {
                    Car = car,
                    Brands = await _brandService.TGetListAsync(),
                    CarModels = await _carModelService.TGetListAsync(),
                    Categories = await _categoryService.TGetListAsync()
                };
                return View(model);
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var extension = Path.GetExtension(ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";
                var savePath = Path.Combine("wwwroot", "images", "cars", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                car.ImageUrl = $"/images/cars/{fileName}";
            }
            else
            {
                var existingCar = await _carService.TGetByIdAsync(car.CarId);
                car.ImageUrl = existingCar.ImageUrl;
            }

            await _carService.TUpdateAsync(car);
            return RedirectToAction("Index");
        }
    }

}
