using AutoMapper;
using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Utils.Automapper;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PersonalFinanceManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(cfg => {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
            });
        }

        protected virtual void Application_BeginRequest()
        {
            HttpContext.Current.Items["_DbContext"] = new ApplicationDbContext();
        }

        protected virtual void Application_EndRequest()
        {
            var entityContext = HttpContext.Current.Items["_DbContext"] as ApplicationDbContext;
            if (entityContext != null)
                entityContext.Dispose();
        }
    }
}
