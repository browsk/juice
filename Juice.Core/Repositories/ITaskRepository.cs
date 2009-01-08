using System.Collections.Generic;
using Juice.Core.Domain;

namespace Juice.Core.Repositories
{
    public interface ITaskRepository : IRepository<Task, int>
    {
        IEnumerable<Task> GetTasksForProject(Project project);
    }
}
