using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class ProductSubcategory : DomainObject
    {
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }

        public ProductCategory ProductCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
