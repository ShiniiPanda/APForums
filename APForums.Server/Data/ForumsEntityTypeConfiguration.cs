using APForums.Server.Models;
using APForums.Server.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APForums.Server.Data
{
    public class ForumsEntityTypeConfiguration : IEntityTypeConfiguration<Forum>
    {
        public void Configure(EntityTypeBuilder<Forum> builder)
        {
            builder.Property(f => f.Visibility)
                .HasConversion<int>()
                .HasDefaultValue(ForumVisibility.Public);
        }
    }
}
