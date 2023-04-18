using Autofac;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Interfaces;
using System.Reflection;

namespace PFM.Api.Extensions
{
    public class AutoFacModule: Autofac.Module
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
