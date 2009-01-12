using System;
using Juice.Core.Domain;
using System.Web.Mvc;
using Juice.Core.Repositories;

namespace Juice.WebSite.Controllers
{
    public class SprintsController : Controller
    {
        private ISprintRepository _sprintRepository;
        private IProjectRepository _projectRepository;

        private Project _currentProject;

        public ISprintRepository SprintRepository
        {
            set
            {
                _sprintRepository = value;
            }
        }

        public IProjectRepository ProjectRepository
        {
            set
            {
                _projectRepository = value;
            }
        }

        public ActionResult Index()
        {
            var sprints = _sprintRepository.GetAll();

            if (sprints.Count == 0)
                ViewData["Message"] = "No sprints defined for this project";

            return View(sprints);
        }

        public ActionResult Create()
        {
            var projects = _projectRepository.GetAll();

            int currentProjectId = int.Parse(Request.Cookies["CurrentProjectId"].Value);

            return View("Create", new SelectList(projects, "Id", "Name", currentProjectId));
        }

        public ActionResult CreateNew(FormCollection formCollection)
        {
            var sprint = new Sprint
                             {
                                 Name = formCollection["name"],
                                 StartDate = DateTime.Parse(formCollection["startdate"]),
                                 EndDate = DateTime.Parse(formCollection["enddate"]),
                                 Project = _projectRepository.Get(int.Parse(formCollection["Id"]))
                             };

            _sprintRepository.Save(sprint);

            return RedirectToAction("Index");
        }
    }
}