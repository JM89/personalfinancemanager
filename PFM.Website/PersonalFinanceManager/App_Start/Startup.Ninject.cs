using Ninject;
using Ninject.Web.Common.OwinHost;
using Owin;
using PersonalFinanceManager.App_Start;
using PersonalFinanceManager.Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PersonalFinanceManager
{
    public partial class Startup
    {
        public void ConfigureNinject(IAppBuilder app)
        {
            var kernel = CreateKernel();
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(kernel));
            app.UseNinjectMiddleware(() => kernel);
        }

        public IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}