using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Reports;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class LabelPriceTagTemplateViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly IReportService _reportService;
        private readonly ILabelPriceTagService _labelPriceTagService;
        private readonly ILabelPriceTagSizeService _labelPriceTagSizeService;
        private readonly IDataService<TypeLabelPriceTag> _typeLabelPriceTagService;
        private IEnumerable<LabelPriceTag> _labelPriceTags;
        private int _selectedTypeLabelPriceTagId;
        private LabelPriceTag _labelPriceTag;

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
        public int SelectedTypeLabelPriceTagId
        {
            get => _selectedTypeLabelPriceTagId;
            set
            {
                _selectedTypeLabelPriceTagId = value;
                OnPropertyChanged(nameof(SelectedTypeLabelPriceTagId));
            }
        }
        public LabelPriceTag SelectedTypeLabelPriceTag
        {
            get => _labelPriceTag;
            set
            {
                _labelPriceTag = value;
                if (_labelPriceTag != null)
                {
                    try
                    {
                        SelectedTypeLabelPriceTagId = _labelPriceTag.TypeLabelPriceTagId;
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
                OnPropertyChanged(nameof(SelectedTypeLabelPriceTag));
            }
        }
        public GridControl LabelPriceTagGridControl { get; set; }

        #endregion

        #region Constructor

        public LabelPriceTagTemplateViewModel(IReportService reportService,
            ILabelPriceTagService labelPriceTagService, 
            IDataService<TypeLabelPriceTag> typeLabelPriceTagService,
            ILabelPriceTagSizeService labelPriceTagSizeService)
        {
            _reportService = reportService;
            _labelPriceTagService = labelPriceTagService;
            _typeLabelPriceTagService = typeLabelPriceTagService;
            _labelPriceTagSizeService = labelPriceTagSizeService;
            Title = "Шаблоны этикеток и ценников";
        }

        #endregion

        #region Public Voids

        [Command]
        public void CreateLabelPriceTag()
        {
            try
            {
                WindowService.Show(nameof(CreationAssistantLabelPriceTagView), new CreationAssistantLabelPriceTagViewModel(_reportService, _typeLabelPriceTagService, _labelPriceTagSizeService) { SelectedTypeLabelPriceTagId = SelectedTypeLabelPriceTagId });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void UserControlLoaded()
        {
            try
            {
                LabelPriceTags = await _labelPriceTagService.GetAllAsync();
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public void GridControlLoaded(object sender)
        {
            try
            {
                if (sender is RoutedEventArgs e)
                {
                    if (e.Source is GridControl gridControl)
                    {
                        LabelPriceTagGridControl = gridControl;
                        LabelPriceTagGridControl.FilterString = $"[TypeLabelPriceTagId] = {SelectedTypeLabelPriceTagId}";
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion
    }
}
