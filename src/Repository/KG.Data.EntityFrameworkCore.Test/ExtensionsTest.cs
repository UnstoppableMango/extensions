using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace KG.Data.EntityFrameworkCore
{
    public class ExtensionsTest
    {
        private readonly Mock<DbContext> _context = new Mock<DbContext>();
        private readonly Mock<Repository<object>> _repository;

        public ExtensionsTest()
        {
            _repository = new Mock<Repository<object>>(_context.Object);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Invalid_Repo()
        {
            var badRepo = new Mock<IRepository<object>>();

            await Assert.ThrowsAsync<InvalidOperationException>(()
                => RepositoryExtensions.SaveChangesAsync(badRepo.Object, default));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [Trait("Category", "Unit")]
        public void Saves_Changes_AcceptAll(int dbResult)
        {
            _context.Setup(x => x.SaveChanges(It.IsAny<bool>())).Returns(dbResult);

            var result = RepositoryExtensions.SaveChanges(_repository.Object, true);

            Assert.Equal(dbResult, result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Saves_Changes_AcceptAll_Fail()
        {
            _context.Setup(x => x.SaveChanges(It.IsAny<bool>())).Returns(-1);

            var result = RepositoryExtensions.SaveChanges(_repository.Object, true);

            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [Trait("Category", "Unit")]
        public async Task Saves_Changes_Async(int dbResult)
        {
            var token = new CancellationToken();
            _context.Setup(x => x.SaveChangesAsync(token)).ReturnsAsync(dbResult);

            var result = await RepositoryExtensions.SaveChangesAsync(_repository.Object, token);

            Assert.Equal(dbResult, result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Save_Changes_Asnyc_Fails()
        {
            var token = new CancellationToken();
            _context.Setup(x => x.SaveChangesAsync(token)).ReturnsAsync(-1);

            var result = await RepositoryExtensions.SaveChangesAsync(_repository.Object, token);

            Assert.Equal(-1, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [Trait("Category", "Unit")]
        public async Task Saves_Changes_AcceptAll_Async(int dbResult)
        {
            var token = new CancellationToken();
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<bool>(), token)).ReturnsAsync(dbResult);

            var result = await RepositoryExtensions.SaveChangesAsync(_repository.Object, true, token);

            Assert.Equal(dbResult, result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Save_Changes_Async_AcceptAll_Fails()
        {
            var token = new CancellationToken();
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<bool>(), token)).ReturnsAsync(-1);

            var result = await RepositoryExtensions.SaveChangesAsync(_repository.Object, true, token);

            Assert.Equal(-1, result);
        }
    }
}
