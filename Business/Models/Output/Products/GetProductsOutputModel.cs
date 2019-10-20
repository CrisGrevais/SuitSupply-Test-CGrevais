using Business.Contracts.Models.Output.Products;
using Domain.Entities;
using System.Collections.Generic;

namespace Business.Models.Output.Products
{
    public class GetProductsOutputModel : IGetProductsOutputModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
