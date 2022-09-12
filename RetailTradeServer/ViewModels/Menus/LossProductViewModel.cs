using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using RetailTrade.Domain.Models;
using RetailTrade.Domain.Services;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RetailTradeServer.ViewModels.Menus
{
    public class LossProductViewModel : BaseViewModel
    {
        #region Private Members

        private Document _document = new();
        private readonly IProductService _productService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IDocumentService _documentService;
        private readonly IUserStore _userStore;
        private readonly IMessageStore _messageStore;
        private IEnumerable<Product> _products;
        private IEnumerable<WareHouse> _wareHouses;
        private LossProduct _selectedLossProduct;

        #endregion

        #region Public Properties

        public GlobalMessageViewModel GlobalMessageViewModel { get; }
        public Document CreatedDocument
        {
            get => _document;
            set
            {
                _document = value;
                OnPropertyChanged(nameof(CreatedDocument));
            }
        }
        public IEnumerable<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public IEnumerable<WareHouse> WareHouses
        {
            get => _wareHouses;
            set
            {
                _wareHouses = value;
                OnPropertyChanged(nameof(WareHouses));
            }
        }
        public TableView LossProductTableView { get; set; }
        public LossProduct SelectedLossProduct
        {
            get => _selectedLossProduct;
            set
            {
                _selectedLossProduct = value;
                OnPropertyChanged(nameof(SelectedLossProduct));
            }
        }

        #endregion

        #region Constructor

        public LossProductViewModel(IProductService productService,
            IWareHouseService wareHouseService,
            IDocumentService documentService,
            IUserStore userStore,
            IMessageStore messageStore)
        {
            _productService = productService;
            _wareHouseService = wareHouseService;
            _documentService = documentService;
            _userStore = userStore;
            _messageStore = messageStore;
            GlobalMessageViewModel = new(_messageStore);
            Header = "Списание (создание)";
        }

        #endregion

        #region Public Voids

        [Command]
        public async void UserControlLoaded()
        {
            ShowLoadingPanel = false;
            WareHouses = await _wareHouseService.GetAllAsync();
            Products = await _productService.GetAllAsync();
        }

        [Command]
        public void LossProductTableViewLoadedCommand(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is TableView tableView)
                {
                    LossProductTableView = tableView;
                    LossProductTableView.CellValueChanged += LossProductTableView_CellValueChanged;
                }
            }
        }

        [Command]
        public async void Save()
        {
            try
            {
                if (CreatedDocument.Id == 0)
                {
                    if (CreatedDocument.WareHouseId == null)
                    {
                        _messageStore.SetCurrentMessage("Выберите склад!", MessageType.Error);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CreatedDocument.Number))
                        {
                            if (await _documentService.CheckNumber(CreatedDocument.Number, DocumentTypeEnum.Enter))
                            {
                                _messageStore.SetCurrentMessage($"Документ с номером \"{CreatedDocument.Number}\" уже существует.", MessageType.Error);
                                return;
                            }
                        }
                        if (CreatedDocument.LossProducts != null && CreatedDocument.LossProducts.Any())
                        {
                            CreatedDocument.Amount = CreatedDocument.LossProducts.Sum(r => r.Amount);
                            CreatedDocument.UserId = _userStore.CurrentUser.Id;
                            if (await _documentService.CreateAsync(CreatedDocument, DocumentTypeEnum.Loss) == null)
                            {
                                _messageStore.SetCurrentMessage("Ошибка!", MessageType.Error);
                            }
                            else
                            {
                                _messageStore.SetCurrentMessage("Данные созданы!", MessageType.Success);
                                Header = $"Списание №{CreatedDocument.Number} от {CreatedDocument.CreatedDate:dd.MM.yyyy}";
                            }
                        }
                    }
                }
                else
                {
                    if (CreatedDocument.LossProducts != null && CreatedDocument.LossProducts.Any())
                    {
                        CreatedDocument.Amount = CreatedDocument.LossProducts.Sum(d => d.Amount);
                        if (await _documentService.UpdateAsync(CreatedDocument.Id, CreatedDocument) != null)
                        {
                            _messageStore.SetCurrentMessage("Данные сохранены!", MessageType.Success);
                        }
                        else
                        {
                            _messageStore.SetCurrentMessage("Ошибка при сохранении!", MessageType.Error);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Private Voids

        private void LossProductTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Cell.Property == nameof(LossProduct.ProductId))
                {
                    if (SelectedLossProduct != null && SelectedLossProduct.ProductId != 0)
                    {
                        Product product = Products.FirstOrDefault(p => p.Id == SelectedLossProduct.ProductId);
                        if (product != null)
                        {
                            SelectedLossProduct.Price = product.PurchasePrice;
                        }
                        SelectedLossProduct.Quantity = 1;
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
