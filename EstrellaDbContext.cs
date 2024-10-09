using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;

namespace EstrellaAccesoriosWpf;

public class EstrellaDbContext(DbContextOptions<EstrellaDbContext> options) : DbContext(options)
{
    public DbSet<CashClose> CashCloses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<MoneyMovement> MoneyMovements { get; set; }
    public DbSet<MoneyMovementType> MoneyMovementTypes { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Sell> Sells { get; set; }
    public DbSet<SellItem> SellItems { get; set; }
    public DbSet<StockIncome> StockIncomes { get; set; }
    public DbSet<StockItem> StockItems { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EstrellaDbContext).Assembly);
    }
}
