using System.Windows.Controls;

namespace Backup_Manager.Core.Validators
{
    /// <summary>
    /// Class for validating input fields to make sure they are not empty.
    /// Created and developed by Omar Bizreh.
    /// </summary>
    internal class CheckNull : ValidationRule
    {
        private string _val;

        public string Val
        {
            get { return _val; }
            set { _val = value; }
        }

        public string Val2 { get; set; }

        public string Val3 { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Trim().Length == 0)
                return new ValidationResult(false, "Required field");
            else
                return new ValidationResult(true, null);
        }
    }
}