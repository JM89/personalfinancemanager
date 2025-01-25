using Autofac;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services;
using System.Reflection;

namespace PFM.Api.Extensions
{
    public class AutoFacModule: Autofac.Module
    {
        private ContainerBuilder? _builder;

        protected override void Load(ContainerBuilder builder)
        {
            _builder = builder;
            RegisterRepositories();
            RegisterServices();
        }

        private void RegisterRepositories()
        {
            var assembly = Assembly.GetAssembly(typeof(IBaseRepository<>));
            ArgumentNullException.ThrowIfNull(assembly);
            _builder?.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces(); 
        }

        private void RegisterServices()
        {
            var assembly = Assembly.GetAssembly(typeof(IBaseService));
            ArgumentNullException.ThrowIfNull(assembly);
            _builder?.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
        }
    }
}
