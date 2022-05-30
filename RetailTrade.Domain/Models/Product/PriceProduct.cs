using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailTrade.Domain.Models
{
    public class PriceProduct : DomainObject
    {
        #region Private Members

        private int _typePriceId;
        private int _productId;
        private decimal _price;

        #endregion

        #region Public Properties

        [Key]
        [ForeignKey("TypePrice")]
        public int TypePriceId
        {
            get => _typePriceId;
            set
            {
                _typePriceId = value;
                OnPropertyChanged(nameof(TypePriceId));
            }
        }
        public int ProductId
        {
            get => _productId;
            set
            {
                _productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        public virtual TypePrice TypePrice { get; set; }
        public Product Product { get; set; }

        #endregion
    }
}
