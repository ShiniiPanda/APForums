using APForums.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APForums.Server.Data
{
    public class CommentImpressionEntityTypeConfiguration : IEntityTypeConfiguration<CommentImpression>
    {
        public void Configure(EntityTypeBuilder<CommentImpression> builder)
        {
            builder.Property(ci => ci.Value)
                 .HasConversion<int>();

            builder.Property(ci => ci.LastUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
