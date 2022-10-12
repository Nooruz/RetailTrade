using DevExpress.Xpf.PivotGrid;
using System.Globalization;
using System.Windows.Controls;

namespace RetailTradeServer.Validation
{
    public class RequiredValidationRule : ValidationRule
    {
        public static string GetErrorMessage(string fieldName, object fieldValue)
        {
            string errorMessage = string.Empty;
            if (fieldValue != null && int.TryParse(fieldValue.ToString(), out int id))
            {
                if (id == 0)
                    errorMessage = $"\nВы не можете оставить поле \"{fieldName}\" пустым.";
            }
            if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                errorMessage = $"\nВы не можете оставить поле \"{fieldName}\" пустым.";
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
