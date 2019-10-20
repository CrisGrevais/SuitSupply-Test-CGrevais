using Domain.Entities;
using System.Data.Entity;

namespace Data.Contexts
{
    public class ProductContext : DbContext
    {
        public ProductContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
