using Business.Models.Input.Products;
using Business.Models.Output.Export;
using Business.Models.Output.Products;
using Domain.Contracts.Errors;
using System.Configuration;
using WebAPI.Client.Models.Output;
using WebAPI.Client.Service.RestClient;

namespace WebAPI.Client.Service
{
    public class ProductApiClient : ExternalServiceBase, IProductApiClient
    {
        public ProductApiClient(IRestWebClient webClient, IErrorCollector errorCollector)
            : base(errorCollector, webClient)
        {
            ServiceUrl = ConfigurationManager.AppSettings["ProductApiUrl"];
        }

        public GenericResponse<AddProductOutputModel> AddProduct(AddProductInputModel input)
        {
            return CallWebClient<AddProductInputModel, AddProductOutputModel>(CreateServiceCallParameter(input, (request) => _webClient.Post(request).Result, "Products", ServiceUrl));
        }

        public GenericResponse<GetProductByIdOutputModel> GetProductById(GetProductByIdInputModel input)
        {
            return CallWebClient<GetProductByIdInputModel, GetProductByIdOutputModel>(CreateServiceCallParameter(input, (request) => _webClient.Get(request).Result, $"Products/{input.Id}", ServiceUrl));
        }

        public GenericResponse<GetProductsOutputModel> GetProducts(GetProductsInputModel input)
        {
            var requestUrl = $"Products/?codeOrName={input.CodeOrName}";
            return CallWebClient<GetProductsInputModel, GetProductsOutputModel>(CreateServiceCallParameter(input, (request) => _webClient.Get(request).Result, requestUrl, ServiceUrl));
        }

        public GenericResponse<DeleteProductOutputModel> DeleteProduct(DeleteProductInputModel input)
        {
            return CallWebClient<DeleteProductInputModel, DeleteProductOutputModel>(CreateServiceCallParameter(input,
                                                                                             (request) => _webClient.Delete(request).Result,
                                                                                             $"Products/{input.Id}", ServiceUrl));
        }

        public GenericResponse<UpdateProductOutputModel> UpdateProduct(UpdateProductInputModel input)
        {
            return CallWebClient<UpdateProductInputModel, UpdateProductOutputModel>(CreateServiceCallParameter(input,
                                                                                        (request) => _webClient.Put(request).Result,
                                                                                        "Products/", ServiceUrl));
        }

        public GenericResponse<ExportOutputModel> ExportProducts()
        {
            return CallWebClient<string, ExportOutputModel>(CreateServiceCallParameter(string.Empty,
                                                                                    (request) => _webClient.Post(request).Result,
                                                                                    "Products/Export", ServiceUrl));
        }
    }
}
