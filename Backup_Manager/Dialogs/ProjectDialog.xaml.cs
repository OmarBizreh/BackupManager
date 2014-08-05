using Backup_Manager.Core;
using Backup_Manager.Core.Lists;
using Backup_Manager.Core.Objects;
using System.Windows;
using System.Windows.Controls;

namespace Backup_Manager.Dialogs
{
    /// <summary>
    /// Interaction logic for NewProjectDialog.xaml
    /// Created and developed by Omar Bizreh.
    /// </summary>
    public partial class ProjectDialog : Window
    {
        internal DelegateMethods.AddProjects addProjects;

        internal DelegateMethods.EditProjects editProjects;

        internal bool IsEditMode = false;

        internal ProjectsTableValueClass ProjectToEdit;

        public ProjectDialog()
        {
            InitializeComponent();
        }

        internal ProjectsList Projects { get; set; }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            BrowseFolder(this.txtSourceLocation);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Utils.TriggerValidation(this.txtProjectName);
            Utils.TriggerValidation(this.txtSourceLocation);
            Utils.TriggerValidation(this.txtBackupDestination);

            if (Utils.HasError(this.txtProjectName) || Utils.HasError(this.txtSourceLocation))
                return;

            if (!IsEditMode)
            {
                this.Projects.Add(new Core.Objects.ProjectsTableValueClass()
                {
                    ProjectName = this.txtProjectName.Text,
                    SourceLocation = this.txtSourceLocation.Text,
                    LastBackup = "--",
                    BackupDestination = this.txtBackupDestination.Text
                });
                this.addProjects(this.Projects);
            }
            else
            {
                this.ProjectToEdit.ProjectName = this.txtProjectName.Text;
                this.ProjectToEdit.SourceLocation = this.txtSourceLocation.Text;
                this.ProjectToEdit.BackupDestination = this.txtBackupDestination.Text;
                this.editProjects(this.ProjectToEdit);
            }

            this.Close();
        }

        private void txtProjectName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Utils.TriggerValidation(this.txtProjectName);
        }

        private void txtSourceLocation_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Utils.TriggerValidation(this.txtSourceLocation);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Projects = new ProjectsList();

            if (this.IsEditMode)
            {
                this.txtProjectName.Text = this.ProjectToEdit.ProjectName;
                this.txtSourceLocation.Text = this.ProjectToEdit.SourceLocation;
                this.txtBackupDestination.Text = this.ProjectToEdit.BackupDestination;
            }

            this.txtProjectName.Focus();
        }

        private void btnBackupBrowse_Click(object sender, RoutedEventArgs e)
        {
            BrowseFolder(this.txtBackupDestination);
        }

        private void BrowseFolder(TextBox target)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            target.Text = dialog.SelectedPath;
        }

        private void txtBackupDestination_TextChanged(object sender, TextChangedEventArgs e)
        {
            Utils.TriggerValidation(this.txtBackupDestination);
        }
    }
}