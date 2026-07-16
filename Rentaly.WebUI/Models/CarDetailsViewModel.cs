using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Models
{
    public class CarDetailsViewModel
    {
        public Car Car { get; set; }
        public List<Branch> Branches { get; set; }
    }
}