﻿using DevExpress.XtraReports.UI;
using RetailTradeServer.Dialogs;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Threading.Tasks;
using System.Windows;

namespace SalePageServer.State.Dialogs
{
    public enum DialogResult
    {
        Access,
        Error
    }

    public interface IDialogService
    {
        Task ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : BaseDialogViewModel;
        Task ShowPrintDialog(XtraReport report);
        Task Show<TUserControl>(TUserControl userControl) where TUserControl : BaseDialogUserControl;
        MessageBoxResult ShowMessage(string message);
        MessageBoxResult ShowMessage(string message, string title);
        MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton);
        MessageBoxResult ShowMessage(string message, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage);
        void Close();
    }
}
