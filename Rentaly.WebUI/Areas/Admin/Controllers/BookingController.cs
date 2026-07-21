using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.WebUI.Areas.Admin.Models;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IBranchService _branchService;
        private readonly ICarService _carService;

        public BookingController(IBookingService bookingService, IBranchService branchService, ICarService carService)
        {
            _bookingService = bookingService;
            _branchService = branchService;
            _carService = carService;
        }

        public async Task<IActionResult> Index(int? branchId, string status, int page = 1)
        {
            var allBookings = await _bookingService.TGetListAsync();
            var branches = await _branchService.TGetListAsync();
            var totalCarCount = (await _carService.TGetListAsync()).Count;

            var now = DateTime.Now;

            var filtered = allBookings.AsEnumerable();

            if (branchId.HasValue)
                filtered = filtered.Where(b => b.PickUpBranchId == branchId.Value);

            if (!string.IsNullOrEmpty(status))
            {
                filtered = status switch
                {
                    "Cancelled" => filtered.Where(b => b.Status == "Cancelled"),
                    "Ongoing" => filtered.Where(b => b.Status != "Cancelled" && b.PickUpDate <= now && b.DropOffDate >= now),
                    "Completed" => filtered.Where(b => b.Status != "Cancelled" && b.DropOffDate < now),
                    "Confirmed" => filtered.Where(b => b.Status != "Cancelled" && b.PickUpDate > now),
                    _ => filtered
                };
            }

            var filteredList = filtered.ToList();

            int pageSize = 5;
            int totalCount = filteredList.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedRows = filteredList
                .OrderByDescending(b => b.BookingId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookingRowViewModel
                {
                    BookingId = b.BookingId,
                    CustomerName = b.CustomerName,
                    CarName = $"{b.Car?.Brand?.BrandName} {b.Car?.CarModel?.ModelName}",
                    CarCategoryDisplay = b.Car?.Category?.CategoryName,
                    CarYear = b.Car?.Year ?? 0,
                    PickUpDate = b.PickUpDate,
                    DropOffDate = b.DropOffDate,
                    DayCount = Math.Max(1, (b.DropOffDate.Date - b.PickUpDate.Date).Days),
                    TotalPrice = b.TotalPrice,
                    StatusText = GetStatusText(b.Status, b.PickUpDate, b.DropOffDate, now),
                    StatusCssClass = GetStatusCssClass(b.Status, b.PickUpDate, b.DropOffDate, now)
                })
                .ToList();

            var activeRentalCount = allBookings.Count(b => b.Status != "Cancelled" && b.PickUpDate <= now && b.DropOffDate >= now);
            var upcomingCount = allBookings.Count(b => b.Status != "Cancelled" && b.PickUpDate > now);
            var cancelledCount = allBookings.Count(b => b.Status == "Cancelled");
            var cancellationRate = allBookings.Any() ? Math.Round((double)cancelledCount / allBookings.Count * 100, 1) : 0;

            var topDropOffPoints = allBookings
                .Where(b => b.DropOffBranch != null)
                .GroupBy(b => b.DropOffBranch.BranchName)
                .OrderByDescending(g => g.Count())
                .Take(2)
                .Select(g => (g.Key, allBookings.Count > 0 ? (int)Math.Round((double)g.Count() / allBookings.Count * 100) : 0))
                .ToList();

            var model = new BookingListViewModel
            {
                Rows = pagedRows,
                Branches = branches,
                TotalBookingCount = allBookings.Count,
                ActiveRentalCount = activeRentalCount,
                UpcomingCount = upcomingCount,
                CancellationRatePercent = cancellationRate,
                TopDropOffPoints = topDropOffPoints,
                FleetUsagePercent = totalCarCount > 0 ? Math.Round((double)activeRentalCount / totalCarCount * 100, 1) : 0,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetDetailsByIdAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _bookingService.TGetByIdAsync(id);
            if (booking == null)
                return NotFound();

            booking.Status = "Cancelled";
            await _bookingService.TUpdateAsync(booking);

            return RedirectToAction("Index");
        }

        private static string GetStatusText(string status, DateTime pickUp, DateTime dropOff, DateTime now)
        {
            if (status == "Cancelled") return "İptal Edildi";
            if (pickUp <= now && dropOff >= now) return "Devam Ediyor";
            if (dropOff < now) return "Tamamlandı";
            return "Onaylandı";
        }

        private static string GetStatusCssClass(string status, DateTime pickUp, DateTime dropOff, DateTime now)
        {
            if (status == "Cancelled") return "status-badge-cancelled";
            if (pickUp <= now && dropOff >= now) return "status-badge-ongoing";
            if (dropOff < now) return "status-badge-completed";
            return "status-badge-confirmed";
        }
    }
}