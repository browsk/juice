using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Juice.Core.Domain;
using Juice.Core.Repositories;
using Juice.WebSite.Controllers;
using Xunit;
using Rhino.Mocks;

namespace Juice.WebSite.Tests
{
    public class ProjectsControllerTests
    {
        [Fact]
        void Test_Index_Action()
        {
            var repository = MockRepository.GenerateStub<IProjectRepository>();

            ProjectsController controller = new ProjectsController();
            controller.Repository = repository;

            repository.Stub(x => x.GetAll()).Return(
                new List<Project>
                    {
                        new Project {Name = "Test1", Description = "Desc1"},
                        new Project {Name = "Test2", Description = "Desc2"},
                        new Project {Name = "Test3", Description = "Desc3"},
                    });

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);

            var model = result.ViewData.Model as List<Project>;

            Assert.NotNull(model);
            Assert.Equal("Index", result.ViewName);
            Assert.Equal(3, model.Count);
        }

        [Fact]
        void Test_Create_Action()
        {
            ProjectsController controller = new ProjectsController();

            var result = controller.Create() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Create", result.ViewName);
        }

        [Fact]
        void Test_CreateNew_Action()
        {
            var controller = new ProjectsController();
            var repository = MockRepository.GenerateMock<IProjectRepository>();
            controller.Repository = repository;

            repository.Expect(x => x.Save(Arg<Project>.Matches
                                              (p => p.Name == "Test" && p.Description == "Desc"))).Return(1);

            FormCollection formCollection = new FormCollection
                                                {
                                                    {"name", "Test"},
                                                    {"description", "Desc"}
                                                };

            var result = controller.CreateNew(formCollection) as RedirectToRouteResult;

            repository.VerifyAllExpectations();

            Assert.NotNull(result);
            Assert.Equal("Index", result.Values["action"]);
        }

        [Fact]
        void Test_Edit_Action()
        {
            var repository = MockRepository.GenerateMock<IProjectRepository>();
            var controller = new ProjectsController();
            controller.Repository = repository;

            var project = new Project
                              {
                                  Name = "Test",
                                  Description = "Desc"
                              };

            repository.Expect(x => x.Get(Arg<int>.Is.Anything)).Return(project);

            var result = controller.Edit(1) as ViewResult;

            repository.VerifyAllExpectations();

            Assert.NotNull(result);
            Assert.Equal("Edit", result.ViewName);
            Assert.Same(project, result.ViewData.Model);
        }

        [Fact]
        void Test_Delete_Action()
        {
            var repository = MockRepository.GenerateMock<IProjectRepository>();
            var controller = new ProjectsController();
            controller.Repository = repository;

            int id = (new Random((int)DateTime.UtcNow.Ticks)).Next();

            var project = new Project {Name = "Test", Description = "Desc"};
            repository.Stub(x => x.Get(id)).Return(project);
            repository.Expect(x => x.Delete(Arg<Project>.Is.Same(project)));

            var result = controller.Delete(id) as RedirectToRouteResult;

            repository.VerifyAllExpectations();

            Assert.NotNull(result);
            Assert.Equal("Index", result.Values["action"]);
        }

        [Fact]
        void Test_Update_Action()
        {
            // set up a http context for the controller
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var request = MockRepository.GenerateStub<HttpRequestBase>();
            httpContext.Stub(x => x.Request).Return(request);
            request.RequestType = "POST";

            var repository = MockRepository.GenerateMock<IProjectRepository>();
            var controller = new ProjectsController {Repository = repository};
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            var project = MockRepository.GenerateMock<Project>();
            project.Stub(x => x.Id).Return(22);
            project.Stub(x => x.Name).PropertyBehavior();
            project.Stub(x => x.Description).PropertyBehavior();
            project.Name = "Name";
            project.Description = "Desc";

            var formCollection = new FormCollection
                                     {
                                         {"Name", "NewName"},
                                         {"Description", "NewDescription"}
                                     };

            repository.Stub(x => x.Get(Arg<int>.Is.Equal(22))).Return(project);
            repository.Expect(
                x =>
                x.Update(Arg<Project>.Matches(p => p.Id == project.Id && p.Name == formCollection["Name"] && p.Description == "NewDescription")));

            var result = controller.Update(22, formCollection) as RedirectToRouteResult;

            repository.VerifyAllExpectations();
            Assert.NotNull(result);
            Assert.Equal("Index", result.Values["action"]);
        }

        [Fact]
        void Test_Select_Action()
        {
            var repository = MockRepository.GenerateStub<IProjectRepository>();
            var controller = new ProjectsController { Repository = repository };

            var context = MockRepository.GenerateStub<HttpContextBase>();
            var response = MockRepository.GenerateStub<HttpResponseBase>();
            var cookies = new HttpCookieCollection();
            response.Stub(x => x.Cookies).Return(cookies);
            context.Stub(x => x.Response).Return(response);

            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            var project = MockRepository.GenerateMock<Project>();
            project.Stub(x => x.Id).Return(22);
            project.Stub(x => x.Name).PropertyBehavior();
            project.Stub(x => x.Description).PropertyBehavior();

            project.Name = "Name";

            repository.Stub(x => x.Get(project.Id)).Return(project);

            var result = controller.Select(project.Id) as RedirectToRouteResult;

            Assert.Contains("CurrentProjectId", cookies.AllKeys);
            Assert.Equal(project.Id.ToString(), cookies["CurrentProjectId"].Value);
            Assert.True(DateTime.Now.AddDays(30).Subtract(cookies["CurrentProjectId"].Expires).TotalSeconds < 30);
            Assert.Contains("CurrentProjectName", cookies.AllKeys);
            Assert.Equal(project.Name, cookies["CurrentProjectName"].Value);
            Assert.True(DateTime.Now.AddDays(30).Subtract(cookies["CurrentProjectName"].Expires).TotalSeconds < 30);
        }
    }
}