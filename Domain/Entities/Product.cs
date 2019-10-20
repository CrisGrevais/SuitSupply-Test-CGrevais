using Domain.Contracts.Entities;
using System;

namespace Domain.Entities
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Code) &&
                   !string.IsNullOrWhiteSpace(Name) &&
                   Price > 0;
        }
    }
}
