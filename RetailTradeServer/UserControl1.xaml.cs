using RetailTradeServer.Components;
using RetailTradeServer.Views.Menus;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace SalePageServer
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
