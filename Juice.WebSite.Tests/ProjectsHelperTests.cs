using System;
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
        [Fact]
        void Test_Get_Current_Project()
        {
            HttpCookieCollection cookies = new HttpCookieCollection
                                               {
                                                   new HttpCookie("CurrentProjectId", "2"),
                                                   new HttpCookie("CurrentProjectName", "ProjectName")
                                               };

            ProjectsHelper helper = new ProjectsHelper();

            int? id = helper.GetCurrentProjectId(cookies);

            Assert.NotNull(id);
            Assert.Equal(2, id);
        }

        [Fact]
        void Test_Get_Current_Project_Returns_Null_When_No_Cookie_Set()
        {
            HttpCookieCollection cookies = new HttpCookieCollection();

            ProjectsHelper helper = new ProjectsHelper();

            int? id = helper.GetCurrentProjectId(cookies);

            Assert.Null(id);
        }

        [Fact]
        void Test_Get_Current_Project_Throws_Exception_When_Parameter_Null()
        {
            ProjectsHelper helper = new ProjectsHelper();

            Assert.Throws<ArgumentNullException>(() => helper.GetCurrentProjectId(null));
        }

        [Fact]
        void Test_Set_Current_Project()
        {
            HttpCookieCollection cookies = new HttpCookieCollection();

            Project project = MockRepository.GenerateMock<Project>();
            project.Stub(x => x.Id).Return(2);
            project.Name = "ProjectName";

            ProjectsHelper helper = new ProjectsHelper();
            helper.SetCurrentProjectId(cookies, project);

            Assert.Contains("CurrentProjectId", cookies.AllKeys);
            Assert.Equal(project.Id.ToString(), cookies["CurrentProjectId"].Value);
            Assert.True(DateTime.Now.AddYears(2).Subtract(cookies["CurrentProjectId"].Expires).TotalSeconds < 30);
            Assert.Contains("CurrentProjectName", cookies.AllKeys);
            Assert.Equal(project.Name, cookies["CurrentProjectName"].Value);
            Assert.True(DateTime.Now.AddYears(2).Subtract(cookies["CurrentProjectName"].Expires).TotalSeconds < 30);
        }
    }
}
