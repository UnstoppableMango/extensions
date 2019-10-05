using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Repository.EntityFrameworkCore
{
    public class Repository<T> : IEntityFrameworkRepository<T>
        where T : class
    {
        public Repository(DbContext context)
        {
            Context = context;
        }

        public DbContext Context { get; }

        public DbSet<T> Set => Context.Set<T>();

        Type IQueryable.ElementType => ((IQueryable)Set).ElementType;

        Expression IQueryable.Expression => ((IQueryable)Set).Expression;

        IQueryProvider IQueryable.Provider => ((IQueryable)Set).Provider;

        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return ((IAsyncEnumerable<T>)Set).GetAsyncEnumerator(cancellationToken);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)Set).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Set).GetEnumerator();
        }
    }
}
