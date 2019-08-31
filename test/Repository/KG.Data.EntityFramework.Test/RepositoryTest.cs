using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace KG.Data.EntityFramework.Test
{
    public class RepositoryTest
    {
        private readonly Mock<DbSet<object>> _entities = new Mock<DbSet<object>>();
        private readonly Mock<DbContext> _context = new Mock<DbContext>();

        private readonly Fake _repository;

        public RepositoryTest()
        {
            _context.Setup(x => x.Set<object>()).Returns(_entities.Object);
            _repository = new Fake(_context.Object);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Adds_Entity()
        {
            var obj = new object();

            _repository.Add(obj);

            _entities.Verify(x => x.Add(obj));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Adds_Entity_Async()
        {
            var obj = new object();

#pragma warning disable CS0618 // Type or member is obsolete
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.AddAsync(obj));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Adds_Range_Params()
        {
            var arr = new object[0];

            _repository.AddRange(arr);

            _entities.Verify(x => x.AddRange(arr));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Adds_Range_Params_Async()
        {
            var arr = new object[0];

#pragma warning disable CS0618 // Type or member is obsolete
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.AddRangeAsync(arr));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Adds_Range_Enumerable()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var arr = new List<object>();

            _repository.AddRange(arr);

            _entities.Verify(x => x.AddRange(arr));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Adds_Range_Enumerable_Async()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var arr = new List<object>();

#pragma warning disable CS0618 // Type or member is obsolete
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.AddRangeAsync(arr));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Attaches()
        {
            var obj = new object();

            _repository.Attach(obj);

            _entities.Verify(x => x.Attach(obj));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Attaches_Range()
        {
            var obj = new object();
            var arr = new[] { obj };

            _repository.AttachRange(arr);

            _entities.Verify(x => x.Attach(obj));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public async Task Finds_By_Key(bool async)
        {
            const string key = "key";
            var obj = new object();
            _entities.Setup(x => x.Find(key)).Returns(obj);
            _entities.Setup(x => x.FindAsync(key)).ReturnsAsync(obj);

            Assert.Same(obj, async
                ? await _repository.FindAsync(key)
                : _repository.Find(key));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Removes()
        {
            var obj = new object();

            _repository.Remove(obj);

            _entities.Verify(x => x.Remove(obj));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Removes_Range()
        {
            var arr = new object[0];

            _repository.RemoveRange(arr);

            _entities.Verify(x => x.RemoveRange(arr));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Removes_Range_Enumerable()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var arr = new List<object>();

            _repository.RemoveRange(arr);

            _entities.Verify(x => x.RemoveRange(arr));
        }

        // TODO: Finish adding test cases for our pseudo-update function
        //[Fact(Skip = "Can't mock the necessary values atm. Need to research further")]
        [Trait("Category", "Unit")]
        public void Updates()
        {
            var obj = new object();

            _repository.Update(obj);

            //_entities.Verify(x => x.Update(obj));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Saves_No_Changes_Success()
        {
            _context.Setup(x => x.SaveChanges()).Returns(0);

            var result = ((IRepository)_repository).SaveChanges();

            _context.Verify(x => x.SaveChanges());
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Saves_One_Change_Success()
        {
            _context.Setup(x => x.SaveChanges()).Returns(1);

            var result = ((IRepository)_repository).SaveChanges();

            _context.Verify(x => x.SaveChanges());
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Save_Changes_Fails()
        {
            _context.Setup(x => x.SaveChanges()).Returns(-1);

            var result = ((IRepository)_repository).SaveChanges();

            _context.Verify(x => x.SaveChanges());
            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Saves_No_Changes_Success_Async()
        {
            _context.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            var result = await ((IRepository)_repository).SaveChangesAsync();

            _context.Verify(x => x.SaveChangesAsync());
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Saves_One_Change_Success_Async()
        {
            _context.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var result = await ((IRepository)_repository).SaveChangesAsync();

            _context.Verify(x => x.SaveChangesAsync());
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Save_Changes_Fails_Async()
        {
            _context.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

            var result = await ((IRepository)_repository).SaveChangesAsync();

            _context.Verify(x => x.SaveChangesAsync());
            Assert.False(result);
        }

        private class Fake : Repository<object>
        {
            public Fake(DbContext context)
                : base(context) { }
        }
    }
}
