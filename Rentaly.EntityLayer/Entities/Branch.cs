using System.Collections.Generic;

namespace Rentaly.EntityLayer.Entities
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string? ManagerName { get; set; }
        public bool IsOpen { get; set; } = true;

        public List<Car> Cars { get; set; } = new();
    }
}