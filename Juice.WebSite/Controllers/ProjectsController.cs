using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Juice.Core.Domain;
using Juice.Core.Repositories;

namespace Juice.WebSite.Controllers
{
    public class ProjectsController : Controller
    {
        private IProjectRepository _projectRepository;

        /// <summary>
        /// Sets the repository used to access the project information.
        /// </summary>
        /// <value>The repository.</value>
        public IProjectRepository Repository
        {
            set
            {
                _projectRepository = value;
            }
        }

        /// <summary>
        /// Fetch the list of projects
        /// </summary>
        /// <returns>View containing a list of projects</returns>
        public ActionResult Index()
        {
            var projects = _projectRepository.GetAll();
            return View("Index", projects.ToList());
        }

        /// <summary>
        /// Used when a user chooses to create a project
        /// </summary>
        /// <returns>The Create view</returns>
        public ActionResult Create()
        {
            return View("Create");
        }

        /// <summary>
        /// Creates the new project.
        /// </summary>
        /// <param name="formCollection">The form collection containing the data.</param>
        /// <returns>Redirect to <see cref="Index"/></returns>
        public ActionResult CreateNew(FormCollection formCollection)
        {
            var project = new Project
                              {
                                  Name = formCollection["name"],
                                  Description = formCollection["description"]
                              };

            _projectRepository.Save(project);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Controller action for editting a project
        /// </summary>
        /// <param name="id">The project id.</param>
        /// <returns>A view for editing the project details</returns>
        public ActionResult Edit(int id)
        {
            return View("Edit", _projectRepository.Get(id));
        }

        /// <summary>
        /// Action to delete the specified project
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Redirect to <see cref="Index"/></returns>
        public ActionResult Delete(int id)
        {
            _projectRepository.Delete(_projectRepository.Get(id));
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Controller action corresponding to a user selecting a project
        /// </summary>
        /// <param name="id">The project id.</param>
        /// <returns>A redirect to <see cref="HomeController.Index"/></returns>
        public ActionResult Select(int id)
        {
            if (Response.Cookies["CurrentProjectId"] == null)
                Response.Cookies.Add(new HttpCookie("CurrentProjectId"));

            if (Response.Cookies["CurrentProjectName"] == null)
                Response.Cookies.Add(new HttpCookie("CurrentProjectName"));

            Response.Cookies["CurrentProjectId"].Value = id.ToString();
            Response.Cookies["CurrentProjectName"].Value = _projectRepository.Get(id).Name;
            Response.Cookies["CurrentProjectId"].Expires = DateTime.Now.AddDays(30);
            Response.Cookies["CurrentProjectName"].Expires = DateTime.Now.AddDays(30);

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Updates the specified project using the information in the 
        /// <see cref="formCollection"/>
        /// </summary>
        /// <param name="id">The project id.</param>
        /// <param name="formCollection">The form collection.</param>
        /// <returns>Redirect to <see cref="Index"/></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int id, FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                var project = _projectRepository.Get(id);

                UpdateModel(project, new [] {"Name", "Description"}, formCollection);

                _projectRepository.Update(project);

            }
            return RedirectToAction("Index");
        }
    }
}