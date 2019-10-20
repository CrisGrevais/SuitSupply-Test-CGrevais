using Business.Contracts.Models.Output.Products;
using Domain.Entities;

namespace Business.Models.Output.Products
{
    public class AddProductOutputModel : IAddProductOutputModel
    {
        public Product Product { get; set; }
    }
}
