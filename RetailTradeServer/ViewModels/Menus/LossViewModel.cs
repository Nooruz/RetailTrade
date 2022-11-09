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
        private DocumentView _selectedLossDocumentView;
        private ObservableCollection<DocumentView> _documents;

        #endregion

        #region Public properties

        public ObservableCollection<DocumentView> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                OnPropertyChanged(nameof(Documents));
            }
        }
        public DocumentView SelectedLossDocumentView
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
            _documentService.OnCreated += DocumentService_OnCreated;
            _documentService.OnUpdated += DocumentService_OnUpdated;

            GetData();
        }

        #endregion

        #region Private Voids

        private void DocumentService_OnCreated(DocumentView documentView, DocumentTypeEnum documentTypeEnum)
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

        private void DocumentService_OnUpdated(DocumentView updatedDocumentView, DocumentTypeEnum documentTypeEnum)
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

        private async void GetData()
        {
            try
            {
                Documents = new(await _documentService.GetDocumentViews(DocumentTypeEnum.Loss));
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
                        CreatedDocument = await _documentService.GetDocumentByIncludeAsync(SelectedLossDocumentView.Id)
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
