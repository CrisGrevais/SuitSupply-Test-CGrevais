using Data.Contexts;
using Data.Contracts.Repositories;
using Domain.Contracts.Entities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        protected readonly ProductContext _dbContext;

        public ProductRepository(ProductContext context)
        {
            this._dbContext = context;
        }

        public IProduct AddProduct(IProduct product)
        {
            product.LastUpdated = DateTime.UtcNow;
            _dbContext.Products.Add((Product)product);
            _dbContext.SaveChanges();
            return product;
        }

        public void DeleteProduct(IProduct product)
        {
            _dbContext.Products.Remove((Product)product);
            _dbContext.SaveChanges();
        }

        public IProduct GetProductById(int id)
        {
            return _dbContext.Products.Find(id);
        }

        public IEnumerable<IProduct> GetProducts(string codeOrName)
        {
            var products = _dbContext.Products.ToArray();

            if (!string.IsNullOrWhiteSpace(codeOrName))
            {
                products = products.Where(product => product.Name.Contains(codeOrName) ||
                                                     product.Code.Contains(codeOrName)).ToArray();
            }

            return products;
        }

        public void UpdateProduct(IProduct product)
        {
            product.LastUpdated = DateTime.UtcNow;
            _dbContext.SaveChanges();
        }
    }
}
