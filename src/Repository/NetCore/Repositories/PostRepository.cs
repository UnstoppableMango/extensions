using JetBrains.Annotations;
using KG.Data.EntityFrameworkCore;
using NetCore.Model;

namespace NetCore.Repositories
{
    internal class PostRepository : Repository<Post>
    {
        public PostRepository([NotNull] BloggingDbContext context)
            : base(context) { }
    }
}
