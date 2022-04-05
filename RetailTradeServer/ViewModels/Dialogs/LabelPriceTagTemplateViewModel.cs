using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class LabelPriceTagTemplateViewModel : BaseDialogViewModel
    {
        #region Private Members

        private IEnumerable<LabelPriceTag> _labelPriceTags;

        #endregion

        #region Public Properties

        public IEnumerable<LabelPriceTag> LabelPriceTags
        {
            get => _labelPriceTags;
            set
            {
                _labelPriceTags = value;
                OnPropertyChanged(nameof(LabelPriceTags));
            }
        }

        #endregion

        #region Constructor

        public LabelPriceTagTemplateViewModel()
        {
            Title = "Шаблоны этикеток и ценников";
        }

        #endregion

        #region Public Voids

        [Command]
        public void CreateLabelPriceTag()
        {
            try
            {
                WindowService.Show(nameof(CreationAssistantLabelPriceTagView), new CreationAssistantLabelPriceTagViewModel());
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion
    }
}
