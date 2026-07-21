using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class BookingRowViewModel
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string CarName { get; set; }
        public string CarCategoryDisplay { get; set; }
        public int CarYear { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public int DayCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string StatusText { get; set; }
        public string StatusCssClass { get; set; }
    }

    public class BookingListViewModel
    {
        public List<BookingRowViewModel> Rows { get; set; } = new();
        public List<Branch> Branches { get; set; } = new();

        public int TotalBookingCount { get; set; }
        public int ActiveRentalCount { get; set; }
        public int UpcomingCount { get; set; }
        public double CancellationRatePercent { get; set; }

        public List<(string BranchName, int Percent)> TopDropOffPoints { get; set; } = new();
        public double FleetUsagePercent { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}