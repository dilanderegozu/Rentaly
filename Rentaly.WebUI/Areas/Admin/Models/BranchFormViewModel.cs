using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class BranchFormViewModel
    {
        public Branch Branch { get; set; } = new();
    }

    public class BranchRowViewModel
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string? ManagerName { get; set; }
        public bool IsOpen { get; set; }
        public int TotalCarCount { get; set; }
        public int ActiveRentalCount { get; set; }
    }

    public class BranchListViewModel
    {
        public List<BranchRowViewModel> Rows { get; set; } = new();

        public int TotalBranchCount { get; set; }
        public int TotalFleetCount { get; set; }
        public int TotalActiveRentalCount { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}