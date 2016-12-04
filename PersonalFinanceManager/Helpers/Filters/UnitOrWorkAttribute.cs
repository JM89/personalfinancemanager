using PersonalFinanceManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalFinanceManager.Helpers.Filters
{
    public class UnitOrWorkAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var requestMethod = filterContext.HttpContext.Request.HttpMethod;
            if (requestMethod == "POST" || requestMethod == "PUT" || requestMethod == "DELETE")
            {
                var dbCtx = (ApplicationDbContext)HttpContext.Current.Items["_DbContext"];
                var dbCxtTransaction = dbCtx.Database.BeginTransaction();
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var dbCtx = (ApplicationDbContext)HttpContext.Current.Items["_DbContext"];
            var dbCxtTransaction = dbCtx.Database.CurrentTransaction;

            if (dbCxtTransaction != null)
            {
                if (filterContext.Exception == null)
                {
                    dbCxtTransaction.Commit();
                }
                else
                {
                    dbCxtTransaction.Rollback();
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}