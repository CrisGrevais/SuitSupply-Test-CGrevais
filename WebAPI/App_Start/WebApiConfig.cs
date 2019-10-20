using Autofac;
using Autofac.Integration.WebApi;
using Data.Contexts;
using Domain.Errors;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace WebAPI.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.Register(c => new HttpContextWrapper(HttpContext.Current)).As<HttpContextBase>().InstancePerRequest();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ErrorCollector>().AsImplementedInterfaces().InstancePerRequest();

            builder.Register(c => new ProductContext(ConfigurationManager.ConnectionStrings["SuitSupplyTest"].ConnectionString))
                   .As<ProductContext>()
                   .InstancePerRequest();

            builder.RegisterModule(new Data.RegisterModules());
            builder.RegisterModule(new Business.RegisterModules());

            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.MapHttpAttributeRoutes();
        }
    }
}