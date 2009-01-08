using Juice.Core.Domain;
using Juice.Core.Repositories;

namespace Juice.DataAccess.Repositories
{
    public class ProjectRepository : NHibernateRepository<Project, int>, IProjectRepository
    {
    }
}
