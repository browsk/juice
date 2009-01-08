using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juice.Core.Domain;
using Juice.DataAccess.Domain;
using NHibernate;

namespace Juice.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public void Add(User user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public void Update(User user)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(User user)
        {
            throw new System.NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
