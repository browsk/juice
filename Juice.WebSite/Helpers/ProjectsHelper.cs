using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Juice.Core.Domain;
using Juice.Core.Repositories;

namespace Juice.WebSite.Helpers
{
    public class ProjectsHelper
    {
        private Controller _controller;
        private IProjectRepository _repository;

        public ProjectsHelper(Controller controller, IProjectRepository repository)
        {
            _controller = controller;
            _repository = repository;
        }

        public virtual Project CurrentProject
        {
            get
            {
                Project project = null;

                if (_controller.Request != null && _controller.Request.Cookies.AllKeys.Contains("CurrentProjectId"))
                {
                    string idString = _controller.Request.Cookies["CurrentProjectId"].Value;

                    int projectId;
                    if (int.TryParse(idString, out projectId))
                    {
                        try
                        {
                            project = _repository.Get(projectId);
                        }
                        catch (Exception e)
                        {
                            // not too interested about errors
                            Debug.Assert(false, e.Message);
                        }
                    }
                }

                Debug.Assert(project != null);
                
                return project;
            }

            set
            {
                Debug.Assert(_controller.Response != null);

                if (_controller.Response != null)
                {
                    HttpCookieCollection cookies = _controller.Response.Cookies;

                    if (cookies["CurrentProjectId"] == null)
                        cookies.Add(new HttpCookie("CurrentProjectId"));

                    if (cookies["CurrentProjectName"] == null)
                        cookies.Add(new HttpCookie("CurrentProjectName"));

                    cookies["CurrentProjectId"].Value = value.Id.ToString();
                    cookies["CurrentProjectName"].Value = value.Name;
                    cookies["CurrentProjectId"].Expires = DateTime.Now.AddYears(2);
                    cookies["CurrentProjectName"].Expires = DateTime.Now.AddYears(2);                    
                }
            }
        }
    }
}