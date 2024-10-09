using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstrellaAccesoriosWpf.Configurations.EntityFramework;

internal class SellConfigurations : IEntityTypeConfiguration<Sell>
{
    public void Configure(EntityTypeBuilder<Sell> builder)
    {
        builder.ToTable("Sells");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.SubTotalPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.FixedDiscount)
            .HasPrecision(18, 2);

        builder.Property(x => x.PercentageDiscount)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalEarned)
            .HasPrecision(18, 2);
    }
}
