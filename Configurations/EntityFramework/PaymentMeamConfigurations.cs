using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstrellaAccesoriosWpf.Configurations.EntityFramework;

internal class PaymentMeanConfigurations : IEntityTypeConfiguration<PaymentMean>
{
    public void Configure(EntityTypeBuilder<PaymentMean> builder)
    {
        builder.ToTable("PaymentMeans");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
