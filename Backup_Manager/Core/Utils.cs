using System.Windows.Controls;

namespace Backup_Manager.Core
{
    /// <summary>
    /// Created and developed by Omar Bizreh.
    /// </summary>
    internal class Utils
    {
        /// <summary>
        /// Check if input feild validation returns error.
        /// </summary>
        /// <param name="sender">input feild to check.</param>
        /// <returns>check result</returns>
        public static bool HasError(object sender)
        {
            return (sender as TextBox).GetBindingExpression(TextBox.TextProperty).HasError;
        }

        /// <summary>
        /// Trigger validation for input feild
        /// </summary>
        /// <param name="target">input field to trigger validation for.</param>
        public static void TriggerValidation(object target)
        {
            /* Using sender as object just in-case in future more controls are added to validation check */
            (target as TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}