using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnMango.Extensions.Repository.EntityFrameworkCore
{
    public class Repository<T> : Repository<T, DbContext>, IEFRepository<T>
        where T : class
    {
        public Repository(DbContext context)
            : base(context, new UnitOfWorkContextAdapter(context)) { }
    }

    public class Repository<TEntity, TContext> : IEFRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        public Repository(TContext context)
            : this(context, GetUnitOfWork(context))
        { }

        public Repository(TContext context, IUnitOfWork unitOfWork)
        {
            Context = context;
            UnitOfWork = unitOfWork;
        }

        public TContext Context { get; }

        public IUnitOfWork UnitOfWork { get; }

        public DbSet<TEntity> Set => Context.Set<TEntity>();

        bool IListSource.ContainsListCollection => ((IListSource)Set).ContainsListCollection;

        Type IQueryable.ElementType => ((IQueryable)Set).ElementType;

        Expression IQueryable.Expression => ((IQueryable)Set).Expression;

        IQueryProvider IQueryable.Provider => ((IQueryable)Set).Provider;

        public virtual TEntity Add(TEntity entity) => Set.Add(entity).Entity;

        public virtual async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entry = await Set.AddAsync(entity, cancellationToken);

            return entry.Entity;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities) => Set.AddRange(entities);

        public virtual ValueTask AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return new ValueTask(Set.AddRangeAsync(entities, cancellationToken));
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return Set.Find(keyValues);
        }

        public virtual ValueTask<TEntity> FindAsync(params object[] keyValues)
        {
            return Set.FindAsync(keyValues);
        }

        public virtual ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default)
        {
            return Set.FindAsync(keyValues, cancellationToken);
        }

        public virtual TEntity Remove(TEntity entity)
        {
            return Set.Remove(entity).Entity;
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
        }

        public virtual TEntity Update(TEntity entity)
        {
            return Set.Update(entity).Entity;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            Set.UpdateRange(entities);
        }

        IList IListSource.GetList() => ((IListSource)Set).GetList();

        IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return ((IAsyncEnumerable<TEntity>)Set).GetAsyncEnumerator(cancellationToken);
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return ((IEnumerable<TEntity>)Set).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Set).GetEnumerator();
        }

        private static IUnitOfWork GetUnitOfWork(TContext context)
        {
            return context is IUnitOfWork unitOfWork
                ? unitOfWork
                : new UnitOfWorkContextAdapter<TContext>(context);
        }
    }
}
