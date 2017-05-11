﻿using AutoMapper;
using PersonalFinanceManager.DataAccess;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PersonalFinanceManager.Core.Automapper;

namespace PersonalFinanceManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
                cfg.AddProfile<EntityToEntityMapping>();
                cfg.AddProfile<SearchParametersMapping>();
            });
        }

        protected virtual void Application_BeginRequest()
        {
            var ctx = HttpContext.Current;
            ctx.Items["_DbContext"] = new ApplicationDbContext();
        }

        protected virtual void Application_EndRequest()
        {
            var ctx = HttpContext.Current;
            var dbCtx = ctx.Items["_DbContext"] as ApplicationDbContext;
            if (dbCtx != null)
            {
                var currentTransaction = dbCtx.Database.CurrentTransaction;
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                }
                dbCtx.Dispose();
            }
        }
    }
}
