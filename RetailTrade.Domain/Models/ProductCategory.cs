using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    public class ProductCategory : DomainObject
    {
        /// <summary>
        /// Категории товаров
        /// </summary>
        public string Name { get; set; }

        public ICollection<ProductSubcategory> ProductSubcategories { get; set; }
    }
}
