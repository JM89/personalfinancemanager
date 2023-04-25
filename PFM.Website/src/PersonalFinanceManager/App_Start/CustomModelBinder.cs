using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace PersonalFinanceManager.App_Start
{
    public class CustomModelBinder : System.Web.Mvc.DefaultModelBinder
    {
        protected override void OnModelUpdated(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            controllerContext.Controller.ViewData.Model = bindingContext.Model;
            base.OnModelUpdated(controllerContext, bindingContext);
        }
    }
}