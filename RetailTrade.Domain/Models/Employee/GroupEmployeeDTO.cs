using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class GroupEmployeeDTO
    {
        public int Id { get; set; }
        public bool NotFolder { get; set; }
        public string Name { get; set; }
        public GroupEmployee SubGroup { get; set; }
        public int? SubGroupId { get; set; }
        public int EmployeeId { get; set; }
        public ICollection<GroupEmployee> SubGroups { get; set; }
    }
}
