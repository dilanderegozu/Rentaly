using System.Collections.Generic;

namespace Rentaly.EntityLayer.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public string? IconName { get; set; } = "category";
        public bool IsActive { get; set; } = true;

        public List<Car> Cars { get; set; } = new();
    }
}