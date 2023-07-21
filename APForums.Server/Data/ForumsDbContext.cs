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

        public DbSet<Connection> Connections { get; set; }  

        public DbSet<ProfileTag> ProfileTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           /* base.OnModelCreating(modelBuilder);*/
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new SocialEntityTypeConfiguration().Configure(modelBuilder.Entity<Social>());
            new ClubEntityTypeConfiguration().Configure(modelBuilder.Entity<Club>());
            new EventEntityTypeConfiguration().Configure(modelBuilder.Entity<Event>());
            new ProfileTagEntityTypeConfiguration().Configure(modelBuilder.Entity<ProfileTag>());

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
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<User>()
                .HasMany(u => u.FollowingList)
                .WithMany(u => u.FollowersList)
                .UsingEntity<Connection>(
                    r => r.HasOne(c => c.Follower).WithMany().HasForeignKey(c => c.FollowedId),
                    l => l.HasOne(c => c.Followed).WithMany().HasForeignKey(c => c.FollowerId),
                    j => j.Property(c => c.Date).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate()
                );

            modelBuilder.Entity<User>()
                .HasMany(u => u.ProfileTags)
                .WithMany(pt => pt.Users)
                .UsingEntity("UserProfileTags");
        }

    }
}
