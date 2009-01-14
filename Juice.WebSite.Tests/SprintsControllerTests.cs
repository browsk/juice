using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
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

        private void GenerateStubsAndControllerForIndexTests()
        {
            var project = MockRepository.GenerateMock<Project>();
            project.Stub(x => x.Id).Return(2001);

            GenerateStubsAndControllerForIndexTests(project);
        }
        
        private void GenerateStubsAndControllerForIndexTests(Project currentProject)
        {
            ProjectsHelper helper = MockRepository.GenerateMock<ProjectsHelper>(null, null);

            _project = currentProject;

            helper.Stub(x => x.CurrentProject).Return(_project);

            _controller = new SprintsController { ProjectsHelper = helper };
        }

        [Fact]
        void Test_Index_Action_When_Sprints_Exist()
        {
            GenerateStubsAndControllerForIndexTests();

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
            GenerateStubsAndControllerForIndexTests();

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
            GenerateStubsAndControllerForIndexTests(null);

            var result = _controller.Index() as RedirectToRouteResult;

            Assert.NotNull(result);

            Assert.Equal("Index", result.Values["action"]);
            Assert.Equal("Projects", result.Values["controller"]);
        }

    }
}
