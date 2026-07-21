using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class CarCreateViewModel
    {
        public Car Car { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
        public List<CarModel> CarModels { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<VehicleType> VehicleTypes { get; set; } = new();
        public List<Branch> Branches { get; set; } = new();
    }
}