using RetailTradeClient.Commands;
using RetailTradeClient.ViewModels;
using RetailTradeClient.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailTradeClient.Dialogs
{
    public class BaseDialogUserControl : UserControl
    {
        #region Private Members

        private Regex regex;

        #endregion

        #region Public Properties

        public int WindowMinimumWidth { get; set; } = 250;
        public int WindowMinimumHeight { get; set; } = 100;
        public int TitleHeight { get; set; } = 30;
        public string Title { get; set; }
        public DialogWindow Window { get; set; }

        #endregion

        #region Commands

        public ICommand CloseCommand { get; private set; }

        #endregion

        #region Constructor

        public BaseDialogUserControl()
        {
            Window = new DialogWindow();
            Window.ViewModel = new DialogWindowViewModel(Window);
            CloseCommand = new RelayCommand(Close);
            regex = new Regex("[^0-9]+");
        }

        #endregion

        #region Public Dialog Show Methods

        public Task<bool> ShowDialog<T>(T viewModel)
            where T : BaseDialogViewModel
        {
            var tcs = new TaskCompletionSource<bool>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    Window.ViewModel.Title = string.IsNullOrEmpty(viewModel.Title) ? Title : viewModel.Title;
                    Window.ViewModel.Content = this;


                    DataContext = viewModel;

                    Window.ShowDialog();
                }
                finally
                {
                    tcs.TrySetResult(viewModel.Result);
                }
            });

            return tcs.Task;
        }

        #endregion

        #region Private Voids

        private void Close()
        {
            Window.Close();
        }

        #endregion

        #region TextBox_PreviewTextInput

        public void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion
    }
}
