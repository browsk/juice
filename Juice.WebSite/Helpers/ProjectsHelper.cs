using System;
using System.Web.Mvc;
using System.Linq;
using Juice.Core.Domain;
using Juice.Core.Repositories;

namespace Juice.WebSite.Helpers
{
    class ProjectsHelper
    {
        private Controller _controller;
        private IProjectRepository _repository;

        public ProjectsHelper(Controller controller, IProjectRepository repository)
        {
            _controller = controller;
            _repository = repository;
        }

        public Project CurrentProject
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
                        catch (Exception)
                        {
                            // not too interested about errors
                        }
                    }
                }
                return project;
            }
        }
    }
}