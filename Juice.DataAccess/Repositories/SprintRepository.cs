using Juice.Core.Domain;
using Juice.Core.Repositories;

namespace Juice.DataAccess.Repositories
{
    public class SprintRepository : NHibernateRepository<Sprint, int>, ISprintRepository
    {
    }
}
