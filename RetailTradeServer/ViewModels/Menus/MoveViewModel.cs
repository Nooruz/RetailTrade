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
    public class MoveViewModel : BaseViewModel
    {
        #region Private Members

        private readonly IMenuNavigator _menuNavigator;
        private readonly IMenuViewModelFactory _menuViewModelFactory;
        private readonly IDocumentService _documentService;
        private readonly IProductService _productService;
        private readonly IWareHouseService _wareHouseService;
        private readonly IUserStore _userStore;
        private readonly IMessageStore _messageStore;
        private ObservableCollection<DocumentProductView> _documents = new();
        private DocumentProductView _selectedMoveDocumentView;

        #endregion

        #region Public Properties

        public ObservableCollection<DocumentProductView> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }
        public DocumentProductView SelectedMoveDocumentView
        {
            get => _selectedMoveDocumentView;
            set
            {
                _selectedMoveDocumentView = value;
                OnPropertyChanged(nameof(SelectedMoveDocumentView));
            }
        }

        #endregion

        #region Commands

        public ICommand UpdateCurrentMenuViewModelCommand => new UpdateCurrentMenuViewModelCommand(_menuNavigator, _menuViewModelFactory);

        #endregion

        #region Constructor

        public MoveViewModel(IMenuNavigator menuNavigator,
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

            CreateCommand = new RelayCommand(() => UpdateCurrentMenuViewModelCommand.Execute(MenuViewType.MoveProduct));
            //_documentService.OnMoveCreated += DocumentService_OnMoveCreated;
            //_documentService.OnMoveUpdated += DocumentService_OnMoveUpdated;

            GetData();
        }

        #endregion

        #region Private Voids

        private void DocumentService_OnMoveUpdated(DocumentProductView updatedMoveDocumentView)
        {
            try
            {
                DocumentProductView enterDocumentView = Documents.FirstOrDefault(d => d.Id == updatedMoveDocumentView.Id);
                enterDocumentView.WareHouse = updatedMoveDocumentView.WareHouse;
                enterDocumentView.Username = updatedMoveDocumentView.Username;
                enterDocumentView.Amount = updatedMoveDocumentView.Amount;
                enterDocumentView.CreatedDate = updatedMoveDocumentView.CreatedDate;
                enterDocumentView.Number = updatedMoveDocumentView.Number;
                enterDocumentView.Comment = updatedMoveDocumentView.Comment;
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void DocumentService_OnMoveCreated(DocumentProductView enterDocumentView)
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
            //Documents = new(await _documentService.GetMoveDocumentViews());
        }

        #endregion

        #region Public Voids

        [Command]
        public void UserControlLoaded()
        {
            Header = "Перемещения";
            ShowLoadingPanel = false;
        }

        [Command]
        public async void EditMove()
        {
            try
            {
                if (SelectedMoveDocumentView != null)
                {
                    _menuNavigator.CurrentViewModel = new MoveProductViewModel(_productService, _wareHouseService, _documentService, _userStore, _messageStore)
                    {
                        Header = $"Перемещение №{SelectedMoveDocumentView.Number} от {SelectedMoveDocumentView.CreatedDate:dd.MM.yyyy}",
                        //CreatedDocument = await _documentService.GetIncludeMoveProduct(SelectedMoveDocumentView.Id)
                    };
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        [Command]
        public async void DeleteMoveDocument()
        {
            try
            {
                if (SelectedMoveDocumentView != null)
                {
                    if (MessageBoxService.ShowMessage("Удалить выбранный элемент?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        if (await _documentService.DeleteAsync(SelectedMoveDocumentView.Id))
                        {
                            _ = Documents.Remove(SelectedMoveDocumentView);
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
