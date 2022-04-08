using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Reports;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreationAssistantLabelPriceTagViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IReportService _reportService;
        private readonly IDataService<TypeLabelPriceTag> _typeLabelPriceTagService;
        private readonly ILabelPriceTagSizeService _labelPriceTagSizeService;
        private IEnumerable<TypeLabelPriceTag> _typeLabelPriceTags;
        private IEnumerable<LabelPriceTagSize> _labelPriceTagSizes;
        private int? _selectedTypeLabelPriceTagId;
        private int? _selectedLabelPriceTagSizeId = 1;
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

        public IEnumerable<LabelPriceTagSize> LabelPriceTagSizes
        {
            get => _labelPriceTagSizes;
            set
            {
                _labelPriceTagSizes = value;
                OnPropertyChanged(nameof(LabelPriceTagSizes));
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

        public int? SelectedLabelPriceTagSizeId
        {
            get => _selectedLabelPriceTagSizeId;
            set
            {
                _selectedLabelPriceTagSizeId = value;
                OnPropertyChanged(nameof(SelectedLabelPriceTagSizeId));
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
            IDataService<TypeLabelPriceTag> typeLabelPriceTagService,
            ILabelPriceTagSizeService labelPriceTagSizeService)
        {
            _reportService = reportService;
            _labelPriceTagSizeService = labelPriceTagSizeService;
            _typeLabelPriceTagService = typeLabelPriceTagService;
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            Report = await _reportService.ForTemplate();
            LabelPriceTagSizes = await _labelPriceTagSizeService.GetAllAsync();
            TypeLabelPriceTags = await _typeLabelPriceTagService.GetAllAsync();
        }

        [Command]
        public void CreateLabelPriceTagSize()
        {
            try
            {
                WindowService.Show(nameof(CreateLabelPriceTagSizeView), new CreateLabelPriceTagSizeViewModel(_labelPriceTagSizeService) { TypeLabelPriceTags = TypeLabelPriceTags });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion
    }
}
