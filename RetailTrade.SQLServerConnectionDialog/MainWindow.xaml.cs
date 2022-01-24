using RetailTrade.SQLServerConnectionDialog.ViewModels;
using System.Windows;

namespace RetailTrade.SQLServerConnectionDialog
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ConnectionViewModel CurrentViewModel { get; set; }
        public MainWindow()
        {            
            InitializeComponent();
            DataContext = this;
        }
    }
}
