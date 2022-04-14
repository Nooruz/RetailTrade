using DevExpress.Xpf.Editors;
using System.Windows;

namespace RetailTradeClient.Customs
{
    public class TextEditCustom : TextEdit
    {
        #region Dependency Properties

        public static readonly DependencyProperty UnitNameProperty =
            DependencyProperty.Register(nameof(UnitName), typeof(string), typeof(TextEditCustom), new PropertyMetadata(string.Empty), new FrameworkPropertyMetadata(), new ValidateValueCallback(ValidateUnitName));

        #endregion

        #region Public Properties

        public string UnitName
        {
            get => (string)GetValue(UnitNameProperty);
            set => SetValue(UnitNameProperty, value);
        }

        #endregion

        #region Constructor

        public TextEditCustom()
        {
            
        }

        #endregion

        #region Private Voids

        public static void ValidateUnitName(string value)
        {

        }

        #endregion
    }
}
