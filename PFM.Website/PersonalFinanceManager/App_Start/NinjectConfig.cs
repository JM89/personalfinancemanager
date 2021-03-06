﻿using Ninject.Modules;
using PersonalFinanceManager.Services.Core;
using Ninject.Extensions.Conventions;
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
            //Default interface in this scenario means that if your interface is named IXService, then the class named XService will be binded to it. 
            Kernel.Bind(x => x.FromAssemblyContaining(typeof(IBaseService))
                            .SelectAllClasses()
                            .BindDefaultInterface());
        }
    }
}