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

        public DbSet<UserClub> UserClubs { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventInterest> EventInterests { get; set; }

        public DbSet<Connection> Connections { get; set; }  

        public DbSet<ProfileTag> ProfileTags { get; set; }

        public DbSet<UserProfileTags> UsersProfileTags { get; set; }

        public DbSet<Forum> Forums { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<CommentImpression> CommentImpressions { get; set; }

        public DbSet<PostImpression> PostImpressions { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<UserActivity> UserActivities { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           /* base.OnModelCreating(modelBuilder);*/
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new SocialEntityTypeConfiguration().Configure(modelBuilder.Entity<Social>());
            new ClubEntityTypeConfiguration().Configure(modelBuilder.Entity<Club>());
            new EventEntityTypeConfiguration().Configure(modelBuilder.Entity<Event>());
            new ProfileTagEntityTypeConfiguration().Configure(modelBuilder.Entity<ProfileTag>());
            new PostEntityTypeConfiguration().Configure(modelBuilder.Entity<Post>());
            new CommentEntityTypeConfiguration().Configure(modelBuilder.Entity<Comment>());
            new PostImpressionEntityTypeConfiguration().Configure(modelBuilder.Entity<PostImpression>());
            new CommentImpressionEntityTypeConfiguration().Configure(modelBuilder.Entity<CommentImpression>());
            new ActivityEntityTypeConfiguration().Configure(modelBuilder.Entity<Activity>());
            new ForumsEntityTypeConfiguration().Configure(modelBuilder.Entity<Forum>());

            modelBuilder.Entity<Club>()
            .HasMany(c => c.Users)
            .WithMany(u => u.Clubs)
            .UsingEntity<UserClub>(
                l => l.HasOne<User>(uc => uc.User).WithMany(u => u.UserClubs),
                r => r.HasOne<Club>(uc => uc.Club).WithMany(c => c.UserClubs));

            modelBuilder.Entity<UserClub>()
                .Property(uc => uc.Role)
                .HasConversion<int>()
                .HasDefaultValue(ClubRole.Member);

            modelBuilder.Entity<UserClub>()
                .Property(uc => uc.LastUpdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<User>()
                .HasMany(u => u.FollowingList)
                .WithMany(u => u.FollowersList)
                .UsingEntity<Connection>(
                    r => r.HasOne(c => c.Follower).WithMany().HasForeignKey(c => c.FollowerId),
                    l => l.HasOne(c => c.Followed).WithMany().HasForeignKey(c => c.FollowedId),
                    j => j.Property(c => c.Date).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate()
                );

            modelBuilder.Entity<User>()
                .HasMany(u => u.ProfileTags)
                .WithMany(pt => pt.Users)
                .UsingEntity<UserProfileTags>(
                    l => l.HasOne<ProfileTag>(e => e.ProfileTag).WithMany(e => e.UserProfileTags),
                    r => r.HasOne<User>(e => e.User).WithMany(e => e.UserProfileTags));

            modelBuilder.Entity<User>()
               .HasMany(u => u.Events)
               .WithMany(e => e.InterestedUsers)
               .UsingEntity<EventInterest>(
                   l => l.HasOne<Event>(e => e.Event).WithMany(e => e.EventInterests),
                   r => r.HasOne<User>(e => e.User).WithMany(e => e.EventInterests));

            modelBuilder.Entity<User>()
               .HasMany(u => u.PostsWithImpressions)
               .WithMany(p => p.UsersWithImpressions)
               .UsingEntity<PostImpression>(
                   l => l.HasOne<Post>(p => p.Post).WithMany(p => p.Impressions),
                   r => r.HasOne<User>(u => u.User).WithMany(p => p.PostImpressions));

            modelBuilder.Entity<User>()
               .HasMany(u => u.UserPosts)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId)
               .IsRequired();

            modelBuilder.Entity<User>()
               .HasMany(u => u.PostsWithImpressions)
               .WithMany(p => p.UsersWithImpressions)
               .UsingEntity<PostImpression>(
                   l => l.HasOne<Post>(p => p.Post).WithMany(p => p.Impressions),
                   r => r.HasOne<User>(u => u.User).WithMany(p => p.PostImpressions));

            modelBuilder.Entity<User>()
               .HasMany(u => u.UserComments)
               .WithOne(c => c.User)
               .HasForeignKey(c => c.UserId)
               .IsRequired();

            modelBuilder.Entity<User>()
               .HasMany(u => u.CommentsWithImpressions)
               .WithMany(c => c.UsersWithImpressions)
               .UsingEntity<CommentImpression>(
                   l => l.HasOne<Comment>(c => c.Comment).WithMany(c => c.Impressions),
                   r => r.HasOne<User>(u => u.User).WithMany(u => u.CommentImpressions));

            modelBuilder.Entity<Post>()
                .HasMany(p => p.PostTags)
                .WithMany(pt => pt.Posts)
                .UsingEntity("posts_post_tags");

            modelBuilder.Entity<User>()
            .HasMany(u => u.SubscribedForums)
            .WithMany(f => f.SubscribedUsers)
            .UsingEntity("forum_subscriptions");

            modelBuilder.Entity<User>()
                .HasMany(u => u.Activities)
                .WithMany(a => a.Users)
                .UsingEntity<UserActivity>();

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .IsRequired(false);

        }

    }
}
