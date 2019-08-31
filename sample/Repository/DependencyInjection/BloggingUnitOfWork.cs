using JetBrains.Annotations;
using DependencyInjection.Model;
using KG.Data;
using KG.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DependencyInjection
{
    [UsedImplicitly]
    internal class BloggingUnitOfWork : UnitOfWork
    {
        public BloggingUnitOfWork(DbContextOptions options)
            : base(options) { }

        public IRepository<Blog> Blogs { get; set; }

        public IRepository<Post> Posts { get; set; }

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
