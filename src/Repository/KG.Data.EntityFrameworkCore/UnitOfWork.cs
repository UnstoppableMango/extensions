using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using KG.Data.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;

namespace KG.Data.EntityFrameworkCore
{
    /// <summary>
    /// A helper class for implementing <see cref="IUnitOfWork"/> with <see cref="Microsoft.EntityFrameworkCore"/>.
    /// </summary>
    public class UnitOfWork : DbContext, IUnitOfWork
    {
        [UsedImplicitly]
        public UnitOfWork([NotNull] DbContextOptions options)
            : base(options)
        {
            InitializeRepositories();
        }

        private void InitializeRepositories()
        {
            var properties = GetType().GetProperties()
                .Where(p => RepoUtility.ImplementsGenericIRepository(p.PropertyType)
                            && p.GetValue(this) == null);

            foreach (var propinfo in properties)
            {
                var entityType = propinfo.PropertyType.GenericTypeArguments.Single();
                var repo = CreateRepository(entityType);
                propinfo.SetValue(this, repo);
            }
        }

        private object CreateRepository(Type entityType)
        {
            var type = typeof(InternalRepository<>).MakeGenericType(entityType);

            return Activator.CreateInstance(type, this);
        }

        #region Unit Of Work Support

        bool IUnitOfWork.SaveChanges() => base.SaveChanges() >= 0;

        async Task<bool> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
            => (await base.SaveChangesAsync(cancellationToken)) >= 0;

        #endregion
    }
}
