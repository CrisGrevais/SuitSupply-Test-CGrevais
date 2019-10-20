using Domain.Contracts.Entities;

namespace Business.Contracts.Models.Input.Products
{
    public interface IAddProductInputModel : IValidable
    {
        string Code { get; set; }
        string Name { get; set; }
        string Photo { get; set; }
        decimal Price { get; set; }
        bool ConfirmPrice { get; set; }
    }
}
