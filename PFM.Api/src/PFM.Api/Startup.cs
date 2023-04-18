﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PFM.Api.Middlewares;
using PFM.DataAccessLayer;
using PFM.Services.Core.Automapper;
using PFM.Services.ExternalServices.AuthApi;
using Refit;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PFM.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Log.Logger);

            services.AddMvc(o => {
                var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddDbContext<PFMContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("PFMConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<PFMContext>()
                .AddDefaultTokenProviders();

            services
                .AddRefitClient<IAuthApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration["AuthApi:EndpointUrl"]));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "PFM API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    In = "header",
                    Type = "apiKey",
                    Name = "Authorization",
                    Description = "JWT Authorization Token"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<Configuration.DefaultModule>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<TimedOperationMiddleware>();
            app.UseMiddleware<UnhandledExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PFM API v1");
            });

            app.UseMvc();

            //http://localhost:52688/swagger/index.html

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
                cfg.AddProfile<EntityToEntityMapping>();
                cfg.AddProfile<SearchParametersMapping>();
            });
        }
    }
}
