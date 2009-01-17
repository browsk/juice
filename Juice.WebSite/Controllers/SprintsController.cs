using System;
using System.Linq;
using System.Collections.Generic;
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

        public SprintsController()
        {
            ProjectsHelper = new ProjectsHelper();
        }

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

        public ProjectsHelper ProjectsHelper
        {
            get; set;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            int? currentProjectId = ProjectsHelper.GetCurrentProjectId(Request.Cookies);

            if (currentProjectId == null)
            {
                ViewData["Message"] = "Select a project first";
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                Project currentProject = _projectRepository.Get(currentProjectId.Value);

                var sprints = currentProject.Sprints;

                if (sprints.Count == 0)
                    ViewData["Message"] = "No sprints defined for this project";

                return View(sprints);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
        {
            ICollection<Project> projects = _projectRepository.GetAll();

            int? currentProjectId = ProjectsHelper.GetCurrentProjectId(Request.Cookies);

            return View("Create", new SelectList(projects, "projectId", "Name", currentProjectId ?? projects.First().Id));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateNew(FormCollection formCollection)
        {
            var sprint = new Sprint
                             {
                                 Name = formCollection["name"],
                                 StartDate = DateTime.Parse(formCollection["startdate"]),
                                 EndDate = DateTime.Parse(formCollection["enddate"]),
                             };

            var project = _projectRepository.Get(int.Parse(formCollection["projectId"]));
            project.Sprints.Add(sprint);
            _projectRepository.Save(project);

            return RedirectToAction("Index");
        }
    }
}