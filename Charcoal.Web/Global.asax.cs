using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Charcoal.Common.Providers;
using Charcoal.PivotalTracker;
using StructureMap;

namespace Charcoal.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var tokenRetrieval = new Func<string>(() =>
            {
                if (HttpContext.Current != null)
                {
                    var token = HttpContext.Current.Session["token"];
                    if (token is string)
                    {
                        return token as string;
                    }
                }
                return null;
            });

            ObjectFactory.Initialize(context =>
            {
                context.For<IAccountProvider>().Use<PTAuthenticationProvider>();
                context.For<IProjectProvider>().Use(() => new PTProjectProvider(tokenRetrieval()));
                context.For<IStoryProvider>().Use(() => new PTStoryProvider(tokenRetrieval()));

                context.Scan(ias =>
                                {
                                    ias.TheCallingAssembly();
                                    ias.AddAllTypesOf<Controller>();
                                });
            });

            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
        }
    }
}