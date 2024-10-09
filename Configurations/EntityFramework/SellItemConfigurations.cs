using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstrellaAccesoriosWpf.Configurations.EntityFramework;

internal class SellItemConfigurations : IEntityTypeConfiguration<SellItem>
{
    public void Configure(EntityTypeBuilder<SellItem> builder)
    {
        builder.ToTable("SellItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.ProductPrice)
            .HasPrecision(18, 2);

    }
}
