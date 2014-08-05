namespace Backup_Manager.Core.Objects
{
    /// <summary>
    /// Description class of Projects table in the database.
    /// All Insert, create, delete SQL commands use fields below instead of hardcoding into "Database.cs" />
    /// Created and developed by Omar Bizreh.
    /// </summary>
    internal class ProjectsTable
    {
        public static string TableName
        {
            get
            {
                return "Projects";
            }
        }

        public static string ProjectID
        {
            get
            {
                return "ProjectID";
            }
        }

        public static string ProjectName
        {
            get
            {
                return "ProjectName";
            }
        }

        public static string SourceLocation
        {
            get
            {
                return "SourceLocation";
            }
        }

        public static string LastBackup
        {
            get
            {
                return "LastBackup";
            }
        }

        public static string BackupDestination
        {
            get { return "BackupDestination"; }
        }
    }
}