using APForums.Server.Models;
using APForums.Server.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace APForums.Server.Data
{
    public class ForumsDbContext : DbContext
    {

        public ForumsDbContext()
        {

        }

        public ForumsDbContext(DbContextOptions<ForumsDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Social> Socials { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           /* base.OnModelCreating(modelBuilder);*/
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new SocialEntityTypeConfiguration().Configure(modelBuilder.Entity<Social>());
            new ClubEntityTypeConfiguration().Configure(modelBuilder.Entity<Club>());
            new EventEntityTypeConfiguration().Configure(modelBuilder.Entity<Event>());

            modelBuilder.Entity<Club>()
            .HasMany(c => c.Users)
            .WithMany(u => u.Clubs)
            .UsingEntity<UserClub>();

            modelBuilder.Entity<UserClub>()
                .Property(uc => uc.Role)
                .HasColumnType("nvarchar(10)")
                .HasDefaultValue(ClubRole.Member);

            modelBuilder.Entity<UserClub>()
                .Property(uc => uc.LastUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP()")
                .ValueGeneratedOnAddOrUpdate();
        }

    }
}
