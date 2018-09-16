using AutoMapper;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PersonalFinanceManager.Services.Automapper;
using System.Web.Helpers;
using System.Security.Claims;

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
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToDTOMapping>();
                cfg.AddProfile<DTOToModelMapping>();
            });
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        protected virtual void Application_BeginRequest()
        {
          
        }

        protected virtual void Application_EndRequest()
        {
            
        }
    }
}
