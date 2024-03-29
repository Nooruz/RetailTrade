﻿using RetailTradeServer.Commands;
using RetailTradeServer.ViewModels.Dialogs.Base;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RetailTradeServer.ViewModels.Dialogs
{
    public class MessageBoxDialogViewModel : BaseDialogViewModel
    {
        public string Message { get; set; }
        public string Icon { get; set; }
        public MessageBoxResult MessageBoxResult { get; set; }
        public List<UIButton> Buttons { get; set; }
        public Window DialogWindow { get; set; }

        public MessageBoxDialogViewModel(string message)
        {
            Buttons = new List<UIButton>();
            Message = message;

            Buttons.Add(new UIButton
            {
                Content = "Ок",
                Command = new RelayCommand(Ok)
            });
        }

        public MessageBoxDialogViewModel(string message, string title)
        {
            Buttons = new List<UIButton>();
            Message = message;
            Title = title;

            Buttons.Add(new UIButton
            {
                Content = "Ок",
                Command = new RelayCommand(Ok)
            });
        }

        public MessageBoxDialogViewModel(string message,
            string title, MessageBoxButton messageBoxButton)
        {
            Buttons = new List<UIButton>();
            Message = message;
            Title = title;
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    Buttons.Add(new UIButton
                    {
                        Content = "Ок",
                        Command = new RelayCommand(Ok)
                    });
                    break;
                case MessageBoxButton.OKCancel:
                    Buttons.AddRange(new List<UIButton>
                    {
                        new UIButton
                        {
                            Content = "Ок",
                            Command = new RelayCommand(Ok)
                        },
                        new UIButton
                        {
                            Content = "Отмена",
                            Command = new RelayCommand(Cancel)
                        }
                    });
                    break;
                case MessageBoxButton.YesNo:
                    Buttons.AddRange(new List<UIButton>
                    {
                        new UIButton
                        {
                            Content = "Да",
                            Command = new RelayCommand(Yes)
                        },
                        new UIButton
                        {
                            Content = "Нет",
                            Command = new RelayCommand(No)
                        }
                    });
                    break;
                case MessageBoxButton.YesNoCancel:
                    Buttons.AddRange(new List<UIButton>
                    {
                        new UIButton
                        {
                            Content = "Да",
                            Command = new RelayCommand(Yes)
                        },
                        new UIButton
                        {
                            Content = "Нет",
                            Command = new RelayCommand(No)
                        },
                        new UIButton
                        {
                            Content = "Отмена",
                            Command = new RelayCommand(Cancel)
                        }
                    });
                    break;
            }
        }

        public MessageBoxDialogViewModel(string message,
            string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            Buttons = new List<UIButton>();
            Message = message;
            Title = title;
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    Buttons.Add(new UIButton
                    {
                        Content = "Ок",
                        Command = new RelayCommand(Ok)
                    });
                    break;
                case MessageBoxButton.OKCancel:
                    Buttons.AddRange(new List<UIButton>
                    {
                        new UIButton
                        {
                            Content = "Ок",
                            Command = new RelayCommand(Ok)
                        },
                        new UIButton
                        {
                            Content = "Отмена",
                            Command = new RelayCommand(Cancel)
                        }
                    });
                    break;
                case MessageBoxButton.YesNo:
                    Buttons.AddRange(new List<UIButton>
                    {
                        new UIButton
                        {
                            Content = "Да",
                            Command = new RelayCommand(Yes)
                        },
                        new UIButton
                        {
                            Content = "Нет",
                            Command = new RelayCommand(No)
                        }
                    });
                    break;
                case MessageBoxButton.YesNoCancel:
                    Buttons.AddRange(new List<UIButton>
                    {
                        new UIButton
                        {
                            Content = "Да",
                            Command = new RelayCommand(Yes)
                        },
                        new UIButton
                        {
                            Content = "Нет",
                            Command = new RelayCommand(No)
                        },
                        new UIButton
                        {
                            Content = "Отмена",
                            Command = new RelayCommand(Cancel)
                        }
                    });
                    break;
            }

            switch (messageBoxImage)
            {
                case MessageBoxImage.Error:
                    Icon = "\uf00d";
                    break;
                case MessageBoxImage.Exclamation:
                    Icon = "\uf12a";
                    break;
                case MessageBoxImage.Information:
                    Icon = "\uf129";
                    break;
                case MessageBoxImage.Question:
                    Icon = "\uf059";
                    break;
            }
        }        

        #region Private Voids

        private void Ok()
        {
            MessageBoxResult = MessageBoxResult.OK;
            DialogWindow?.Close();
        }

        private void Cancel()
        {
            MessageBoxResult = MessageBoxResult.Cancel;
            DialogWindow?.Close();
        }

        private void Yes()
        {
            MessageBoxResult = MessageBoxResult.Yes;
            DialogWindow?.Close();
        }

        private void No()
        {
            MessageBoxResult = MessageBoxResult.No;
            DialogWindow?.Close();
        }

        #endregion
    }

    public class UIButton : Button
    {
        public UIButton()
        {
            //Style = FindResource("btnPrimeryButton") as Style;
            MinWidth = 80;
            Margin = new Thickness(5, 0, 0, 0);
        }
    }
}
