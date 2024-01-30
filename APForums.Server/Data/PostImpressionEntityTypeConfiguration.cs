using APForums.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APForums.Server.Data
{
    public class PostImpressionEntityTypeConfiguration : IEntityTypeConfiguration<PostImpression>
    {
        public void Configure(EntityTypeBuilder<PostImpression> builder)
        {
            builder.Property(pi => pi.Value)
                .HasConversion<int>();

            builder.Property(pi => pi.LastUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
