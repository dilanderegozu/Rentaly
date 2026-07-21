using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class CarModel
    {
        public int CarModelId { get; set; }
        public string ModelName { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
