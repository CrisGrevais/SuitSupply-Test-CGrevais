using Business.Contracts.Models.Output.Export;
using Business.Contracts.Models.Output.Products;
using Business.Contracts.Services;
using Business.Models.Input.Products;
using Domain.Contracts.Errors;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ProductController : ApiControllerBase
    {
        protected readonly IProductService _service;

        public ProductController(IProductService service, IErrorCollector errorCollector)
            : base(errorCollector)
        {
            _service = service;
        }

        [Route("products/"), HttpGet]
        public GenericResponse<IGetProductsOutputModel> Get([FromUri] string codeOrName)
        {
            return new GenericResponse<IGetProductsOutputModel>
            {
                Code = 200,
                Result = _service.GetProducts(new GetProductsInputModel
                {
                    CodeOrName = codeOrName
                })
            };
        }

        [Route("products/{id}"), HttpGet]
        public GenericResponse<IGetProductByIdOutputModel> GetById([FromUri] int id)
        {
            return new GenericResponse<IGetProductByIdOutputModel>
            {
                Code = 200,
                Result = _service.GetProductById(new GetProductByIdInputModel
                {
                    Id = id
                })
            };
        }

        [Route("products/"), HttpPost]
        public GenericResponse<IAddProductOutputModel> Add([FromBody] AddProductInputModel inputModel)
        {
            return new GenericResponse<IAddProductOutputModel>
            {
                Code = 200,
                Result = _service.AddProduct(inputModel)
            };
        }

        [Route("products/"), HttpPut]
        public GenericResponse<IUpdateProductOutputModel> Update([FromBody] UpdateProductInputModel inputModel)
        {
            return new GenericResponse<IUpdateProductOutputModel>
            {
                Code = 200,
                Result = _service.UpdateProduct(inputModel)
            };
        }

        [Route("products/{id}"), HttpDelete]
        public GenericResponse<IDeleteProductOutputModel> Delete([FromUri] int id)
        {
            return new GenericResponse<IDeleteProductOutputModel>
            {
                Code = 200,
                Result = _service.DeleteProduct(new DeleteProductInputModel
                {
                    Id = id
                })
            };
        }

        [Route("products/export"), HttpPost]
        public GenericResponse<IExportOutputModel> Export()
        {
            return new GenericResponse<IExportOutputModel>
            {
                Code = 200,
                Result = _service.ExportProducts()
            };
        }
    }
}