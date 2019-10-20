using Domain.Entities;

namespace Business.Contracts.Models.Output.Products
{
    public interface IAddProductOutputModel
    {
        Product Product { get; set; }
    }
}
