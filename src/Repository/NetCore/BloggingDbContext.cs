using Microsoft.EntityFrameworkCore;
using NetCore.Model;

namespace NetCore
{
    internal class BloggingDbContext : DbContext
    {
        public BloggingDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Blog>()
                .ToTable("Blogs")
                .HasKey(x => x.BlogId);
            builder.Entity<Post>()
                .ToTable("Posts")
                .HasKey(x => new { x.PostId, x.BlogId });

            // Seeding
            builder.Entity<Blog>().HasData(
               new Blog { BlogId = 1, Url = "https://www.kumandgo.com/", Rating = 5 },
               new Blog { BlogId = 2, Url = "https://www.kumandgo.com/", Rating = 4 });

            builder.Entity<Post>().HasData(
                new Post { BlogId = 1, PostId = 1, Title = "First post", Content = "Test 1" },
                new Post { BlogId = 1, PostId = 2, Title = "Second post", Content = "Test 2" });
        }
    }
}
