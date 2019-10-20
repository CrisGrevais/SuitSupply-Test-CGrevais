using Business.Models.Input.Products;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Models;
using Web.UI.Models.Binders;
using WebAPI.Client.Service;

namespace Web.UI.Controllers
{
    public class ProductController : Controller
    {
        protected readonly IProductApiClient _service;
        
        public ProductController(IProductApiClient service)
        {
            this._service = service;
        }

        public ActionResult Index(string name)
        {
            var getProductsResponse = _service.GetProducts(new GetProductsInputModel
            {
                CodeOrName = name
            });
            if (getProductsResponse != null &&
                getProductsResponse.Errors != null &&
                getProductsResponse.Errors.Any())
            {
                ViewBag.Message = getProductsResponse.Errors.FirstOrDefault();
            }

            return View(getProductsResponse?.Result?.Products.Select(p => new ProductModel
            {
                Code = p.Code,
                Id = p.Id,
                Name = p.Name,
                LastUpdated = p.LastUpdated,
                Photo = p.Photo,
                Price = p.Price
            }).ToArray());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([ModelBinder(typeof(ProductModelBinder))] ProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var addProductResponse = _service.AddProduct(new AddProductInputModel
                    {
                        Code = model.Code,
                        Name = model.Name,
                        Price = model.Price,
                        Photo = GetPhotoPath(),
                        ConfirmPrice = model.ConfirmPrice
                    });

                    if (addProductResponse != null &&
                        addProductResponse.Errors != null &&
                        addProductResponse.Errors.Any())
                    {
                        var errorMessage = addProductResponse.Errors.FirstOrDefault();
                        ViewBag.Message = errorMessage.Message;
                    }
                    else
                    {
                        ModelState.Clear();
                        return RedirectToAction("Index");
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            var getProductByIdResponse = _service.GetProductById(new GetProductByIdInputModel
            {
                Id = id
            });
            if (getProductByIdResponse != null &&
                getProductByIdResponse.Errors != null &&
                getProductByIdResponse.Errors.Any())
            {
                ViewBag.Message = getProductByIdResponse.Errors.FirstOrDefault();
            }

            if (getProductByIdResponse != null &&
                getProductByIdResponse.Result != null &&
                getProductByIdResponse.Result.Product != null)
            {
                var product = getProductByIdResponse.Result.Product;
                return View(new ProductModel
                {
                    Code = product.Code,
                    Id = product.Id,
                    LastUpdated = product.LastUpdated,
                    Name = product.Name,
                    Photo = product.Photo,
                    Price = product.Price
                });
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            var getProductByIdResponse = _service.GetProductById(new GetProductByIdInputModel
            {
                Id = id
            });
            if (getProductByIdResponse != null &&
                getProductByIdResponse.Errors != null &&
                getProductByIdResponse.Errors.Any())
            {
                ViewBag.Message = getProductByIdResponse.Errors.FirstOrDefault();
            }

            if (getProductByIdResponse != null &&
                getProductByIdResponse.Result != null &&
                getProductByIdResponse.Result.Product != null)
            {
                var product = getProductByIdResponse.Result.Product;
                return View(new ProductModel
                {
                    Code = product.Code,
                    Id = product.Id,
                    LastUpdated = product.LastUpdated,
                    Name = product.Name,
                    Photo = product.Photo,
                    Price = product.Price
                });
            }

            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, [ModelBinder(typeof(ProductModelBinder))] ProductModel model)
        {
            try
            {
                var updateProductResponse = _service.UpdateProduct(new UpdateProductInputModel
                {
                    Code = model.Code,
                    Name = model.Name,
                    Price = model.Price,
                    Photo = GetPhotoPath(),
                    Id = id,
                    ConfirmPrice = model.ConfirmPrice
                });
                if (updateProductResponse != null &&
                    updateProductResponse.Errors != null &&
                    updateProductResponse.Errors.Any())
                {
                    var errorMessage = updateProductResponse.Errors.FirstOrDefault();
                    ViewBag.Message = errorMessage.Message;
                    return View(model);
                }
                else
                {
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var deleteProductResponse = _service.DeleteProduct(new DeleteProductInputModel
                {
                    Id = id
                });
                if (deleteProductResponse != null &&
                    deleteProductResponse.Errors != null &&
                    deleteProductResponse.Errors.Any())
                {
                    var errorMessage = deleteProductResponse.Errors.FirstOrDefault();
                    ViewBag.Message = errorMessage.Message;
                    return View();
                }
                else
                {
                    ViewBag.AlertMsg = "Product Deleted Successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public FileContentResult Export()
        {
            try
            {
                var exportProductsResponse = _service.ExportProducts();
                if (exportProductsResponse != null &&
                    exportProductsResponse.Errors != null &&
                    exportProductsResponse.Errors.Any())
                {
                    var errorMessage = exportProductsResponse.Errors.FirstOrDefault();
                    ViewBag.Message = errorMessage.Message;
                    return null;
                }

                var file = exportProductsResponse.Result.File;
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Report.xlsx");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Protected

        protected string GetPhotoPath()
        {
            var files = HttpContext.Request.Files;
            if (files == null || files.Count <= 0)
            {
                return string.Empty;
            }

            var file = files[0];
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var photo = $"{Guid.NewGuid().ToString()}_{fileName}";
                var path = Path.Combine(
                    Server.MapPath("~/Photos"), photo);
                file.SaveAs(path);
                return $"/Photos/{photo}";
            }

            return string.Empty;
        }

        #endregion
    }
}