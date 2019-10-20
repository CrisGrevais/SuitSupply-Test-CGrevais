using Business.Contracts.Models.Output.Products;

namespace Business.Models.Output.Products
{
    public class UpdateProductOutputModel : IUpdateProductOutputModel
    {
        public bool Success { get; set; }
    }
}
