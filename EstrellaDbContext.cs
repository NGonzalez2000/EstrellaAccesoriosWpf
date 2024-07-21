using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace EstrellaAccesoriosWpf;

public class EstrellaDbContext(DbContextOptions<EstrellaDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<MoneyMovement> MoneyMovements { get; set; }
    public DbSet<PaymentMean> PaymentMeans { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EstrellaDbContext).Assembly);
    }
}
