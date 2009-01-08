using System.Collections.Generic;
using Juice.Core.Domain;
using Juice.Core.Repositories;

namespace Juice.DataAccess.Repositories
{
    public class TaskRepository : NHibernateRepository<Task, int>, ITaskRepository
    {
        public IEnumerable<Task> GetTasksForProject(Project project)
        {
            return base.FindAll(new Dictionary<string, object>
                                    {
                                        {"Project", project}
                                    });
        }
    }
}
