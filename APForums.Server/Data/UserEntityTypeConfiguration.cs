using APForums.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace APForums.Server.Data
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Make The TP Number for each user unique, add an index for faster lookups
            builder.HasIndex(u => u.TPNumber).IsUnique();

            // Virtual table, data is not persisted for storage efficiency. Email is always computed as TP Email when retrieved.
            builder.Property(u => u.Email)
                .HasComputedColumnSql("CONCAT(TpNumber, '@mail.apu.edu.my')", stored: false);

            // Enum to String conversions for storage and retrieval
            builder.Property(u => u.DegreeType)
                .HasColumnType("nvarchar(20)");

            builder.Property(u => u.Course)
                .HasColumnType("nvarchar(10)");

            builder.Property(u => u.Enrollment)
                .HasColumnType("nvarchar(20)");

            builder.Property(u => u.Department)
                .HasColumnType("nvarchar(20)");
        }
    }
}
