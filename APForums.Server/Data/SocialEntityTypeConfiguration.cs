using APForums.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APForums.Server.Data
{
    public class SocialEntityTypeConfiguration : IEntityTypeConfiguration<Social>
    {
        public void Configure(EntityTypeBuilder<Social> builder)
        {
            builder.Property(s => s.Type)
                .HasColumnType("nvarchar(20)");

            builder.Property(s => s.LastUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP()")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
