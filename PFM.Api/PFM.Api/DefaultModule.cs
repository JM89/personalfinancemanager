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

namespace PFM.Api
{
    public class DefaultModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CurrencyRepository>().As<ICurrencyRepository>();
            builder.RegisterType<SalaryRepository>().As<ISalaryRepository>();
            builder.RegisterType<PensionRepository>().As<IPensionRepository>();
            builder.RegisterType<TaxRepository>().As<ITaxRepository>();
            builder.RegisterType<BankAccountRepository>().As<IBankAccountRepository>();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>();
        }
    }
}
