using Domain.Contracts.Entities;

namespace Business.Contracts.Models.Input.Products
{
    public interface IGetProductsInputModel : IValidable
    {
        string CodeOrName { get; set; }
    }
}
