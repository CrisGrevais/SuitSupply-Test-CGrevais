using Autofac;
using Data.Contracts.Repositories;
using Data.Repositories;

namespace Data
{
    public class RegisterModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
        }
    }
}
