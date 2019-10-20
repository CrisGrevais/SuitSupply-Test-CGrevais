using Autofac;
using Business.Contracts.Services;
using Business.Services;

namespace Business
{
    public class RegisterModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExportService>().As<IExportService>();
            builder.RegisterType<ProductService>().As<IProductService>();
        }
    }
}
