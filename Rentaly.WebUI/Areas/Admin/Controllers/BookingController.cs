using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUI.Areas.Admin.Models;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ICarService _carService;
        private readonly IBranchService _branchService;

        public BookingController(IBookingService bookingService, ICarService carService, IBranchService branchService)
        {
            _bookingService = bookingService;
            _carService = carService;
            _branchService = branchService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.TGetListAsync();
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new BookingFormViewModel
            {
                Cars = await _carService.TGetListAsync(),
                Branches = await _branchService.TGetListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                var model = new BookingFormViewModel
                {
                    Booking = booking,
                    Cars = await _carService.TGetListAsync(),
                    Branches = await _branchService.TGetListAsync()
                };
                return View(model);
            }

            await _bookingService.TInsertAsync(booking);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _bookingService.TGetByIdAsync(id);
            if (booking == null)
                return NotFound();

            var model = new BookingFormViewModel
            {
                Booking = booking,
                Cars = await _carService.TGetListAsync(),
                Branches = await _branchService.TGetListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                var model = new BookingFormViewModel
                {
                    Booking = booking,
                    Cars = await _carService.TGetListAsync(),
                    Branches = await _branchService.TGetListAsync()
                };
                return View(model);
            }

            await _bookingService.TUpdateAsync(booking);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookingService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}