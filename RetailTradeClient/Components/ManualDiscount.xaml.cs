using RetailTradeClient.Commands;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RetailTradeClient.Components
{
    /// <summary>
    /// Логика взаимодействия для ManualDiscount.xaml
    /// </summary>
    public partial class ManualDiscount : UserControl, INotifyPropertyChanged
    {
        #region Public Properties

        public Visibility ManualVisibility
        {
            get => (Visibility)GetValue(ManualVisibilityProperty);
            set
            {
                SetValue(ManualVisibilityProperty, value);
                OnPropertyChanged(nameof(ManualVisibility));
                OnPropertyChanged(nameof(ProductVisibility));
            }
        }

        public Visibility ProductVisibility => ManualVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

        #endregion

        #region Commands

        public ICommand ManualDiscountCloseCommand => new RelayCommand(ManualDiscountClose);

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ManualVisibilityProperty =
            DependencyProperty.Register(nameof(ManualVisibility), typeof(Visibility), typeof(ManualDiscount), new UIPropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(OnManualVisibilityChanged)));

        #endregion

        #region Constructor

        public ManualDiscount()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion

        #region Private Voids

        private static void OnManualVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (d is ManualDiscount manualDiscount)
                {
                    Visibility oldValue = (Visibility)e.OldValue;
                    Visibility newValue = (Visibility)e.NewValue;

                    if (oldValue != newValue)
                    {
                        manualDiscount.ManualDiscountClose();
                    }
                }
            }
            catch (Exception)
            {
                //ignore
            }
        }

        public void ManualDiscountClose()
        {
            ManualVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
