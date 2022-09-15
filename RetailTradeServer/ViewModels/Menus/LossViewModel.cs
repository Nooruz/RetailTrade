using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using RetailTrade.Domain.Services;
using RetailTrade.Domain.Views;
using RetailTradeServer.Commands;
using RetailTradeServer.State.Messages;
using RetailTradeServer.State.Navigators;
using RetailTradeServer.State.Users;
using RetailTradeServer.ViewModels.Base;
using RetailTradeServer.ViewModels.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RetailTradeServer.ViewModels.Menus
{
    public class LossViewModel : BaseViewModel
    {
        #region Private members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMenuViewModelFactory _menuViewModelFactory;
        private readonly IDocumentService _documentService;
        private readonly IProductService _productService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IUserStore _userStore;
        private readonly IMessageStore _messageStore;
        private DocumentProductView _selectedLossDocumentView;
        private ObservableCollection<DocumentProductView> _documents;

        #endregion

        #region Public properties

        public ObservableCollection<DocumentProductView> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }
        public DocumentProductView SelectedLossDocumentView
        {
            get => _selectedLossDocumentView;
            set
            {
                _selectedLossDocumentView = value;
                OnPropertyChanged(nameof(SelectedLossDocumentView));
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public LossViewModel(IProductService productService,
            IDocumentService documentService,
            IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IWareHouseService wareHouseService,
            IUserStore userStore,
            IMessageStore messageStore)
        { 
            _productService = productService;
            _documentService = documentService;
            _menuNavigator = menuNavigator;
            _menuViewModelFactory = menuViewModelFactory;
            _wareHouseService = wareHouseService;
            _userStore = userStore;
            _messageStore = messageStore;

            Header = "Списание товара";

            CreateCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.LossProduct));
            //_documentService.OnLossCreated += DocumentService_OnLossCreated;
            //_documentService.OnLossUpdated += DocumentService_OnLossUpdated;

            GetData();
        }

        #endregion

        #region Private Voids

        private void DocumentService_OnLossUpdated(DocumentProductView updatedLossDocumentView)
        {
            try
            {
                DocumentProductView lossDocumentView = Documents.FirstOrDefault(d => d.Id == updatedLossDocumentView.Id);
                lossDocumentView.WareHouse = updatedLossDocumentView.WareHouse;
                lossDocumentView.Username = updatedLossDocumentView.Username;
                lossDocumentView.Amount = updatedLossDocumentView.Amount;
                lossDocumentView.Comment = updatedLossDocumentView.Comment;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void DocumentService_OnLossCreated(DocumentProductView lossDocumentView)
        {
            try
            {
                Documents.Add(lossDocumentView);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void GetData()
        {
            try
            {
                //Documents = new(await _documentService.GetLossDocumentViews());
            }
            catch (Exception)
            {
                //ignore
            }
        }

        #endregion

        #region Public Voids

        [Command]
        public async void EditLoss()
        {
            try
            {
                if (SelectedLossDocumentView != null)
                {
                    _menuNavigator.CurrentViewModel = new LossProductViewModel(_productService, _wareHouseService, _documentService, _userStore, _messageStore)
                    {
                        Header = $"Спиание №{SelectedLossDocumentView.Number} от {SelectedLossDocumentView.CreatedDate:dd.MM.yyyy}",
                        //CreatedDocument = await _documentService.GetIncludeLossProduct(SelectedLossDocumentView.Id)
                    };
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void DeleteEnterDocument()
        {
            try
            {
                if (SelectedLossDocumentView != null)
                {
                    if (MessageBoxService.ShowMessage("Удалить выбранный элемент?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        if (await _documentService.DeleteAsync(SelectedLossDocumentView.Id))
                        {
                            _ = Documents.Remove(SelectedLossDocumentView);
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

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
