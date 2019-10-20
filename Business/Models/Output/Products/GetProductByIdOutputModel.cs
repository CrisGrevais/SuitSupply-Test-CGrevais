using Business.Contracts.Models.Output.Products;
using Domain.Entities;

namespace Business.Models.Output.Products
{
    public class GetProductByIdOutputModel : IGetProductByIdOutputModel
    {
        public Product Product { get; set; }
    }
}
