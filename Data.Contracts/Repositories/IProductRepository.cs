using Domain.Contracts.Entities;
using System.Collections.Generic;

namespace Data.Contracts.Repositories
{
    public interface IProductRepository
    {
        IProduct AddProduct(IProduct product);
        void UpdateProduct(IProduct product);
        void DeleteProduct(IProduct product);
        IProduct GetProductById(int id);
        IEnumerable<IProduct> GetProducts(string codeOrName);
    }
}
