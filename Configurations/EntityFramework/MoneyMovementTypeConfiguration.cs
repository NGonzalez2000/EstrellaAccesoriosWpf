using EstrellaAccesoriosWpf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EstrellaAccesoriosWpf.Configurations.EntityFramework
{
    internal class MoneyMovementTypeConfiguration : IEntityTypeConfiguration<MoneyMovementType>
    {
        public void Configure(EntityTypeBuilder<MoneyMovementType> builder)
        {
            builder.ToTable("MoneyMovementTypes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

        }
    }
}
