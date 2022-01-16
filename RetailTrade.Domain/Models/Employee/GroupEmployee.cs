using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class GroupEmployee : DomainObject
    {
        #region Private Members

        private string _name;

        #endregion

        #region Public Properties

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public GroupEmployee SubGroup { get; set; }
        public int? SubGroupId { get; set; }
        public ICollection<GroupEmployee> SubGroups { get; set; }
        public ICollection<Employee> Employees { get; set; }

        #endregion
    }
}
