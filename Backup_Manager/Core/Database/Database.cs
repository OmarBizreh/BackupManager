using Backup_Manager.Core.Events;
using Backup_Manager.Core.Lists;
using Backup_Manager.Core.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace Backup_Manager.Core.Database
{
    /// <summary>
    /// This class handles all Database related tasks
    /// 1. Creating database and relevant tables.
    /// 2. Inserting new data.
    /// 3. Editing existing data.
    /// 4. Deleting existing data.
    /// 5. Retrieving existing data.
    /// Created and developed by Omar Bizreh
    /// </summary>
    internal class Database
    {
        private SQLiteConnection _connection;

        private bool _isconnectionopen;

        /// <summary>
        /// Initilize new instance of Database class
        /// </summary>
        /// <exception cref="SQliteConnectionException"/>
        public Database()
        {
            this.Connection = new SQLiteConnection(this.ConnectionString.ConnectionString);
            using (SQLiteCommand Command = new SQLiteCommand(Connection))
            {
                if (!Directory.Exists(this.DirectoryName))
                    Directory.CreateDirectory(this.DirectoryName);

                if (!File.Exists(Path.Combine(this.DirectoryName, this.Name)))
                {
                    SQLiteConnection.CreateFile(Path.Combine(this.DirectoryName, this.Name));
                    this.Connection.Open();
                    this.IsConnectionOpen = true;
                    Command.CommandText = string.Join(" ", "create table", ProjectsTable.TableName, "(",
                        string.Join(" ", ProjectsTable.ProjectID, "integer", "primary", "key,"),
                        string.Join(" ", ProjectsTable.ProjectName, "varchar2(50),"),
                        string.Join(" ", ProjectsTable.BackupDestination, "varchar2(500),"),
                        string.Join(" ", ProjectsTable.SourceLocation, "varchar2(500),"),
                        string.Join(" ", ProjectsTable.LastBackup, "varchar2(50)"), ")");

                    Command.ExecuteNonQuery();

                    Command.CommandText = string.Join(" ", "create", "table", SettingsTable.TableName, "(",
                        string.Join(" ", SettingsTable.BackupTime, "varchar2(10),"),
                        string.Join(" ", SettingsTable.DestinationEmail, "varchar2(50),"),
                        string.Join(" ", SettingsTable.EnableNotifications, "int(1),"),
                        string.Join(" ", SettingsTable.IncomingMailServer, "varchar2(50),"),
                        string.Join(" ", SettingsTable.OutgoingMailServer, "varchar2(50),"),
                        string.Join(" ", SettingsTable.Password, "varchar2(50),"),
                        string.Join(" ", SettingsTable.SourceEmail, "varchar2(50),"),
                        string.Join(" ", SettingsTable.Username, "varchar2(50)"), ")");

                    Command.ExecuteNonQuery();
                }

                try
                {
                    if (!this.IsConnectionOpen)
                    {
                        Connection.Open();
                        this.IsConnectionOpen = true;
                    }
                }
                catch (SQLiteException)
                {
                    this.IsConnectionOpen = false;
                }
            }
        }

        public event DelegateMethods.NewProjectEventHandler ProjectAddedEvent;

        public event DelegateMethods.DeletProjectEventHandler ProjectDeletedEvent;

        public SQLiteConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// Sqlite Connecion string builder
        /// </summary>
        public SQLiteConnectionStringBuilder ConnectionString
        {
            get
            {
                return new SQLiteConnectionStringBuilder(
                    string.Join("", "Data Source=", this.Location, ";Version=3"));
            }
        }

        /// <summary>
        /// Directory where database file is found
        /// </summary>
        public string DirectoryName
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Backup_Manager");
            }
        }

        public bool IsConnectionOpen
        {
            get { return _isconnectionopen; }
            set { _isconnectionopen = value; }
        }

        /// <summary>
        /// Full path of database file (name and directory)
        /// </summary>
        public string Location
        {
            get
            {
                return Path.Combine(this.DirectoryName, this.Name);
            }
        }

        /// <summary>
        /// Database name
        /// </summary>
        public string Name
        {
            get
            {
                return "maindb.db";
            }
        }

        /// <summary>
        /// Adds new project
        /// </summary>
        /// <param name="Values">Project Parameters</param>
        /// <returns>Number of rows added</returns>
        public int AddNewProject(ProjectsTableValueClass project)
        {
            int val = this.AddRows(ProjectsTable.TableName,
                 new string[] { ProjectsTable.ProjectName, ProjectsTable.SourceLocation, ProjectsTable.LastBackup, ProjectsTable.BackupDestination },
                 project.ProjectName,
                 project.SourceLocation,
                 project.LastBackup,
                 project.BackupDestination);

            if (val > 0)
            {
                NewProjectEventArgs args = new NewProjectEventArgs()
                {
                    NewProject = project
                };
                OnProjectAdded(args);
            }
            return val;
        }

        public void DeleteProject(int ProjectID)
        {
            using (SQLiteCommand Command = new SQLiteCommand(this.Connection))
            {
                Command.CommandText = string.Join(" ", "delete", "from",
                ProjectsTable.TableName, "where", ProjectsTable.ProjectID, "=", ProjectID.ToString());
                int val = Command.ExecuteNonQuery();
                if (val > 0)
                    OnProjectDelete(new ProjectDeletedEventArgs());
            }
        }

        public void EditProject(ProjectsTableValueClass project)
        {
            using (SQLiteCommand Command = new SQLiteCommand(this.Connection))
            {
                Command.CommandType = System.Data.CommandType.Text;
                Command.CommandText = string.Join(" ", "update", ProjectsTable.TableName, "set", ProjectsTable.ProjectName, "=", "'" + project.ProjectName + "'", ",", ProjectsTable.SourceLocation, "=", "'" + project.SourceLocation + "'", ",", ProjectsTable.BackupDestination, "=", "'" + project.BackupDestination + "'",
                   "where", ProjectsTable.ProjectID, "=", project.ProjectID);
                int val = Command.ExecuteNonQuery();
            }
        }

        public ProjectsList GetProjects(string condition = "1=1")
        {
            ProjectsList list = new ProjectsList();

            List<string> columnsArray = new List<string> {
                ProjectsTable.ProjectID,
                ProjectsTable.ProjectName,
                ProjectsTable.SourceLocation,
                ProjectsTable.LastBackup,
                ProjectsTable.BackupDestination
            };

            string coloumns = string.Join(",", columnsArray);
            string GetCommand = string.Join(" ", "select", coloumns, "from", ProjectsTable.TableName, "where", condition);

            SQLiteDataReader dreader;
            using (SQLiteCommand comm = new SQLiteCommand(GetCommand, this.Connection))
            {
                comm.CommandType = System.Data.CommandType.Text;
                dreader = comm.ExecuteReader();
            }

            if (!dreader.HasRows)
            {
                dreader.Close();
                dreader.Dispose();
                return null;
            }
            while (dreader.Read())
            {
                list.Add(new ProjectsTableValueClass()
                {
                    ProjectID = dreader.GetInt32(columnsArray.IndexOf(ProjectsTable.ProjectID)),
                    ProjectName = dreader.GetString(columnsArray.IndexOf(ProjectsTable.ProjectName)),
                    SourceLocation = dreader.GetString(columnsArray.IndexOf(ProjectsTable.SourceLocation)),
                    LastBackup = dreader.GetValue(columnsArray.IndexOf(ProjectsTable.LastBackup)).ToString(),
                    BackupDestination = dreader.GetString(columnsArray.IndexOf(ProjectsTable.BackupDestination))
                });
            }
            if (dreader != null)
            {
                dreader.Close();
                dreader.Dispose();
            }
            return list;
        }

        protected void OnProjectAdded(NewProjectEventArgs e)
        {
            ProjectAddedEvent(this, e);
        }

        protected void OnProjectDelete(ProjectDeletedEventArgs e)
        {
            ProjectDeletedEvent(this, e);
        }

        /// <summary>
        /// Add rows to any table in database
        /// </summary>
        /// <param name="Table">Table Name</param>
        /// <param name="Columns">Columns effected</param>
        /// <param name="Values">Columns Values</param>
        /// <returns>Number of rows affected</returns>
        /// <remarks>This method is internally used by Database Class
        /// to create insert commands for any table in the database</remarks>
        private int AddRows(string Table, string[] Columns, params string[] Values)
        {
            if (this.Connection.State != System.Data.ConnectionState.Open)
                return -1;

            string SqlCommand = string.Join(" ", "insert into", Table);

            SqlCommand = string.Join(" ", SqlCommand, "(", string.Join(",", Columns), ")");
            SqlCommand = string.Join(" ", SqlCommand, "values", "(", "'" + string.Join("','", Values) + "'", ")");

            int result = 0;

            using (SQLiteCommand comm = new SQLiteCommand(SqlCommand, this.Connection))
            {
                comm.CommandType = System.Data.CommandType.Text;
                result = comm.ExecuteNonQuery();
            }
            return result;
        }

        public void AddSettings(SettingsTableValueClass settings)
        {
            string columns = string.Join(",", SettingsTable.BackupTime, SettingsTable.EnableNotifications, SettingsTable.SourceEmail, SettingsTable.DestinationEmail, SettingsTable.Username, SettingsTable.Password, SettingsTable.IncomingMailServer, SettingsTable.OutgoingMailServer);

            this.AddRows(SettingsTable.TableName, columns.Split(';'),
                 new string[]{
            settings.BackupTime, settings.EnableNotifications ? "0" : "1", settings.SourceEmail,settings.DestinationEmail,settings.Username,settings.Password,settings.IncomingMailServer,settings.OutgoingMailServer
            });
        }

        public SettingsTableValueClass ReadSettings(string condition = "1=1")
        {
            List<string> SettingsTableColumns = new List<string>()
            {
                SettingsTable.BackupTime,
                SettingsTable.DestinationEmail,
                SettingsTable.EnableNotifications,
                SettingsTable.IncomingMailServer,
                SettingsTable.OutgoingMailServer,
                SettingsTable.Password,
                SettingsTable.SourceEmail,
                SettingsTable.Username
            };

            string command = string.Join(" ", "select", string.Join(",", SettingsTableColumns), "from", SettingsTable.TableName, "where", condition);
            SettingsTableValueClass settings = new SettingsTableValueClass();


            using (SQLiteCommand com = new SQLiteCommand(command, this.Connection))
            {
                using (SQLiteDataReader reader = com.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        settings.BackupTime = reader.GetString(SettingsTableColumns.IndexOf(SettingsTable.BackupTime));
                        settings.DestinationEmail = reader.GetString(SettingsTableColumns.IndexOf(SettingsTable.DestinationEmail));
                        settings.EnableNotifications = reader.GetValue(SettingsTableColumns.IndexOf(SettingsTable.EnableNotifications)).ToString() == "1" ? true : false;
                        settings.IncomingMailServer = reader.GetString(SettingsTableColumns.IndexOf(SettingsTable.IncomingMailServer));
                        settings.OutgoingMailServer = reader.GetString(SettingsTableColumns.IndexOf(SettingsTable.OutgoingMailServer));
                        settings.Password = reader.GetString(SettingsTableColumns.IndexOf(SettingsTable.Password));
                        settings.SourceEmail = reader.GetString(SettingsTableColumns.IndexOf(SettingsTable.SourceEmail));
                        settings.Username = reader.GetString(SettingsTableColumns.IndexOf(SettingsTable.Username));
                    }
                }
            }

            return settings;
        }

        public void UpdateSettings(SettingsTableValueClass updatedSettings)
        {
            string command = string.Join(" ", "update", SettingsTable.TableName, "set",
                SettingsTable.BackupTime, "=", "'" + updatedSettings.BackupTime + "',",
                SettingsTable.DestinationEmail, "=", "'" + updatedSettings.DestinationEmail + "',",
                SettingsTable.EnableNotifications, "=", (updatedSettings.EnableNotifications ? "1" : "0") + ",",
                SettingsTable.IncomingMailServer, "=", "'" + updatedSettings.IncomingMailServer + "',",
                SettingsTable.OutgoingMailServer, "=", "'" + updatedSettings.OutgoingMailServer + "',",
                SettingsTable.Password, "=", "'" + updatedSettings.Password + "',",
                SettingsTable.SourceEmail, "=", "'" + updatedSettings.SourceEmail + "',",
                SettingsTable.Username, "=", "'" + updatedSettings.Username + "'");

            using (SQLiteCommand com = new SQLiteCommand(command, this.Connection))
            {
                com.ExecuteNonQuery();
            }
        }
    }
}