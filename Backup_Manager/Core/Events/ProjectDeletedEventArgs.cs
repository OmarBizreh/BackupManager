using Backup_Manager.Core.Objects;
using System;

namespace Backup_Manager.Core.Events
{
    /// <summary>
    /// Event class to used when deleting a project to database.
    /// Created and developed by Omar Bizreh.
    /// </summary>
    public class ProjectDeletedEventArgs : EventArgs
    {
        private ProjectsTableValueClass _project;

        public ProjectsTableValueClass DeletedProject
        {
            get { return _project; }
            set { _project = value; }
        }
    }
}