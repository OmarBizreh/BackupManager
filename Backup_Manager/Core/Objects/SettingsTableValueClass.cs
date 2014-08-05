using System.ComponentModel;

namespace Backup_Manager.Core.Objects
{
    internal class SettingsTableValueClass : INotifyPropertyChanged
    {
        private string _backupTime;

        private string _destinationemail;

        private bool _enableNotifications;

        private string _incomingserver;

        private string _outgoingmail;

        private string _password;

        private string _sourceemail;

        private string _username;

        public event PropertyChangedEventHandler PropertyChanged;

        public string BackupTime
        {
            get { return _backupTime; }
            set
            {
                _backupTime = value;
                OnPropertyChanged("BackupTime");
            }
        }

        public string DestinationEmail
        {
            get { return _destinationemail; }
            set
            {
                _destinationemail = value;
                OnPropertyChanged("DestinationEmail");
            }
        }

        public bool EnableNotifications
        {
            get { return _enableNotifications; }
            set
            {
                _enableNotifications = value;
                OnPropertyChanged("EnableNotifications");
            }
        }

        public string IncomingMailServer
        {
            get { return _incomingserver; }
            set
            {
                _incomingserver = value;
                OnPropertyChanged("IncomingMailServer");
            }
        }

        public string OutgoingMailServer
        {
            get { return _outgoingmail; }
            set
            {
                _outgoingmail = value;
                OnPropertyChanged("OutgoingMailServer");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string SourceEmail
        {
            get { return _sourceemail; }
            set
            {
                _sourceemail = value;
                OnPropertyChanged("SourceEmail");
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }
}