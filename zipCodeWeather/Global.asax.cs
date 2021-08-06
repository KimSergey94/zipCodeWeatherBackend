using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace zipCodeWeather
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RegisterIoC();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void RegisterIoC()
        {
            var builderMvc = new ContainerBuilder();
            builderMvc.RegisterControllers(Assembly.GetExecutingAssembly());
            builderMvc.RegisterApiControllers(Assembly.GetExecutingAssembly());
            AutofacBootstrap.Init(builderMvc);
            var containerMvc = builderMvc.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(containerMvc));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(containerMvc);
        }
    }
}
