using Business.Models.Input.Products;
using Business.Models.Output.Export;
using Business.Models.Output.Products;
using WebAPI.Client.Models.Output;

namespace WebAPI.Client.Service
{
    public interface IProductApiClient : IExternalService
    {
        GenericResponse<GetProductByIdOutputModel> GetProductById(GetProductByIdInputModel input);
        GenericResponse<GetProductsOutputModel> GetProducts(GetProductsInputModel input);
        GenericResponse<AddProductOutputModel> AddProduct(AddProductInputModel input);
        GenericResponse<UpdateProductOutputModel> UpdateProduct(UpdateProductInputModel input);
        GenericResponse<DeleteProductOutputModel> DeleteProduct(DeleteProductInputModel input);
        GenericResponse<ExportOutputModel> ExportProducts();
    }
}
