using Domain.Entities;
using System.Collections.Generic;

namespace Business.Contracts.Models.Output.Products
{
    public interface IGetProductsOutputModel
    {
        IEnumerable<Product> Products { get; set; }
    }
}
