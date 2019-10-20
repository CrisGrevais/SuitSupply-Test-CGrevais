using Business.Contracts.Models.Input.Products;

namespace Business.Models.Input.Products
{
    public class AddProductInputModel : IAddProductInputModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }
        public bool ConfirmPrice { get; set; }

        public virtual bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Code) &&
                   !string.IsNullOrWhiteSpace(Name) &&
                   Price > 0;
        }
    }
}
