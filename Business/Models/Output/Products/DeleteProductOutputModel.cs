using Business.Contracts.Models.Output.Products;

namespace Business.Models.Output.Products
{
    public class DeleteProductOutputModel : IDeleteProductOutputModel
    {
        public bool Success { get; set; }
    }
}
