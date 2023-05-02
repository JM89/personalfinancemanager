using Autofac;
using DataAccessLayer.Repositories.Interfaces;
using Services.Interfaces;
using System.Reflection;

namespace Api.Extensions
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
            if (_builder == null)
                throw new Exception("DI Exception: Load method should be called first");

            var assembly = Assembly.GetAssembly(typeof(IBankAccountRepository)) ?? Assembly.GetExecutingAssembly();
            _builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces(); 
        }

        private void RegisterServices()
        {
            if (_builder == null)
                throw new Exception("DI Exception: Load method should be called first");

            var assembly = Assembly.GetAssembly(typeof(IBankAccountService)) ?? Assembly.GetExecutingAssembly();
            _builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
        }
    }
}
