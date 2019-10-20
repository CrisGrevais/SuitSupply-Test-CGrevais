using Business.Contracts.Models.Input.Products;

namespace Business.Models.Input.Products
{
    public class UpdateProductInputModel : AddProductInputModel, IUpdateProductInputModel
    {
        public int Id { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && Id > 0;
        }
    }
}
