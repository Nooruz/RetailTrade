using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Reports;
using RetailTradeServer.ViewModels.Dialogs.Base;
using RetailTradeServer.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreationAssistantLabelPriceTagViewModel : BaseDialogViewModel
    {
        #region Private Members

        private readonly ILabelPriceTagService _labelPriceTagService;
        private readonly IReportService _reportService;
        private readonly IDataService<TypeLabelPriceTag> _typeLabelPriceTagService;
        private readonly ILabelPriceTagSizeService _labelPriceTagSizeService;
        private IEnumerable<TypeLabelPriceTag> _typeLabelPriceTags;
        private ObservableCollection<LabelPriceTagSize> _labelPriceTagSizes = new();
        private int? _selectedTypeLabelPriceTagId;
        private int? _selectedLabelPriceTagSizeId = 1;
        public LabelPriceTagSize _selectedLabelPriceTagSize;
        private object _report;
        private string _name;

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

        public ObservableCollection<LabelPriceTagSize> LabelPriceTagSizes
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

        public LabelPriceTagSize SelectedLabelPriceTagSize
        {
            get => _selectedLabelPriceTagSize;
            set
            {
                _selectedLabelPriceTagSize = value;
                OnPropertyChanged(nameof(SelectedLabelPriceTagSize));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        #endregion

        #region Constructor

        public CreationAssistantLabelPriceTagViewModel(ILabelPriceTagService labelPriceTagService,
            IReportService reportService,
            IDataService<TypeLabelPriceTag> typeLabelPriceTagService,
            ILabelPriceTagSizeService labelPriceTagSizeService)
        {
            _labelPriceTagService = labelPriceTagService;
            _reportService = reportService;
            _labelPriceTagSizeService = labelPriceTagSizeService;
            _typeLabelPriceTagService = typeLabelPriceTagService;

            _labelPriceTagSizeService.OnCreated += LabelPriceTagSizeService_OnCreated;
        }

        #endregion

        #region Private Members

        private void LabelPriceTagSizeService_OnCreated(LabelPriceTagSize labelPriceTagSize)
        {
            try
            {
                LabelPriceTagSizes.Add(labelPriceTagSize);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            Report = await _reportService.ForTemplate();
            LabelPriceTagSizes = new(await _labelPriceTagSizeService.GetAllAsync());
            TypeLabelPriceTags = await _typeLabelPriceTagService.GetAllAsync();
        }

        [Command]
        public void CreateLabelPriceTagSize()
        {
            try
            {
                WindowService.Show(nameof(CreateLabelPriceTagSizeView), new CreateLabelPriceTagSizeViewModel(_labelPriceTagSizeService) { TypeLabelPriceTags = TypeLabelPriceTags, SelectedTypeLabelPriceTagId = SelectedTypeLabelPriceTagId });
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void SelectedIndexChanged()
        {
            try
            {
                Report = await _reportService.ChangeSizeLabelReport(SelectedLabelPriceTagSize.Width, SelectedLabelPriceTagSize.Height);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void CreateLabelPriceTag()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                await _labelPriceTagService.CreateAsync(new LabelPriceTag
                {
                    Name = Name,
                    TypeLabelPriceTagId = SelectedTypeLabelPriceTagId.Value,
                    LabelPriceTagSizeId = SelectedLabelPriceTagSizeId.Value
                });
                CurrentWindowService.Close();
            }
            else
            {
                _ = MessageBoxService.ShowMessage("Введите наименование!", "Sale Page", MessageButton.OK, MessageIcon.Exclamation);
            }
        }

        #endregion
    }
}
