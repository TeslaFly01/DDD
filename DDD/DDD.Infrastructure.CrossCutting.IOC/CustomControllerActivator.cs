using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DDD.Infrastructure.CrossCutting.IOC
{
    public class CustomControllerActivator : IControllerActivator
    {
        IController IControllerActivator.Create(
            System.Web.Routing.RequestContext requestContext,
            Type controllerType)
        {
            return DependencyResolver.Current
                .GetService(controllerType) as IController;
        }
    }
}
