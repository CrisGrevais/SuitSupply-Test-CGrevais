using Business.Contracts.Models.Input.Products;

namespace Business.Models.Input.Products
{
    public class GetProductsInputModel : IGetProductsInputModel
    {
        public string CodeOrName { get; set; }

        public bool IsValid()
        {
            return true;
        }
    }
}
