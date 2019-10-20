using Domain.Contracts.Entities;

namespace Business.Contracts.Models.Input.Products
{
    public interface IUpdateProductInputModel : IAddProductInputModel, IValidable
    {
        int Id { get; set; }
    }
}
