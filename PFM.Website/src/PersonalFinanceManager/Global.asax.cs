using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceManager.Services.Automapper;
using PersonalFinanceManager.Services.HttpClientWrapper;
using System.Net;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
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

            var services = new ServiceCollection();
            services.AddHttpClient<IHttpClientExtended, HttpClientExtended>();

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
