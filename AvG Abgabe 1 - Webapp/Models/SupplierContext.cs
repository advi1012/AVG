using Microsoft.EntityFrameworkCore;

namespace AvG_Abgabe_1___Webapp.Model
{
    public class SupplierContext : DbContext
    {
        public SupplierContext(DbContextOptions<SupplierContext> options)
            : base(options)
        { }

        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
