using APForums.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APForums.Server.Data
{
    public class ProfileTagEntityTypeConfiguration : IEntityTypeConfiguration<ProfileTag>
    {
        public void Configure(EntityTypeBuilder<ProfileTag> builder)
        {
            builder.Property(pt => pt.Created)
                 .HasDefaultValueSql("CURRENT_TIMESTAMP")
                 .ValueGeneratedOnAdd();

            builder.Property(pt => pt.Updated)
                 .HasDefaultValueSql("CURRENT_TIMESTAMP")
                 .ValueGeneratedOnUpdate();
        }
    }
}
