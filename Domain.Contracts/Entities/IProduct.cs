using System;

namespace Domain.Contracts.Entities
{
    public interface IProduct : IValidable
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string Photo { get; set; }
        decimal Price { get; set; }
        DateTime LastUpdated { get; set; }
    }
}
