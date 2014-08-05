using Backup_Manager.Core.Database;
using Backup_Manager.Core.Lists;
using Backup_Manager.Core.Objects;
using Backup_Manager.Dialogs;
using System;
using System.Data.SQLite;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Backup_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Created and developed by Omar Bizreh.
    /// </summary>
    public partial class MainWindow : Window
    {
        private Database database;
        private ProjectsList lstProjects;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnConnectDatabase(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            if (e.CurrentState != System.Data.ConnectionState.Open)
                this.UpdateUI(this.btnConnectToDatabase, true);
            else
                this.UpdateUI(this.btnConnectToDatabase, false);
        }

        private void database_ProjectAddedEvent(object sender, Core.Events.NewProjectEventArgs e)
        {
            this.lstProjects.Add(e.NewProject);
            this.UpdateUI(txtLog, "Project Added");
        }

        private void database_ProjectDeletedEvent(object sender, Core.Events.ProjectDeletedEventArgs e)
        {
            this.lstProjects.Remove((ProjectsTableValueClass)lstViewProjects.SelectedItem);
            this.UpdateUI(txtLog, "Project deleted");
        }

        private void GetAllProjects()
        {
            ProjectsList TempList = database.GetProjects();
            if (TempList == null)
                return;

            this.UpdateUI(this.lstViewProjects, TempList);
        }

        private void Initialize()
        {
            txtLog.Text = "Attempting to connect to database\n";
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj =>
            {
                try
                {
                    database = new Database();
                    database.Connection.StateChange += Connection_StateChange;
                    database.ProjectAddedEvent += database_ProjectAddedEvent;
                    database.ProjectDeletedEvent += database_ProjectDeletedEvent;

                    GetAllProjects();
                }
                catch (SQLiteException ex)
                {
                    this.UpdateUI(txtLog, ex.Message);
                }
                catch (Exception ex)
                {
                    this.UpdateUI(txtLog, ex.Message);
                }
                finally
                {
                    if (this.database != null && this.database.IsConnectionOpen)
                    {
                        this.UpdateUI(txtLog, "Connected to database\nDone loading projects");
                        this.UpdateUI(btnConnectToDatabase, false);
                    }
                }
            }));
        }

        private void lstViewButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            try
            {
                System.Diagnostics.Process.Start(tag);
            }
            catch (Exception ex)
            {
                this.UpdateUI(txtLog, string.Join("\n", "Error opening project source location", ex.Message));
            }
        }

        private void lstViewProjects_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (lstViewProjects.SelectedIndex == -1)
                e.Handled = true;
        }

        private void menuDeleteProject_Click(object sender, RoutedEventArgs e)
        {
            if (lstViewProjects.SelectedItem != null)
            {
                ProjectsTableValueClass item = this.lstProjects[this.lstViewProjects.SelectedIndex];
                database.DeleteProject(item.ProjectID);
            }
        }

        private void menuEditProjet_Click(object sender, RoutedEventArgs e)
        {
            ProjectsTableValueClass item = this.lstProjects[this.lstViewProjects.SelectedIndex];
            ProjectDialog dialog = new ProjectDialog();
            DelegateMethods.EditProjects edit = project =>
            {
                database.EditProject(project);
            };
            dialog.ProjectToEdit = item;
            dialog.editProjects = edit;
            dialog.IsEditMode = true;
            dialog.ShowDialog();
        }

        private void menuNewProject_Click(object sender, RoutedEventArgs e)
        {
            ProjectDialog dialog = new ProjectDialog();

            DelegateMethods.AddProjects add = obj =>
            {
                ProjectsList lst = obj as ProjectsList;
                foreach (ProjectsTableValueClass item in lst)
                {
                    database.AddNewProject(item);
                }
            };

            dialog.addProjects = add;

            dialog.ShowDialog();
        }

        private void menuRefreshProjects_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateUI(txtLog, "Refreshing projects list");
            this.lstProjects.Clear();
            GetAllProjects();
            this.UpdateUI(txtLog, "Refresh is complete");
        }

        /// <summary>
        /// Update UI from child threads
        /// </summary>
        /// <param name="target">Target UI Element to update</param>
        /// <param name="value">Message to show in Target UI Element</param>
        private void UpdateUI(object target, object value)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (target is TextBox)
                    (target as TextBox).AppendText("\n" + value + "\n------------------------------");
                else if (target is ListView)
                {
                    ProjectsList TempList = (ProjectsList)value;
                    foreach (var item in TempList)
                        this.lstProjects.Add(item);
                }
                else if (target is Button)
                    (target as Button).IsEnabled = bool.Parse(value.ToString());
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstProjects = (ProjectsList)this.Resources["lstProjects"];
            Initialize();
        }

        private void menuSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsDialog settings = new SettingsDialog();

            settings.settings = database.ReadSettings();

            if (!settings.ShowDialog().Value)
                return;

            if (database.ReadSettings().SourceEmail == null)
                database.AddSettings(settings.settings);
            else
                database.UpdateSettings(settings.settings);

            settings.Close();
        }
    }
}