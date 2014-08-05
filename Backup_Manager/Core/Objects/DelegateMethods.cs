using Backup_Manager.Core.Events;

namespace Backup_Manager.Core.Objects
{
    /// <summary>
    /// Class to include all delegate methods used by Backup Manager.
    /// Created and developed by Omar Bizreh.
    /// </summary>
    public class DelegateMethods
    {
        public delegate void AddProjects(object obj);

        public delegate void EditProjects(ProjectsTableValueClass proj);

        public delegate void NewProjectEventHandler(object sender, NewProjectEventArgs e);

        public delegate void DeletProjectEventHandler(object sender, ProjectDeletedEventArgs e);
    }
}