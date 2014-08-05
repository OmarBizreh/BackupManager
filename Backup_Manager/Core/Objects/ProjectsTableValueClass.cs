using System.ComponentModel;
using System.Windows.Controls;

namespace Backup_Manager.Core.Objects
{
    /// <summary>
    /// This class will include data retreived from ProjectsTable in the database.
    /// Instead of retreiving list of string or dictionaries, a list of this class is returned.
    /// Created and developed by Omar Bizreh.
    /// </summary>
    public class ProjectsTableValueClass : ValidationRule, INotifyPropertyChanged
    {
        private int _projectID;

        public int ProjectID
        {
            get { return _projectID; }
            set { _projectID = value; }
        }

        private string _projectname;

        public string ProjectName
        {
            get
            {
                return _projectname;
            }
            set
            {
                _projectname = value;
                OnPropertyChanged("ProjectName");
            }
        }

        private string _sourcelocation;

        public string SourceLocation
        {
            get
            {
                return _sourcelocation;
            }
            set
            {
                _sourcelocation = value;
                OnPropertyChanged("SourceLocation");
            }
        }

        private string _lastbackup;

        public string LastBackup
        {
            get
            {
                return _lastbackup;
            }
            set
            {
                _lastbackup = value;
                OnPropertyChanged("LastBackup");
            }
        }

        private string _backupdestination;

        public string BackupDestination
        {
            get { return _backupdestination; }
            set
            {
                _backupdestination = value;
                OnPropertyChanged("BackupDestination");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "Requried Field");
            else
                return new ValidationResult(true, null);
        }
    }
}