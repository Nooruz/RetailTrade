using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Reports;
using RetailTradeServer.ViewModels.Dialogs.Base;
using SalePageServer.Report;
using System;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreationAssistantLabelPriceTagViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IReportService _reportService;
        private readonly IDataService<TypeLabelPriceTag> _typeLabelPriceTagService;
        private IEnumerable<TypeLabelPriceTag> _typeLabelPriceTags;
        private int? _selectedTypeLabelPriceTagId;
        private object _report;

        #endregion

        #region Private Members

        public IEnumerable<TypeLabelPriceTag> TypeLabelPriceTags
        {
            get => _typeLabelPriceTags;
            set
            {
                _typeLabelPriceTags = value;
                OnPropertyChanged(nameof(TypeLabelPriceTags));
            }
        }

        public int? SelectedTypeLabelPriceTagId
        {
            get => _selectedTypeLabelPriceTagId;
            set
            {
                _selectedTypeLabelPriceTagId = value;
                if (_selectedTypeLabelPriceTagId != null)
                {
                    try
                    {
                        Title = _selectedTypeLabelPriceTagId.Value == 1 ? "Помощник по создания этикетки" : "Помощник по создания ценника";
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
                OnPropertyChanged(nameof(SelectedTypeLabelPriceTagId));
            }
        }

        public object Report
        {
            get => _report;
            set
            {
                _report = value;
                OnPropertyChanged(nameof(Report));
            }
        }

        #endregion

        #region Constructor

        public CreationAssistantLabelPriceTagViewModel(IReportService reportService,
            IDataService<TypeLabelPriceTag> typeLabelPriceTagService)
        {
            _reportService = reportService;
            _typeLabelPriceTagService = typeLabelPriceTagService;
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            Report = await _reportService.ForTemplate();
            TypeLabelPriceTags = await _typeLabelPriceTagService.GetAllAsync();
        }

        #endregion
    }
}
