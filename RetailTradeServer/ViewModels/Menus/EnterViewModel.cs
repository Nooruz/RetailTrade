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
    public class EnterViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMenuViewModelFactory _menuViewModelFactory;
        private readonly IDocumentService _documentService;
        private readonly IProductService _productService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IUserStore _userStore;
        private readonly IMessageStore _messageStore;
        private ObservableCollection<EnterDocumentView> _documents = new();
        private EnterDocumentView _selectedEnterDocumentView;

        #endregion

        #region Public Properties

        public ObservableCollection<EnterDocumentView> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }
        public EnterDocumentView SelectedEnterDocumentView
        {
            get => _selectedEnterDocumentView;
            set
            {
                _selectedEnterDocumentView = value;
                OnPropertyChanged(nameof(SelectedEnterDocumentView));
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public EnterViewModel(IMenuNavigator menuNavigator,
            IMenuViewModelFactory menuViewModelFactory,
            IDocumentService documentService,
            IProductService productService,
            IWareHouseService wareHouseService,
            IUserStore userStore,
            IMessageStore messageStore)
        {
            _menuNavigator = menuNavigator;
            _menuViewModelFactory = menuViewModelFactory;
            _documentService = documentService;
            _productService = productService;
            _wareHouseService = wareHouseService;
            _userStore = userStore;
            _messageStore = messageStore;

            CreateCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.EnterProduct));
            _documentService.OnEnterCreated += DocumentService_OnEnterCreated;
            _documentService.OnEnterUpdated += DocumentService_OnEnterUpdated;

            GetData();
        }

        #endregion

        #region Private Voids

        private void DocumentService_OnEnterUpdated(EnterDocumentView updatedEnterDocumentView)
        {
            try
            {
                EnterDocumentView enterDocumentView = Documents.FirstOrDefault(d => d.Id == updatedEnterDocumentView.Id);
                enterDocumentView.WareHouse = updatedEnterDocumentView.WareHouse;
                enterDocumentView.Username = updatedEnterDocumentView.Username;
                enterDocumentView.Amount = updatedEnterDocumentView.Amount;
                enterDocumentView.CreatedDate = updatedEnterDocumentView.CreatedDate;
                enterDocumentView.Number = updatedEnterDocumentView.Number;
                enterDocumentView.Comment = updatedEnterDocumentView.Comment;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void DocumentService_OnEnterCreated(EnterDocumentView enterDocumentView)
        {
            try
            {
                Documents.Add(enterDocumentView);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void GetData()
        {
            Documents = new(await _documentService.GetEnterDocumentViews());
        }

        #endregion

        #region Public Voids

        [Command]
        public void UserControlLoaded()
        {
            Header = "Оприходования";
            ShowLoadingPanel = false;
        }

        [Command]
        public async void EditEnterDocument()
        {
            try
            {
                if (SelectedEnterDocumentView != null)
                {
                    _menuNavigator.CurrentViewModel = new EnterProductViewModel(_productService, _wareHouseService, _documentService, _userStore, _messageStore)
                    {
                        Header = $"Оприходование №{SelectedEnterDocumentView.Number} от {SelectedEnterDocumentView.CreatedDate:dd.MM.yyyy}",
                        CreatedDocument = await _documentService.GetIncludeEnterProduct(SelectedEnterDocumentView.Id)
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
                if (SelectedEnterDocumentView != null)
                {
                    if (MessageBoxService.ShowMessage("Удалить выбранный элемент?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        if (await _documentService.DeleteAsync(SelectedEnterDocumentView.Id))
                        {
                            _ = Documents.Remove(SelectedEnterDocumentView);
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
    }
}
