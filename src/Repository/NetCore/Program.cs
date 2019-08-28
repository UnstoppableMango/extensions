using KG.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NetCore.Model;
using NetCore.Repositories;
using System;

namespace NetCore
{
    /// <summary>
    ///     This sample app mimics the Microsoft EntityFrameworkCore
    ///     tutorial here <see href="https://docs.microsoft.com/en-us/ef/core/"/>.
    /// </summary>
    internal static class Program
    {
        private const string FORMAT = "{0,5} {1,7}   {2,-15} {3,-20}";

        private static void Main()
        {
            // Poor man's DI
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<BloggingDbContext>()
                .UseSqlite(connection)
                .Options;

            // Typically the DI container will handle the DbContext's lifecycle
            using (var dbContext = new BloggingDbContext(options))
            {
                dbContext.Database.EnsureCreated();

                var blogRepo = new BlogRepository(dbContext);
                var postRepo = new PostRepository(dbContext);
                var repos = new IRepository[] { blogRepo, postRepo };

                // Use the unit of work abstraction
                using (var context = new BloggingContext(repos))
                {
                    Console.WriteLine("List all posts:\n");
                    Console.WriteLine(FORMAT, "Blog", "Post", "Title", "Content");

                    // Get all posts
                    foreach (var post in context.Posts)
                        Console.WriteLine(FORMAT, post.BlogId, post.PostId, post.Title, post.Content);

                    Console.WriteLine();
                    Console.WriteLine();

                    Console.WriteLine("List all blogs:\n");
                    Console.WriteLine(FORMAT, "Blog", "Rating", "Url", string.Empty);

                    // Get all blogs
                    foreach (var blog in context.Blogs)
                        Console.WriteLine(FORMAT, blog.BlogId, blog.Rating, blog.Url, string.Empty);

                    Console.WriteLine();

                    Console.WriteLine("Add some posts...");

                    for (int i = 0; i < 5; i++)
                    {
                        var newPost = new Post { PostId = i, BlogId = 2, Title = $"Post {i + 1}", Content = $"Post {i + 1} on blog 2" };
                        context.Posts.Add(newPost);
                    }

                    // Persist our new additions just like we would with an EF DbContext
                    context.SaveChanges();

                    Console.WriteLine("List all posts:\n");
                    Console.WriteLine(FORMAT, "Blog", "Post", "Title", "Content");

                    // Get all posts again to reflect the updates
                    foreach (var post in context.Posts)
                        Console.WriteLine(FORMAT, post.BlogId, post.PostId, post.Title, post.Content);
                }
            }

            connection.Close();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
