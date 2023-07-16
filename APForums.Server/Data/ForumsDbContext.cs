using Microsoft.EntityFrameworkCore;

namespace APForums.Server.Data
{
    public class ForumsDbContext : DbContext
    {

        public ForumsDbContext()
        {

        }

        public ForumsDbContext(DbContextOptions<ForumsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
