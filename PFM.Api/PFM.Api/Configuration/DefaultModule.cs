using Autofac;
using PFM.DataAccessLayer.Repositories.Implementations;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services;
using PFM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PFM.Api.Configuration
{
    public class DefaultModule: Autofac.Module
    {
        private ContainerBuilder _builder;

        protected override void Load(ContainerBuilder builder)
        {
            _builder = builder;
            RegisterRepositories();
            RegisterServices();
        }

        private void RegisterRepositories()
        {
            _builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IBaseRepository<>))).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces(); 
        }

        private void RegisterServices()
        {
            _builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IBaseService))).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
        }
    }
}
