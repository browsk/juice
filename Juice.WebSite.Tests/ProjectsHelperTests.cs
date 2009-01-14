using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Juice.Core.Domain;
using Juice.Core.Repositories;
using Juice.WebSite.Helpers;
using Rhino.Mocks;
using Xunit;

namespace Juice.WebSite.Tests
{
    public class ProjectsHelperTests
    {
        // require a concrete controller class as the mocked version was not
        // working
        class TestController : Controller
        {
        }

        [Fact]
        void Test_Get_Current_Project()
        {
            HttpContextBase context = MockRepository.GenerateStub<HttpContextBase>();
            HttpRequestBase request = MockRepository.GenerateStub<HttpRequestBase>();

            HttpCookieCollection cookies = new HttpCookieCollection();

            cookies.Add(new HttpCookie("CurrentProjectId", "2"));
            cookies.Add(new HttpCookie("CurrentProjectName", "ProjectName"));

            request.Stub(x => x.Cookies).Return(cookies);
            context.Stub(x => x.Request).Return(request);

            TestController controller = new TestController();
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            Project project = new Project();

            IProjectRepository repository = MockRepository.GenerateMock<IProjectRepository>();
            repository.Expect(x => x.Get(Arg<int>.Is.Equal(2))).Return(project);

            ProjectsHelper helper = new ProjectsHelper(controller, repository);

            Project result = helper.CurrentProject;

            repository.VerifyAllExpectations();
            Assert.Same(project, result);
        }

        [Fact]
        void Test_Set_Current_Project()
        {
            HttpContextBase context = MockRepository.GenerateStub<HttpContextBase>();
            HttpResponseBase response = MockRepository.GenerateStub<HttpResponseBase>();
            
            HttpCookieCollection cookies = new HttpCookieCollection();
            response.Stub(x => x.Cookies).Return(cookies);

            response.Stub(x => x.Cookies).Return(cookies);
            context.Stub(x => x.Response).Return(response);

            TestController controller = new TestController();
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            IProjectRepository repository = MockRepository.GenerateStub<IProjectRepository>();

            ProjectsHelper helper = new ProjectsHelper(controller, repository);

            Project project = MockRepository.GenerateMock<Project>();
            project.Stub(x => x.Id).Return(2);
            project.Name = "ProjectName";

            helper.CurrentProject = project;

            Assert.Contains("CurrentProjectId", cookies.AllKeys);
            Assert.Equal(project.Id.ToString(), cookies["CurrentProjectId"].Value);
            Assert.True(DateTime.Now.AddYears(2).Subtract(cookies["CurrentProjectId"].Expires).TotalSeconds < 30);
            Assert.Contains("CurrentProjectName", cookies.AllKeys);
            Assert.Equal(project.Name, cookies["CurrentProjectName"].Value);
            Assert.True(DateTime.Now.AddYears(2).Subtract(cookies["CurrentProjectName"].Expires).TotalSeconds < 30);
        }
    }
}
