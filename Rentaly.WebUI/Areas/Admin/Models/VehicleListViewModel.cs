using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class VehicleListViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<CarModel> CarModels { get; set; }
        public List<Category> Categories { get; set; }
        public List<Car> Cars { get; set; }
        public int TotalCar { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
