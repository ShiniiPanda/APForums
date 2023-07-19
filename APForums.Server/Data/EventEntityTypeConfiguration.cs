using APForums.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APForums.Server.Data
{
    public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(e => e.PostedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP()")
                .ValueGeneratedOnAdd();
        }
    }
}
