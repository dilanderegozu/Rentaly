using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.EntityLayer.Entities
{
    public class VehicleType
    {
        public int VehicleTypeId { get; set; }

        public string VehicleTypeName { get; set; }

        public string IconUrl { get; set; }

        public List<Car> Cars { get; set; }
    }
}
