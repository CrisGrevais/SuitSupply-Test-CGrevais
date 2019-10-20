using System;
using System.ComponentModel.DataAnnotations;

namespace Web.UI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Display(Name = "Code")]
        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        public DateTime LastUpdated { get; set; }

        [Display(Name = "Confirm Price (It might be required based on entered price)")]
        public bool ConfirmPrice { get; set; }
    }
}