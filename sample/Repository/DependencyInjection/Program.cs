using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DependencyInjection
{
    internal static class Program
    {
        private static void Main()
        {
            var services = new ServiceCollection();

            // Quirk with using in memory sqlite data source with EFCore.
            // The connection object needs to be manually created and opened.
            var sqlConnection = new SqliteConnection("Data Source=:memory:");
            sqlConnection.Open();

            services.AddUnitOfWork<BloggingUnitOfWork>(options =>
            {
                options.UseSqlite(sqlConnection);
            });

            var provider = services.BuildServiceProvider();

            using (sqlConnection)
            using (var scope = provider.CreateScope())
            {
                var uow = scope.ServiceProvider.GetRequiredService<BloggingUnitOfWork>();

                uow.Database.EnsureCreated();

                Console.WriteLine($"Resolved {uow}");

                foreach (var blog in uow.Blogs)
                {
                    Console.Write("Blog: ");
                    Console.WriteLine(blog.BlogId);
                }

                foreach (var post in uow.Posts)
                {
                    Console.Write("Post: ");
                    Console.Write(post.PostId);

                    Console.Write(" for blog ");
                    Console.WriteLine(post.BlogId);
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
