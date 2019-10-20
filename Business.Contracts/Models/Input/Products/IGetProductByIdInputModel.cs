using Domain.Contracts.Entities;

namespace Business.Contracts.Models.Input.Products
{
    public interface IGetProductByIdInputModel : IValidable
    {
        int Id { get; set; }
    }
}
