using Ninject.Modules;
using PersonalFinanceManager.Services.Core;
using System.Linq;
using Ninject.Extensions.Conventions;
using PersonalFinanceManager.Services.Interfaces;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.DataAccess;
using Ninject.Web.Common;

namespace PersonalFinanceManager.App_Start
{
    public class NinjectConfig : NinjectModule
    {
        public object Classes { get; private set; }

        public override void Load()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            //PerRequet, we inject an instance of DbContext.
            Kernel.Bind<ApplicationDbContext>().ToMethod(ctx => { return (ApplicationDbContext)System.Web.HttpContext.Current.Items["_DbContext"]; }).InRequestScope();

            //Default interface in this scenario means that if your interface is named IXService, then the class named XService will be binded to it. 
            Kernel.Bind(x => x.FromAssemblyContaining(typeof(IBaseService))
                            .SelectAllClasses()
                            .BindDefaultInterface());
        }
    }
}