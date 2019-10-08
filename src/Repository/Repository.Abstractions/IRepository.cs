using System.Collections.Generic;

namespace UnMango.Extensions.Repository
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IRepository<T> : IRepository
    {
        T Find(params object[] keyValues);

        T Add(T entity);

        void AddRange(params T[] entities) => AddRange((IEnumerable<T>)entities);

        void AddRange(IEnumerable<T> entities);

        T Remove(T entity);

        void RemoveRange(params T[] entities) => RemoveRange((IEnumerable<T>)entities);

        void RemoveRange(IEnumerable<T> entities);

        T Update(T entity);

        void UpdateRange(params T[] entities) => UpdateRange((IEnumerable<T>)entities);

        void UpdateRange(IEnumerable<T> entities);
    }
}
