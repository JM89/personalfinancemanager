using AutoMapper;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PersonalFinanceManager.Services.Automapper;
using System.Web.Helpers;
using System.Security.Claims;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;

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

            if (System.Diagnostics.Debugger.IsAttached)
            {
                ServicePointManager.ServerCertificateValidationCallback =
                   delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                   {
                       return true;
                   };
            }
        }

        protected virtual void Application_BeginRequest()
        {
          
        }

        protected virtual void Application_EndRequest()
        {
            
        }
    }
}
