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
        public virtual int? GetCurrentProjectId(HttpCookieCollection cookies)
        {
            if (cookies == null)
            {
                throw new ArgumentNullException("cookies");
            }

            if (cookies.AllKeys.Contains("CurrentProjectId"))
            {
                string idString = cookies["CurrentProjectId"].Value;

                int projectId;
                if (int.TryParse(idString, out projectId))
                {
                    return projectId;
                }
            }
            return null;
        }

        public virtual void SetCurrentProjectId(HttpCookieCollection cookies, Project project)
        {
            if (cookies["CurrentProjectId"] == null)
                cookies.Add(new HttpCookie("CurrentProjectId"));

            if (cookies["CurrentProjectName"] == null)
                cookies.Add(new HttpCookie("CurrentProjectName"));

            cookies["CurrentProjectId"].Value = project.Id.ToString();
            cookies["CurrentProjectName"].Value = project.Name;
            cookies["CurrentProjectId"].Expires = DateTime.Now.AddYears(2);
            cookies["CurrentProjectName"].Expires = DateTime.Now.AddYears(2);                    
        }
    }
}