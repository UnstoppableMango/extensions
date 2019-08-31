using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using KG.Data;

namespace TestShared
{
    public sealed class FakeGenericRepository : IRepository<FakeEntity>
    {
        private readonly List<FakeEntity> _entities = new List<FakeEntity> {
            new FakeEntity()
        };

        public FakeGenericRepository()
        {
            ElementType = typeof(FakeEntity);
            Expression = _entities.AsQueryable().Expression;
            Provider = _entities.AsQueryable().Provider;
            ContainsListCollection = true;
        }

        public Type ElementType { get; }

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        public bool ContainsListCollection { get; }

        public void Add(FakeEntity entity) { }

        public Task AddAsync(
            FakeEntity entity,
            CancellationToken cancellationToken = default)
            => default;

        public void AddRange(params FakeEntity[] entities) { }

        public void AddRange(IEnumerable<FakeEntity> entities) { }

        public Task AddRangeAsync(params FakeEntity[] entities)
            => default;

        public Task AddRangeAsync(
            IEnumerable<FakeEntity> entities,
            CancellationToken cancellationToken = default)
            => default;

        public void Attach(FakeEntity entity) { }

        public void AttachRange(params FakeEntity[] entities) { }

        public void AttachRange(IEnumerable<FakeEntity> entities) { }

        public FakeEntity Find(params object[] keyValues) => default;

        public Task<FakeEntity> FindAsync(params object[] keyValues) => default;

        public Task<FakeEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
            => default;

        public IEnumerator<FakeEntity> GetEnumerator() => default;

        public void Remove(FakeEntity entity) { }

        public void RemoveRange(params FakeEntity[] entities) { }

        public void RemoveRange(IEnumerable<FakeEntity> entities) { }

        public bool SaveChanges() => default;

        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) => default;

        public void Update(FakeEntity entity) { }

        public void UpdateRange(params FakeEntity[] entities) { }

        public void UpdateRange(IEnumerable<FakeEntity> entities) { }

        IEnumerator IEnumerable.GetEnumerator() => default;

        public IList GetList() => default;
    }
}
