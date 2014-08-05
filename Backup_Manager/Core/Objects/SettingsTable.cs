namespace Backup_Manager.Core.Objects
{
    public class SettingsTable
    {
        public static string BackupTime
        {
            get
            {
                return "BackupTime";
            }
        }

        public static string DestinationEmail
        {
            get
            {
                return "DestinationEmail";
            }
        }

        public static string EnableNotifications
        {
            get
            {
                return "EnableNotifcations";
            }
        }

        public static string IncomingMailServer
        {
            get
            {
                return "IncomingMailServer";
            }
        }

        public static string OutgoingMailServer
        {
            get
            {
                return "OutgoingMailServer";
            }
        }

        public static string Password
        {
            get
            {
                return "Password";
            }
        }

        public static string SourceEmail
        {
            get
            {
                return "SourceEmail";
            }
        }

        public static string TableName
        {
            get
            {
                return "SettingsTable";
            }
        }

        public static string Username
        {
            get
            {
                return "Username";
            }
        }
    }
}