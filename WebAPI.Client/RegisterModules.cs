using Autofac;
using WebAPI.Client.Service;
using WebAPI.Client.Service.RestClient;

namespace WebAPI.Client
{
    public class RegisterModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RestWebClient>().As<IRestWebClient>().InstancePerRequest();
            builder.RegisterType<ProductApiClient>().As<IProductApiClient>().InstancePerRequest();
        }
    }
}
