using System.Globalization;
using System.Windows.Controls;

namespace RetailTradeServer.Validation
{
    public class RequiredValidationRule : ValidationRule
    {
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = $"Вы не можете оставить поле \"{fieldName}\" пустым.";
            if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                errorMessage = $"Вы не можете оставить поле \"{fieldName}\" пустым.";
            return errorMessage;
        }

        public string FieldName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }
    }
}
