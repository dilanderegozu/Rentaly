using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class CarModelFormViewModel
    {
        public CarModel CarModel { get; set; } = new();
        public List<Brand> Brands { get; set; } = new();
    }
}