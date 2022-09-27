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
        private ObservableCollection<DocumentView> _documents = new();
        private DocumentView _selectedDocumentView;

        #endregion

        #region Public Properties

        public ObservableCollection<DocumentView> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }
        public DocumentView SelectedDocumentView
        {
            get => _selectedDocumentView;
            set
            {
                _selectedDocumentView = value;
                OnPropertyChanged(nameof(SelectedDocumentView));
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
            _documentService.OnCreated += DocumentService_OnCreated;
            _documentService.OnUpdated += DocumentService_OnUpdated;

            GetData();
        }

        #endregion

        #region Private Voids

        private void DocumentService_OnCreated(DocumentView updatedDocumentView, DocumentTypeEnum documentTypeEnum)
        {
            try
            {
                if (documentTypeEnum == DocumentTypeEnum.Enter)
                {
                    DocumentView documentView = Documents.FirstOrDefault(d => d.Id == updatedDocumentView.Id);
                    documentView.WareHouse = updatedDocumentView.WareHouse;
                    documentView.Username = updatedDocumentView.Username;
                    documentView.Amount = updatedDocumentView.Amount;
                    documentView.CreatedDate = updatedDocumentView.CreatedDate;
                    documentView.Number = updatedDocumentView.Number;
                    documentView.Comment = updatedDocumentView.Comment;
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void DocumentService_OnUpdated(DocumentView documentView, DocumentTypeEnum documentTypeEnum)
        {
            try
            {
                if (documentTypeEnum == DocumentTypeEnum.Enter)
                {
                    Documents.Add(documentView);
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private async void GetData()
        {
            Documents = new(await _documentService.GetDocumentViews(DocumentTypeEnum.Enter));
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
                if (SelectedDocumentView != null)
                {
                    _menuNavigator.CurrentViewModel = new EnterProductViewModel(_productService, _wareHouseService, _documentService, _userStore, _messageStore)
                    {
                        Header = $"Оприходование №{SelectedDocumentView.Number} от {SelectedDocumentView.CreatedDate:dd.MM.yyyy}",
                        CreatedDocument = await _documentService.GetDocumentByIncludeAsync(SelectedDocumentView.Id)
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
                if (SelectedDocumentView != null)
                {
                    if (MessageBoxService.ShowMessage("Удалить выбранный элемент?", "Sale Page", MessageButton.YesNo, MessageIcon.Question) == MessageResult.Yes)
                    {
                        if (await _documentService.DeleteAsync(SelectedDocumentView.Id))
                        {
                            _ = Documents.Remove(SelectedDocumentView);
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
