using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Models
{
    public class CarFilterViewModel
    {
        public List<int> VehicleTypeIds { get; set; } = new();
        public List<int> CategoryIds { get; set; } = new();
        public List<int> SeatCounts { get; set; } = new();

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
    public class CarListViewModel
    {
        public List<Car> Cars { get; set; }
        public List<VehicleType> VehicleTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<int> AvailableSeatCounts { get; set; }
        public decimal MaxPossiblePrice { get; set; }
        public CarFilterViewModel Filter { get; set; }
    }
}