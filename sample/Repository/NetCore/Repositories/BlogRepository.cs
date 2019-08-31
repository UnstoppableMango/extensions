using JetBrains.Annotations;
using KG.Data.EntityFrameworkCore;
using NetCore.Model;

namespace NetCore.Repositories
{
    internal class BlogRepository : Repository<Blog>
    {
        public BlogRepository([NotNull] BloggingDbContext context)
            : base(context) { }
    }
}
