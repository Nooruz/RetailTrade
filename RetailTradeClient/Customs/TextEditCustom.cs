using DevExpress.Xpf.Editors;
using System;
using System.Windows;

namespace RetailTradeClient.Customs
{
    public class TextEditCustom : TextEdit
    {
        #region Dependency Properties

        public static readonly DependencyProperty UnitNameProperty =
            DependencyProperty.Register(nameof(UnitName), typeof(string), typeof(TextEditCustom), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnUnitNameChanged)));

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

        private static void OnUnitNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (d is TextEdit textEdit)
                {
                    
                    textEdit.DisplayFormatString = new string(textEdit.Text);
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
