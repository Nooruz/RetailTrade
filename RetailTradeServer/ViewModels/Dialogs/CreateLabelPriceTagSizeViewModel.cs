using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Windows;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class CreateLabelPriceTagSizeViewModel : BaseDialogViewModel
    {
        #region Private Members

        private ILabelPriceTagSizeService _labelPriceTagSizeService;
        private IEnumerable<TypeLabelPriceTag> _typeLabelPriceTags;
        public int? _width;
        public int? _height;
        public int? _selectedTypeLabelPriceTagId;

        #endregion

        #region Private Members

        public IEnumerable<TypeLabelPriceTag> TypeLabelPriceTags
        {
            get => _typeLabelPriceTags;
            set
            {
                _typeLabelPriceTags = value;
                OnPropertyChanged(nameof(TypeLabelPriceTag));
            }
        }

        public int? Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public int? Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public int? SelectedTypeLabelPriceTagId
        {
            get => _selectedTypeLabelPriceTagId;
            set
            {
                _selectedTypeLabelPriceTagId = value;
                OnPropertyChanged(nameof(SelectedTypeLabelPriceTagId));
            }
        }

        #endregion

        #region Constructor

        public CreateLabelPriceTagSizeViewModel(ILabelPriceTagSizeService labelPriceTagSizeService)
        {
            _labelPriceTagSizeService = labelPriceTagSizeService;

            Title = "Создание размера для этикеток и ценников";

            CloseCommand = new RelayCommand(() => CurrentWindowService.Close());
        }

        #endregion

        #region Public Voids

        [Command]
        public async void CreateLabelPriceTagSize()
        {
            if (SelectedTypeLabelPriceTagId != null)
            {
                if (Width != null)
                {
                    if (Height != null)
                    {
                        _ = await _labelPriceTagSizeService.CreateAsync(new LabelPriceTagSize
                        {
                            TypeLabelPriceTagId = SelectedTypeLabelPriceTagId.Value,
                            Width = Width.Value,
                            Height = Height.Value
                        });
                        CurrentWindowService.Close();
                    }
                    else
                    {
                        _ = MessageBoxService.Show("Введите высоту!", "Sale Page", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    _ = MessageBoxService.Show("Введите ширину!", "Sale Page", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                _ = MessageBoxService.Show("Выберите назначение!", "Sale Page", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        #endregion
    }
}
