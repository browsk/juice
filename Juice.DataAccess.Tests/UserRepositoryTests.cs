using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juice.Core.Domain;
using Juice.DataAccess.Domain;
using Juice.DataAccess.Repositories;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Xunit;

namespace Juice.DataAccess.Tests
{
    public class UserRepositoryTests
    {
        private ISessionFactory _sessionFactory;
        private Configuration _configuration;

        public UserRepositoryTests()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof (User).Assembly);

            _sessionFactory = _configuration.BuildSessionFactory();

            new SchemaExport(_configuration).Execute(false, true, false, false);
            
        }

        [Fact]
        void Can_add_new_User()
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var user = new User
                    {
                        ApplicationName = "Test",
                        Email = "test@test.com",
                        Password = "blah",
                        Username = "Tim"

                    };

                    IUserRepository repository = new UserRepository();

                    repository.Add(user);

                    var userFromDb = session.Get<User>(user.Id);

                    Assert.NotNull(userFromDb);
                    Assert.NotSame(userFromDb, user);
                    Assert.Equal(user.Id, userFromDb.Id);
                    Assert.Equal(user.ApplicationName, userFromDb.ApplicationName);
                    Assert.Equal(user.Email, userFromDb.Email);
                    Assert.Equal(user.Password, userFromDb.Password);
                    Assert.Equal(user.Username, userFromDb.Username);

                    transaction.Rollback();
                }
            }
        }
    }
}
