using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Models
{
    public class BookingFormViewModel
    {
        public Booking Booking { get; set; } = new();
        public List<Car> Cars { get; set; } = new();
        public List<Branch> Branches { get; set; } = new();
    }
}