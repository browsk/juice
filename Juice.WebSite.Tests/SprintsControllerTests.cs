using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Juice.Core.Domain;
using Juice.Core.Repositories;
using Juice.WebSite.Controllers;
using Juice.WebSite.Helpers;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Xunit;

namespace Juice.WebSite.Tests
{
    public class SprintsControllerTests
    {
        private SprintsController _controller;
        private Project _project;

        private void GenerateStubsAndController()
        {
            var project = MockRepository.GenerateMock<Project>();
            project.Stub(x => x.Id).Return(2001);

            GenerateStubsAndController(project);
        }
        
        private void GenerateStubsAndController(Project currentProject)
        {
            _project = currentProject;

            _controller = new SprintsController();

            _controller.ProjectsHelper = MockRepository.GenerateStub<ProjectsHelper>();
            if (_project != null)
            {
                _controller.ProjectsHelper.Stub(x => x.GetCurrentProjectId(Arg<HttpCookieCollection>.Is.Anything)).Return(_project.Id);
            }

            HttpRequestBase request = MockRepository.GenerateStub<HttpRequestBase>();
            HttpContextBase context = MockRepository.GenerateStub<HttpContextBase>();
            context.Stub(x => x.Request).Return(request);

            _controller.ControllerContext = new ControllerContext(context, new RouteData(), _controller);

            IProjectRepository repository = MockRepository.GenerateStub<IProjectRepository>();

            if (_project != null)
            {
                repository.Stub(x => x.Get(Arg<int>.Is.Equal(_project.Id))).Return(_project);
            }

            _controller.ProjectRepository = repository;
        }

        [Fact]
        void Test_Index_Action_When_Sprints_Exist()
        {
            GenerateStubsAndController();

            _project.Stub(x => x.Sprints).Return(new List<Sprint>
                                                    {
                                                        {new Sprint {}}
                                                    });

            var result = _controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData.Model);
            Assert.IsType<List<Sprint>>(result.ViewData.Model);

            var sprints = result.ViewData.Model as List<Sprint>;
            Assert.Same(_project.Sprints, sprints);

            // should be no message
            Assert.False(result.ViewData.ContainsKey("Message"));
        }

        [Fact]
        void Test_Index_Action_With_No_Sprints()
        {
            GenerateStubsAndController();

            _project.Stub(x => x.Sprints).Return(new List<Sprint>());

            var result = _controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData.Model);
            Assert.IsType<List<Sprint>>(result.ViewData.Model);

            var sprints = result.ViewData.Model as List<Sprint>;
            Assert.Same(_project.Sprints, sprints);

            Assert.True(result.ViewData.ContainsKey("Message"));
            Assert.Equal("No sprints defined for this project", result.ViewData["Message"]);
        }

        [Fact]
        void Test_Index_Action_With_No_Current_Project()
        {
            GenerateStubsAndController(null);

            var result = _controller.Index() as RedirectToRouteResult;

            Assert.NotNull(result);

            Assert.Equal("Index", result.Values["action"]);
            Assert.Equal("Projects", result.Values["controller"]);
        }

        [Fact]
        void Test_Create_Action()
        {
            GenerateStubsAndController();

            var projects = new List<Project> {{new Project()}, {new Project()}};

            IProjectRepository repository = MockRepository.GenerateStub<IProjectRepository>();
            repository.Stub(x => x.GetAll()).Return(projects);

            _controller.ProjectRepository = repository;

            var result = _controller.Create() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Create", result.ViewName);
            Assert.NotNull(result.ViewData.Model);
            Assert.IsType<SelectList>(result.ViewData.Model);

            SelectList list = result.ViewData.Model as SelectList;
            // current project should be selected
            Assert.Equal(2001, list.SelectedValue);
            Assert.Equal("Name", list.DataTextField);
            Assert.Equal("projectId", list.DataValueField);

            IQueryable<object> items = list.Items.AsQueryable().Cast<object>();
            Assert.Equal(2, items.Count());
        }

        [Fact]
        void Test_CreateNew_Action()
        {
            FormCollection formCollection = new FormCollection
                                                {
                                                    {"name", "Name"},
                                                    {"startdate", "11-01-2009"},
                                                    {"enddate", "11-02-2009"},
                                                    {"projectId", "2"}
                                                };


            Project project = new Project
                                  {
                                      Description = "My Project",
                                      Name = "My Project",
                                      Sprints = new List<Sprint>()
                                  };

            IProjectRepository repository = MockRepository.GenerateMock<IProjectRepository>();
            repository.Expect(x => x.Get(Arg<int>.Is.Equal(2))).Return(project);

            repository.Expect(
                x =>
                x.Save(
                    Arg<Project>.Matches(
                        p =>
                        p.Sprints.First().Name == "Name" && 
                        p.Sprints.First().StartDate == new DateTime(2009, 1, 11) &&
                        p.Sprints.First().EndDate == new DateTime(2009, 2, 11)))).Return(1);

            SprintsController controller = new SprintsController {ProjectRepository = repository};

            RedirectToRouteResult result = controller.CreateNew(formCollection) as RedirectToRouteResult;

            repository.VerifyAllExpectations();

            Assert.NotNull(result);
            Assert.Equal("Index", result.Values["action"]);
        }
    }
}
