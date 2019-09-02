using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

// ReSharper disable CollectionNeverUpdated.Local

namespace KG.Data.EntityFrameworkCore
{
    public class RepositoryTest
    {
        private readonly Mock<DbContext> _context = new Mock<DbContext>();

        private readonly Fake _repository;

        public RepositoryTest()
        {
            _repository = new Fake(_context.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public void SaveChanges_Delegates_Call(bool acceptAll)
        {
            _repository.SaveChanges(acceptAll);

            _context.Verify(x => x.SaveChanges(acceptAll));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task SaveChangesAsync_Delegates_Call()
        {
            var token = new CancellationToken();

            await _repository.SaveChangesAsync(token);

            _context.Verify(x => x.SaveChangesAsync(token));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task SaveChangesAsync_AcceptAll_Delegates_Call()
        {
            var token = new CancellationToken();
            const bool acceptAll = true;

            await _repository.SaveChangesAsync(acceptAll, token);

            _context.Verify(x => x.SaveChangesAsync(acceptAll, token));
        }

        private class FakeEntity { }

        private class Fake : Repository<FakeEntity>
        {
            public Fake(DbContext context)
                : base(context) { }
        }
    }
}
