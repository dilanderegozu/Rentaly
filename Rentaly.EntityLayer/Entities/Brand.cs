using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace Rentaly.EntityLayer.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string ImageUrl { get; set; }
 
        public List<Car> Cars { get; set; }
    }
}