using System;
using System.Collections.Generic;

namespace RetailTrade.Domain.Models
{
    /// <summary>
    /// История изменения цен продуктов
    /// </summary>
    public class Revaluation : DomainObject
    {
        #region Private Members

        private DateTime _revaluationDate;
        private string _comment;

        #endregion

        #region Public Properties
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime RevaluationDate
        {
            get => _revaluationDate;
            set
            {
                _revaluationDate = value;
                OnPropertyChanged(nameof(RevaluationDate));
            }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        /// <summary>
        /// Товары
        /// </summary>
        public ICollection<RevaluationProduct> RevaluationProducts { get; set; }

        #endregion
    }
}
