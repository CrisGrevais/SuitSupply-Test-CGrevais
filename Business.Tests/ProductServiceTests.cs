using Business.Contracts.Services;
using Business.Models.Input.Products;
using Business.Services;
using Data.Contracts.Repositories;
using Domain.Contracts;
using Domain.Contracts.Entities;
using Domain.Contracts.Errors;
using Domain.Entities;
using Domain.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace Business.Tests
{
    [TestClass]
    public class ProductServiceTests
    {
        #region InitializeService

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BusinessServiceCannotBeInstantiatedWithInvalidDataTest()
        {
            var service = new ProductService(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BusinessServiceCannotBeInstantiatedWithMissingErrorCollectorTest()
        {
            var repository = new Mock<IProductRepository>();
            var service = new ProductService(repository.Object, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BusinessServiceCannotBeInstantiatedWithMissingExportServiceTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new Mock<IErrorCollector>();
            var service = new ProductService(repository.Object, errorCollector.Object, null);
        }

        [TestMethod]
        public void BusinessServiceInstantiatedSuccessfullyTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new Mock<IErrorCollector>();
            var exportService = new Mock<IExportService>();
            var service = new ProductService(repository.Object, errorCollector.Object, exportService.Object);
            Assert.IsNotNull(service);
        }

        #endregion

        #region AddProduct

        [TestMethod]
        public void AddProductWithNullInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);
            var outputModel = service.AddProduct(null);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void AddProductWithInvalidInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new AddProductInputModel
            {
            };

            var outputModel = service.AddProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void AddProductWithPriceOverMaxLimitTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new AddProductInputModel
            {
                Code = "T1",
                Name = "Test 1",
                Price = Constants.ProductMaxPrice + 1
            };

            var outputModel = service.AddProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void AddProductWithAlreadyExistingCodeTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.GetProducts(It.IsAny<string>())).Returns(new[]
            {
                new Product
                {
                    Code = "T1",
                    Name = "Test One",
                    Price = 15
                }
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new AddProductInputModel
            {
                Code = "T1",
                Name = "Test 1",
                Price = 10
            };

            var outputModel = service.AddProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void AddProductWithRepositoryDoesNotAddProductTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new AddProductInputModel
            {
                Code = "T1",
                Name = "Test 1",
                Price = 10
            };

            var outputModel = service.AddProduct(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsNull(outputModel.Product);
        }

        [TestMethod]
        public void AddProductSuccessTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.AddProduct(It.IsAny<IProduct>())).Returns(new Product
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new AddProductInputModel
            {
                Code = "T1",
                Name = "Test 1",
                Price = 10
            };

            var outputModel = service.AddProduct(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsNotNull(outputModel.Product);
        }

        [TestMethod]
        public void AddProductWithOverPriceSuccessTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.AddProduct(It.IsAny<IProduct>())).Returns(new Product
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = Constants.ProductMaxPrice + 1
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new AddProductInputModel
            {
                Code = "T1",
                Name = "Test 1",
                Price = Constants.ProductMaxPrice + 1,
                ConfirmPrice = true
            };

            var outputModel = service.AddProduct(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsNotNull(outputModel.Product);
        }

        #endregion

        #region UpdateProduct

        [TestMethod]
        public void UpdateProductWithNullInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);
            var outputModel = service.UpdateProduct(null);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void UpdateProductWithInvalidInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new UpdateProductInputModel
            {
            };

            var outputModel = service.UpdateProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void UpdateProductButProductDoesNotExistTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new UpdateProductInputModel
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            };

            var outputModel = service.UpdateProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void UpdateProductWithPriceOverMaxLimitTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new UpdateProductInputModel
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = Constants.ProductMaxPrice + 1
            };

            var outputModel = service.UpdateProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void UpdateProductWithAlreadyExistingCodeTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.GetProductById(It.IsAny<int>())).Returns(new Product
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            });

            repository.Setup(r => r.GetProducts(It.IsAny<string>())).Returns(new[]
            {
                new Product
                {
                    Id = 2,
                    Code = "T1",
                    Name = "Test One",
                    Price = 15
                }
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new UpdateProductInputModel
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            };

            var outputModel = service.UpdateProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void UpdateProductSuccessTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.GetProductById(It.IsAny<int>())).Returns(new Product
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new UpdateProductInputModel
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            };

            var outputModel = service.UpdateProduct(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsTrue(outputModel.Success);
        }

        [TestMethod]
        public void UpdateProductWithOverPriceSuccessTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.GetProductById(It.IsAny<int>())).Returns(new Product
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new UpdateProductInputModel
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = Constants.ProductMaxPrice + 1,
                ConfirmPrice = true
            };

            var outputModel = service.UpdateProduct(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsTrue(outputModel.Success);
        }

        #endregion

        #region DeleteProduct

        [TestMethod]
        public void DeleteProductWithNullInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);
            var outputModel = service.DeleteProduct(null);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void DeleteProductWithInvalidInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new DeleteProductInputModel
            {
            };

            var outputModel = service.DeleteProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void DeleteProductButProductDoesNotExistTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new DeleteProductInputModel
            {
                Id = 1
            };

            var outputModel = service.DeleteProduct(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void DeleteProductSuccessTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.GetProductById(It.IsAny<int>())).Returns(new Product
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new DeleteProductInputModel
            {
                Id = 1
            };

            var outputModel = service.DeleteProduct(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsTrue(outputModel.Success);
        }

        #endregion

        #region GetProductById

        [TestMethod]
        public void GetProductByIdWithNullInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);
            var outputModel = service.GetProductById(null);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void GetProductByIdWithInvalidInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new GetProductByIdInputModel
            {
            };

            var outputModel = service.GetProductById(inputModel);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void GetProductByIdButProductDoesNotExistTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new GetProductByIdInputModel
            {
                Id = 1
            };

            var outputModel = service.GetProductById(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsNull(outputModel.Product);
        }

        [TestMethod]
        public void GetProductByIdSuccessTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.GetProductById(It.IsAny<int>())).Returns(new Product
            {
                Id = 1,
                Code = "T1",
                Name = "Test 1",
                Price = 10
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new GetProductByIdInputModel
            {
                Id = 1
            };

            var outputModel = service.GetProductById(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsNotNull(outputModel.Product);
        }

        #endregion

        #region GetProducts

        [TestMethod]
        public void GetProductsWithNullInputModelTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);
            var outputModel = service.GetProducts(null);

            Assert.IsNull(outputModel);
            Assert.IsTrue(errorCollector.Errors.Any());
        }

        [TestMethod]
        public void GetProductsButProductDoesNotExistTest()
        {
            var repository = new Mock<IProductRepository>().Object;
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;
            var service = new ProductService(repository, errorCollector, exportService);

            var inputModel = new GetProductsInputModel
            {
                CodeOrName = "T1"
            };

            var outputModel = service.GetProducts(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsNotNull(outputModel.Products);
            Assert.IsFalse(outputModel.Products.Any());
        }

        [TestMethod]
        public void GetProductsSuccessTest()
        {
            var repository = new Mock<IProductRepository>();
            var errorCollector = new ErrorCollector();
            var exportService = new Mock<IExportService>().Object;

            repository.Setup(r => r.GetProducts(It.IsAny<string>())).Returns(new[]
            {
                new Product
                {
                    Id = 1,
                    Code = "T1",
                    Name = "Test 1",
                    Price = 10
                }
            });

            var service = new ProductService(repository.Object, errorCollector, exportService);

            var inputModel = new GetProductsInputModel
            {
                CodeOrName = "T1"
            };

            var outputModel = service.GetProducts(inputModel);

            Assert.IsNotNull(outputModel);
            Assert.IsFalse(errorCollector.Errors.Any());
            Assert.IsNotNull(outputModel.Products);
        }

        #endregion
    }
}