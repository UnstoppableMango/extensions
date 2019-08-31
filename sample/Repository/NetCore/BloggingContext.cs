using System.Collections.Generic;
using JetBrains.Annotations;
using KG.Data;
using NetCore.Model;

namespace NetCore
{
    internal class BloggingContext : UnitOfWorkBase
    {
        public BloggingContext([NotNull] IEnumerable<IRepository> repositories)
            : base(repositories) { }

        public IRepository<Blog> Blogs => Repository<Blog>();

        public IRepository<Post> Posts => Repository<Post>();
    }
}
