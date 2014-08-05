using Backup_Manager.Core.Objects;
using System;

namespace Backup_Manager.Core.Events
{
    /// <summary>
    /// Event class to used when adding new project to database.
    /// Created and developed by Omar Bizreh.
    /// </summary>
    public class NewProjectEventArgs : EventArgs
    {
        private ProjectsTableValueClass _values;

        public ProjectsTableValueClass NewProject
        {
            get { return _values; }
            set { _values = value; }
        }
    }
}