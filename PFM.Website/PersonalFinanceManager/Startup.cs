using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using PersonalFinanceManager.App_Start;
using Serilog;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(PersonalFinanceManager.Startup))]
namespace PersonalFinanceManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {      
            ConfigureNinject(app);

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login")
            });

            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();
        }
    }
}
