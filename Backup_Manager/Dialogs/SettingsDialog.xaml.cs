using Backup_Manager.Core.Objects;
using System.Windows;

namespace Backup_Manager.Dialogs
{
    /// <summary>
    /// Interaction logic for SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        internal SettingsTableValueClass settings { get; set; }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.settings = grd.DataContext as SettingsTableValueClass;
            this.settings.BackupTime = string.Join(":", txtHour.Text, txtMinutes.Text, txtSeconds.Text);

            this.DialogResult = true;
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.settings != null)
            {
                this.grd.DataContext = settings;
                this.txtHour.Text = this.settings.BackupTime.Split(':')[0];
                this.txtMinutes.Text = this.settings.BackupTime.Split(':')[1];
                this.txtSeconds.Text = this.settings.BackupTime.Split(':')[2];
            }
        }
    }
}