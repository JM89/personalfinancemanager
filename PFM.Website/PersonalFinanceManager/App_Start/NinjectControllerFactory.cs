using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PersonalFinanceManager.App_Start
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _ninjectKernel;
        public NinjectControllerFactory(IKernel kernel)
        {
            _ninjectKernel = kernel;
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (controllerType == null) ? null : (IController)_ninjectKernel.Get(controllerType);
        }
    }
}