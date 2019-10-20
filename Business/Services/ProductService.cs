using Business.Contracts.Models.Input.Products;
using Business.Contracts.Models.Output.Export;
using Business.Contracts.Models.Output.Products;
using Business.Contracts.Services;
using Business.Errors;
using Business.Models.Input.Export;
using Business.Models.Output.Products;
using Data.Contracts.Repositories;
using Domain.Contracts;
using Domain.Contracts.Errors;
using Domain.Entities;
using System;
using System.Linq;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        protected readonly IProductRepository _repository;
        protected readonly IErrorCollector _errorCollector;
        protected readonly IExportService _exportService;

        public ProductService(IProductRepository repository,
                              IErrorCollector errorCollector,
                              IExportService exportService)
        {
            if (repository == null ||
                errorCollector == null ||
                exportService == null)
            {
                throw new ArgumentException("ProductService dependencies cannot be NULL.");
            }

            this._repository = repository;
            this._errorCollector = errorCollector;
            this._exportService = exportService;
        }

        public IAddProductOutputModel AddProduct(IAddProductInputModel inputModel)
        {
            if (inputModel == null || !inputModel.IsValid())
            {
                _errorCollector.Errors.Add(BusinessErrors.InvalidInput);
                return null;
            }

            if (inputModel.Price > Constants.ProductMaxPrice && !inputModel.ConfirmPrice)
            {
                _errorCollector.Errors.Add(BusinessErrors.ProductPriceRequiresConfirmation);
                return null;
            }

            var existingProducts = _repository.GetProducts(inputModel.Code);
            if (existingProducts != null &&
                existingProducts.Any(p => string.Equals(p.Code, inputModel.Code, StringComparison.OrdinalIgnoreCase)))
            {
                _errorCollector.Errors.Add(BusinessErrors.ProductCodeAlreadyExists);
                return null;
            }

            var product = _repository.AddProduct(new Product
            {
                Code = inputModel.Code,
                Name = inputModel.Name,
                Photo = inputModel.Photo,
                Price = inputModel.Price
            });

            return new AddProductOutputModel
            {
                Product = product as Product
            };
        }

        public IUpdateProductOutputModel UpdateProduct(IUpdateProductInputModel inputModel)
        {
            if (inputModel == null || !inputModel.IsValid())
            {
                _errorCollector.Errors.Add(BusinessErrors.InvalidInput);
                return null;
            }

            var product = _repository.GetProductById(inputModel.Id);
            if (product == null)
            {
                _errorCollector.Errors.Add(BusinessErrors.ProductNotFound);
                return null;
            }

            if (inputModel.Price > Constants.ProductMaxPrice && !inputModel.ConfirmPrice)
            {
                _errorCollector.Errors.Add(BusinessErrors.ProductPriceRequiresConfirmation);
                return null;
            }

            var existingProducts = _repository.GetProducts(inputModel.Code);
            if (existingProducts != null &&
                existingProducts.Any(p => string.Equals(p.Code, inputModel.Code, StringComparison.OrdinalIgnoreCase) && p.Id != inputModel.Id))
            {
                _errorCollector.Errors.Add(BusinessErrors.ProductCodeAlreadyExists);
                return null;
            }

            product.Code = inputModel.Code;
            product.Name = inputModel.Name;
            product.Photo = inputModel.Photo;
            product.Price = inputModel.Price;

            _repository.UpdateProduct(product);

            return new UpdateProductOutputModel
            {
                Success = true
            };
        }

        public IDeleteProductOutputModel DeleteProduct(IDeleteProductInputModel inputModel)
        {
            if (inputModel == null || !inputModel.IsValid())
            {
                _errorCollector.Errors.Add(BusinessErrors.InvalidInput);
                return null;
            }

            var product = _repository.GetProductById(inputModel.Id);
            if (product == null)
            {
                _errorCollector.Errors.Add(BusinessErrors.ProductNotFound);
                return null;
            }

            _repository.DeleteProduct(product);

            return new DeleteProductOutputModel
            {
                Success = true
            };
        }

        public IGetProductByIdOutputModel GetProductById(IGetProductByIdInputModel inputModel)
        {
            if (inputModel == null || !inputModel.IsValid())
            {
                _errorCollector.Errors.Add(BusinessErrors.InvalidInput);
                return null;
            }

            return new GetProductByIdOutputModel
            {
                Product = _repository.GetProductById(inputModel.Id) as Product
            };
        }

        public IGetProductsOutputModel GetProducts(IGetProductsInputModel inputModel)
        {
            if (inputModel == null || !inputModel.IsValid())
            {
                _errorCollector.Errors.Add(BusinessErrors.InvalidInput);
                return null;
            }

            return new GetProductsOutputModel
            {
                Products = _repository.GetProducts(inputModel.CodeOrName).Select(p => p as Product).Where(p => p != null).ToArray()
            };
        }

        public IExportOutputModel ExportProducts()
        {
            return _exportService.Export(new ExportInputModel<Product>
            {
                EntityName = "Products",
                SourceData = _repository.GetProducts(string.Empty).Select(p => (Product)p).ToArray()
            });
        }
    }
}
