using System;
using Juice.Core.Domain;
using System.Web.Mvc;
using Juice.Core.Repositories;
using Juice.WebSite.Helpers;

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

                ProjectsHelper = new ProjectsHelper(this, _projectRepository);
            }
        }

        public ProjectsHelper ProjectsHelper
        {
            get; set;
        }

        public ActionResult Index()
        {
            var currentProject = ProjectsHelper.CurrentProject;

            if (currentProject == null)
            {
                ViewData["Message"] = "Select a project first";
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                var sprints = currentProject.Sprints;

                if (sprints.Count == 0)
                    ViewData["Message"] = "No sprints defined for this project";

                return View(sprints);
            }
        }

        public ActionResult Create()
        {
            var projects = _projectRepository.GetAll();

            int currentProjectId = ProjectsHelper.CurrentProject.Id;

            return View("Create", new SelectList(projects, "Id", "Name", currentProjectId));
        }

        public ActionResult CreateNew(FormCollection formCollection)
        {
            var sprint = new Sprint
                             {
                                 Name = formCollection["name"],
                                 StartDate = DateTime.Parse(formCollection["startdate"]),
                                 EndDate = DateTime.Parse(formCollection["enddate"]),
                             };

            var project = _projectRepository.Get(int.Parse(formCollection["Id"]));
            project.Sprints.Add(sprint);
            _projectRepository.Save(project);

            return RedirectToAction("Index");
        }
    }
}