using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstrellaAccesoriosWpf.Configurations.EntityFramework;

internal class MoneyMovementConfigurations : IEntityTypeConfiguration<MoneyMovement>
{
    public void Configure(EntityTypeBuilder<MoneyMovement> builder)
    {
        builder.ToTable("MoneyMovements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);
    }
}
