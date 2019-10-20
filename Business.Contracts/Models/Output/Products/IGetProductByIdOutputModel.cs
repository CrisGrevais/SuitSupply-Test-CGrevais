using Domain.Entities;

namespace Business.Contracts.Models.Output.Products
{
    public interface IGetProductByIdOutputModel
    {
        Product Product { get; set; }
    }
}
