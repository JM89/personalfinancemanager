using Api.Extensions;
using Api.Middlewares;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using PFM.Services.Core.Automapper;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables(prefix: "APP_");

            builder.Services.AddControllers();

            builder.Services
                .AddAuthenticationAndAuthorization(builder.Configuration)
                .AddMonitoring(builder.Configuration, builder.Environment.EnvironmentName)
                .AddEndpointsApiExplorer()
                .AddSwaggerDefinition()
                .AddEventPublisherConfigurations(builder.Configuration);

            builder.Services.AddDbContext<PFMContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("PFMConnection")));

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutoFacModule()));

            var app = builder.Build();

            app.UseMiddleware<TimedOperationMiddleware>();
            app.UseMiddleware<ResponseWrapperMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
                cfg.AddProfile<EntityToContractMapping>();
            });

            app.Run();
        }
    }
}