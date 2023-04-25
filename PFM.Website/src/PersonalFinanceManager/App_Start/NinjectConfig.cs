using Ninject.Extensions.Conventions;
using Ninject.Modules;
using PersonalFinanceManager.Services.Core;
using Serilog;

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
            Log.Logger = new LoggerConfiguration()
              .ReadFrom.AppSettings()
              .CreateLogger();

            Kernel.Bind<ILogger>().ToConstant(Log.Logger);

            //Default interface in this scenario means that if your interface is named IXService, then the class named XService will be binded to it. 
            Kernel.Bind(x => x.FromAssemblyContaining(typeof(IBaseService))
                            .SelectAllClasses()
                            .BindDefaultInterface());
        }
    }
}