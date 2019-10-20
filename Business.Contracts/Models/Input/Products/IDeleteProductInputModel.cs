using Domain.Contracts.Entities;

namespace Business.Contracts.Models.Input.Products
{
    public interface IDeleteProductInputModel : IValidable
    {
        int Id { get; set; }
    }
}
