using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UnMango.Extensions.Repository.EntityFrameworkCore
{
    public interface IEFRepository<T> : IEFRepository<T, DbContext>
        where T : class
    { }

    public interface IEFRepository<TEntity, TContext> :
        IAsyncRepository<TEntity>,
        IQueryable<TEntity>,
        IEnumerable<TEntity>,
        IEnumerable,
        IQueryable,
        IAsyncEnumerable<TEntity>,
        IListSource
        where TEntity : class
        where TContext : DbContext
    {
        TContext Context { get; }

        DbSet<TEntity> Set { get; }
    }
}
