using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class CategoryFormViewModel
    {
        public Category Category { get; set; } = new();
    }

    public class CategoryRowViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string IconName { get; set; }
        public bool IsActive { get; set; }
        public decimal BasePrice { get; set; }
        public string CapacityDisplay { get; set; }
        public int CarCount { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CategoryListViewModel
    {
        public List<CategoryRowViewModel> Rows { get; set; } = new();

        public int TotalCategoryCount { get; set; }
        public int TotalActiveCarCount { get; set; }
        public decimal AverageDailyPrice { get; set; }
        public string MostPopularCategoryName { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}