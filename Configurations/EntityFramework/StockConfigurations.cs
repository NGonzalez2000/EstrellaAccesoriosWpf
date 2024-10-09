using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstrellaAccesoriosWpf.Configurations.EntityFramework;

class StockConfigurations : IEntityTypeConfiguration<StockIncome>
{
    public void Configure(EntityTypeBuilder<StockIncome> builder)
    {
        builder.ToTable("StockIncomes");

        builder.HasKey(t => t.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.TotalPrice).HasPrecision(18, 2);
    }
}
