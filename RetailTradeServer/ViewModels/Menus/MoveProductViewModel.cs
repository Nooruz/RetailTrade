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
    public class MoveProductViewModel : BaseViewModel
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
        private MoveProduct _selectedMoveProduct;

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
        public TableView MoveProductTableView { get; set; }
        public MoveProduct SelectedMoveProduct
        {
            get => _selectedMoveProduct;
            set
            {
                _selectedMoveProduct = value;
                OnPropertyChanged(nameof(SelectedMoveProduct));
            }
        }

        #endregion

        #region Constructor

        public MoveProductViewModel(IProductService productService,
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
            Header = "Перемещение (создание)";
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
        public void MoveProductTableViewLoadedCommand(object sender)
        {
            if (sender is RoutedEventArgs e)
            {
                if (e.Source is TableView tableView)
                {
                    MoveProductTableView = tableView;
                    MoveProductTableView.CellValueChanged += MoveProductTableView_CellValueChanged;
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
                        if (CreatedDocument.MoveProducts != null && CreatedDocument.MoveProducts.Any())
                        {
                            CreatedDocument.Amount = CreatedDocument.MoveProducts.Sum(r => r.Amount);
                            CreatedDocument.UserId = _userStore.CurrentUser.Id;
                            if (await _documentService.CreateAsync(CreatedDocument, DocumentTypeEnum.Enter) == null)
                            {
                                _messageStore.SetCurrentMessage("Ошибка!", MessageType.Error);
                            }
                            else
                            {
                                _messageStore.SetCurrentMessage("Данные созданы!", MessageType.Success);
                                Header = $"Перемещение №{CreatedDocument.Number} от {CreatedDocument.CreatedDate:dd.MM.yyyy}";
                            }
                        }
                    }
                }
                else
                {
                    if (CreatedDocument.MoveProducts != null && CreatedDocument.MoveProducts.Any())
                    {
                        CreatedDocument.Amount = CreatedDocument.MoveProducts.Sum(d => d.Amount);
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

        private void MoveProductTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Cell.Property == nameof(MoveProduct.ProductId))
                {
                    if (SelectedMoveProduct != null && SelectedMoveProduct.ProductId != 0)
                    {
                        Product product = Products.FirstOrDefault(p => p.Id == SelectedMoveProduct.ProductId);
                        if (product != null)
                        {
                            SelectedMoveProduct.Price = product.PurchasePrice;
                        }
                        SelectedMoveProduct.Quantity = 1;
                        MoveProductTableView.Grid.UpdateTotalSummary();
                        MoveProductTableView.Grid.UpdateGroupSummary();
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
