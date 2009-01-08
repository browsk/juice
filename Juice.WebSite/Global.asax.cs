using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.ControllerFactories;
using MvcContrib.Services;
using MvcContrib.Spring;
using NHaml.Web.Mvc;
using Spring.Context.Support;

namespace Juice.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
                );

        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        public override void  Init()
        {
            base.Init();
            ViewEngines.Engines.Add(new NHamlMvcViewEngine());
            ConfigureIOC();
        }

        private void ConfigureIOC()
        {
            WebApplicationContext webApplicationContext = ContextRegistry.GetContext() as WebApplicationContext;

            DependencyResolver.InitializeWith(new SpringDependencyResolver(webApplicationContext.ObjectFactory));

            ControllerBuilder.Current.SetControllerFactory(typeof (IoCControllerFactory));

        }
    }
}