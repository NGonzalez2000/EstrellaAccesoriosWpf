using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstrellaAccesoriosWpf.Configurations.EntityFramework
{
    internal class CashCloseConfigurations : IEntityTypeConfiguration<CashClose>
    {
        public void Configure(EntityTypeBuilder<CashClose> builder)
        {
            builder.ToTable("CashCloses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.Balance)
                .HasPrecision(18, 2);
        }
    }
}
