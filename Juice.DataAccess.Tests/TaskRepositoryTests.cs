using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juice.Core.Domain;
using Juice.Core.Repositories;
using Juice.DataAccess.Repositories;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using Xunit;

namespace Juice.DataAccess.Tests
{
    public class TaskRepositoryTests
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly Configuration _configuration;
        private readonly ProjectRepository _projectRepository;
        private readonly TaskRepository _taskRepository;

        private readonly Project[] _projects = new Project[]
                                                   {
                                                       new Project() {Name = "ProjectA", Description = "DescriptionA"},
                                                       new Project() {Name = "ProjectB", Description = "DescriptionB"},
                                                   };

        private readonly Task[] _tasks = new Task[]
                                             {
                                                 new Task() {Name = "Task1", Description = "Desc"}
                                             };
        public TaskRepositoryTests()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Project).Assembly);

            _sessionFactory = _configuration.BuildSessionFactory();

            _projectRepository = new ProjectRepository {SessionFactory = _sessionFactory};
            _taskRepository = new TaskRepository {SessionFactory = _sessionFactory};

            CurrentSessionContext.Bind(_sessionFactory.OpenSession());

            new SchemaExport(_configuration).Execute(false, true, false, false);

            CreateTestData();
        }

        private void CreateTestData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transation = session.BeginTransaction())
                {
                    foreach (var project in _projects)
                    {
                        session.Save(project);
                    }
                    transation.Commit();
                }
            }
        }

        [Fact]
        void Test_Get_Tasks_For_Project()
        {
            
        }
    }
}
