using RetailTrade.Domain.Models;
using RetailTradeClient.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RetailTradeClient.ViewModels.Dialogs
{
    public class PostponeReceiptViewModel : BaseDialogViewModel
    {
        #region Private Members

        private PostponeReceipt _selectedPostponeReceipt;

        #endregion

        #region Public Properties

        public ObservableCollection<PostponeReceipt> PostponeReceipts { get; set; }
        public PostponeReceipt SelectedPostponeReceipt
        {
            get => _selectedPostponeReceipt;
            set
            {
                _selectedPostponeReceipt = value;
                OnPropertyChanged(nameof(SelectedPostponeReceipt));
            }
        }

        #endregion

        public event Action<PostponeReceipt> OnResume;

        #region Commands

        public ICommand ResumeReceiptCommand => new RelayCommand(ResumeReceipt);
        public ICommand RowDoubleClickCommand => new RelayCommand(ResumeReceipt);

        #endregion

        #region Constructor

        public PostponeReceiptViewModel()
        {

        }

        #endregion

        #region Private Voids

        private void ResumeReceipt()
        {
            if (SelectedPostponeReceipt != null)
            {
                try
                {
                    OnResume?.Invoke(SelectedPostponeReceipt);
                    CurrentWindowService.Close();
                    PostponeReceipts.Remove(SelectedPostponeReceipt);
                }
                catch (Exception)
                {
                    //ignore
                }                
            }
        }

        #endregion
    }
}
