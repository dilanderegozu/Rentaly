using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class CarModelRowViewModel
    {
        public int CarModelId { get; set; }
        public string ModelName { get; set; }
        public string BrandName { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryDisplay { get; set; }
        public string YearRangeDisplay { get; set; }
        public int CarCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class CarModelListViewModel
    {
        public List<CarModelRowViewModel> Rows { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
        public List<Category> Categories { get; set; } = new();

        public int TotalModelCount { get; set; }
        public int ActiveModelCount { get; set; }
        public string MostPopularModelName { get; set; }
        public int NewThisMonthCount { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}