using Business.Contracts.Models.Input.Products;
using Business.Contracts.Models.Output.Export;
using Business.Contracts.Models.Output.Products;

namespace Business.Contracts.Services
{
    public interface IProductService
    {
        IAddProductOutputModel AddProduct(IAddProductInputModel inputModel);
        IUpdateProductOutputModel UpdateProduct(IUpdateProductInputModel inputModel);
        IDeleteProductOutputModel DeleteProduct(IDeleteProductInputModel inputModel);
        IGetProductByIdOutputModel GetProductById(IGetProductByIdInputModel inputModel);
        IGetProductsOutputModel GetProducts(IGetProductsInputModel inputModel);
        IExportOutputModel ExportProducts();
    }
}
