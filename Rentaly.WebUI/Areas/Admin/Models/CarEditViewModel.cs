using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class CarEditViewModel
    {
        public Car Car { get; set; }
        public List<Brand> Brands { get; set; } = new();
        public List<CarModel> CarModels { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}