using System;
using System.Collections.Generic;
using Juice.Core.Domain;
using Juice.Core.Repositories;
using Juice.DataAccess.Repositories;
using log4net;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Exceptions;
using NHibernate.Tool.hbm2ddl;
using Xunit;

namespace Juice.DataAccess.Tests
{
    public class ProjectRepositoryTests : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly Configuration _configuration;
        private readonly ProjectRepository _repository;

        private readonly Project[] _projects = new Project[]
                                                   {
                                                       new Project() {Name = "ProjectA", Description = "DescriptionA"},
                                                       new Project() {Name = "ProjectB", Description = "Description"},
                                                       new Project() {Name = "ProjectC", Description = "Description"}
                                                   };

        public ProjectRepositoryTests()
        {
            _configuration = new Configuration();
            _configuration.Configure();
            _configuration.AddAssembly(typeof(Project).Assembly);

            _sessionFactory = _configuration.BuildSessionFactory();

            _repository = new ProjectRepository();
            _repository.SessionFactory = _sessionFactory;
            CurrentSessionContext.Bind(_sessionFactory.OpenSession());

            new SchemaExport(_configuration).Execute(false, true, false, false);

            CreateTestData();

            // enable log4net
            log4net.Config.XmlConfigurator.Configure();
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
        void Can_add_new_project()
        {
            Project project = new Project
                                  {
                                      Name = "Test Project",
                                      Description = "Desc"
                                  };


            int projectId =_repository.Save(project);

            Assert.Equal(projectId, project.Id);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Project>(project.Id);
                
                Assert.NotSame(fromDb, project);
                Assert.Equal(project.Id, fromDb.Id);
                Assert.Equal(project.Name, fromDb.Name);
                Assert.Equal(project.Description, fromDb.Description);
            }
        }

        [Fact]
        void Can_read_project()
        {
            var project = _projects[0];

            var fromDb = _repository.Get(project.Id);


            Assert.NotSame(project, fromDb);
            Assert.Equal(project.Id, fromDb.Id);
            Assert.Equal(project.Description, fromDb.Description);
            Assert.Equal(project.Name, fromDb.Name);
        }

        [Fact]
        void Can_update_Project()
        {
            var project = _projects[1];

            project.Name = "New Name";
            _repository.Update(project);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = _repository.Get(project.Id);

                Assert.Equal(project.Name, fromDb.Name);
            }
        }

        [Fact]
        void Can_delete_existing_Project()
        {

            var project = _repository.Get(_projects[0].Id);

            using (ITransaction transaction = _sessionFactory.GetCurrentSession().BeginTransaction())
            {
                _repository.Delete(project);

                transaction.Commit();
            }

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Project>(project.Id);
                Assert.Null(fromDb);
            }
        }

        [Fact]
        void Expected_exception_occurs_when_adding_project_with_duplicate_name()
        {
            var project = new Project {Name = "ProjectA", Description = "something else"};

            Assert.Throws<GenericADOException>(() =>_repository.Save(project));
        }

        [Fact]
        void Can_read_all_Projects()
        {
            var projects = _repository.GetAll();

            Assert.Equal(_projects.Length, projects.Count);

            foreach (var project in _projects)
            {
                Assert.Contains(project, projects);
            }
        }

        [Fact]
        void Can_find_all_matching_projects()
        {
            var projects = _repository.FindAll(
                new Dictionary<string, object>
                    {
                        {"Description", "Description"},
                    }
                );

            Assert.NotNull(projects);
            Assert.Equal(2, projects.Count);
            Assert.Contains(_projects[1], projects);
            Assert.Contains(_projects[2], projects);
        }

        [Fact]
        void Can_find_single_matching_project()
        {
            var project = _repository.FindSingle(
                new Dictionary<string, object>
                    {
                        {"Name", "ProjectA"},
                    }
                );

            Assert.NotNull(project);
            Assert.Equal(_projects[0], project);
        }

        /// <summary>
        ///                     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            CurrentSessionContext.Unbind(_sessionFactory).Close();
            _sessionFactory.Close();
        }
    }
}
