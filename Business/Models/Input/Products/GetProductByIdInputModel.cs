using Business.Contracts.Models.Input.Products;

namespace Business.Models.Input.Products
{
    public class GetProductByIdInputModel : IGetProductByIdInputModel
    {
        public int Id { get; set; }

        public bool IsValid()
        {
            return Id > 0;
        }
    }
}
