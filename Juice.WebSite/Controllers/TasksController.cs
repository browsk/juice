using System.Linq;
using System.Web.Mvc;
using Juice.Core.Domain;
using Juice.Core.Repositories;

namespace Juice.WebSite.Controllers
{
    public class TasksController : Controller
    {
        private ITaskRepository _taskRepository;
        private IProjectRepository _projectRepository;

        private Project _currentProject;

        public ITaskRepository TaskRepository
        {
            set
            {
                _taskRepository = value;
            }
        }

        public IProjectRepository ProjectRepository
        {
            set
            {
                _projectRepository = value;
            }
        }

        public TasksController()
        {
            if (Request.Cookies.AllKeys.Contains("CurrentProjectId"))
            {
                int projectId = int.Parse(Request.Cookies["CurrentProjectId"].Value);

                _currentProject = _projectRepository.Get(projectId);
            }
        }

        public ActionResult Index()
        {
            return null;
        }
    }
}