using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Vortex.Controllers;
using Vortex.Infrastructure.Windsor;

namespace Vortex
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var container = new WindsorContainer();
            container.Register(FindControllers());
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(container));
        }

  
        

        private BasedOnDescriptor FindControllers()
        {
            return Types.FromThisAssembly()
               .BasedOn<IHttpController>()
               .If(Component.IsInSameNamespaceAs<UserController>())
               .If(t => t.Name.EndsWith("Controller"))
               .LifestyleTransient();
        }
    }
}
