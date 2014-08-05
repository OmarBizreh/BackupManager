using Backup_Manager.Core.Objects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Backup_Manager.Core.Lists
{
    /// <summary>
    /// Observable Collection class to be used to add projects found in database.
    /// Still work in progress.
    /// Created and developed by Omar Bizreh.
    /// </summary>
    public class ProjectsList : ObservableCollection<ProjectsTableValueClass>
    {
        public List<ProjectsTableValueClass> GetProject(int ProjectID)
        {
            var q = from project in this.Items
                    where project.ProjectID == ProjectID
                    select project;

            return q.ToList<ProjectsTableValueClass>();
        }

        public List<ProjectsTableValueClass> GetProject(string ProjectTitle)
        {
            var q = from project in this.Items
                    where project.ProjectName.ToLower().Contains(ProjectTitle.ToLower())
                    select project;

            return q.ToList<ProjectsTableValueClass>();
        }

        public List<ProjectsTableValueClass> GetPRoject(string LastBackup)
        {
            var q = from project in this.Items
                    where project.LastBackup == LastBackup
                    select project;

            return q.ToList<ProjectsTableValueClass>();
        }
    }
}