using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace HobbyClue.Web.Configuration
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer container;

        public StructureMapControllerFactory(IContainer container)
        {
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (controllerType == null) ? base.GetControllerInstance(requestContext, null) 
                                            : container.GetInstance(controllerType) as IController;
        }
    }
}
