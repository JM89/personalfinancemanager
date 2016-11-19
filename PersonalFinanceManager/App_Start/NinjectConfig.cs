using Ninject.Modules;
using PersonalFinanceManager.Services.Core;
using System.Linq;
using Ninject.Extensions.Conventions;
using PersonalFinanceManager.Services.Interfaces;

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
            Kernel.Bind(x => x.FromAssemblyContaining(typeof(IBaseService)).SelectAllClasses().BindDefaultInterface());
        }
    }
}