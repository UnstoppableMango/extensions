using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UnMango.Extensions.Repository.EntityFrameworkCore
{
    public interface IEFRepository<T> : IRepository<T, DbContext>, IQueryable<T>, IEnumerable<T>, IEnumerable, IQueryable, IAsyncEnumerable<T>, IListSource
        where T : class
    {
        DbContext Context { get; }

        DbSet<T> Set { get; }
    }
}
