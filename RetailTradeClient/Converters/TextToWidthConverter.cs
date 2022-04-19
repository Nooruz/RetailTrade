using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;

namespace RetailTradeClient.Converters
{
    public class TextToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is System.Windows.Controls.TextBox textBox)
                {
                    try
                    {
                        string fontName = textBox.FontFamily.Source.ToString();
                        float fontSize = (float)textBox.FontSize;
                        System.Drawing.Size size = TextRenderer.MeasureText(textBox.Text, new System.Drawing.Font(fontName, fontSize));
                        return new Thickness(size.Width, 0, 0, 0);
                    }
                    catch (Exception)
                    {
                        //ignore
                    }
                }
            }
            return new Thickness(0, 0, 0, 0);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
