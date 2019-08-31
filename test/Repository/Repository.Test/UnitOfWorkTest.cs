using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KG.Data
{
    public class UnitOfWorkTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public void Defaults_Repositories_With_Default_Constructor(bool generic)
        {
            var unitOfWork = new Fake();

            Assert.Null(generic
                ? unitOfWork.Repository<object>()
                : unitOfWork.Repository(typeof(object)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Adds_Repository()
        {
            var repoToAdd = new Mock<IRepository<object>>();
            var unitOfWork = new Fake();

            unitOfWork.Add(repoToAdd.Object);

            Assert.Collection(unitOfWork.GetRepositories(),
                x => Assert.Same(repoToAdd.Object, x));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Throws_On_Duplicate()
        {
            var mockRepo = new Mock<IRepository<object>>();
            var repoToAdd = new Mock<IRepository<object>>();
            var unitOfWork = new Fake(new[] { mockRepo.Object });

            Assert.Throws<InvalidOperationException>(
                () => unitOfWork.Add(repoToAdd.Object));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Doesnt_Allow_Null()
        {
            var unitOfWork = new Fake();

            Assert.Throws<ArgumentNullException>(
                // ReSharper disable once AssignNullToNotNullAttribute
                () => unitOfWork.Add((IRepository<object>)null));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public void Gets_Single_Repository(bool generic)
        {
            var mockRepo = new Mock<IRepository<object>>();
            var unitOfWork = new Fake(new[] { mockRepo.Object });

            Assert.Same(mockRepo.Object, generic
                ? unitOfWork.Repository<object>()
                : unitOfWork.Repository(typeof(object)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public void Gets_Single_Repository_From_Multiple(bool generic)
        {
            var objRepo = new Mock<IRepository<object>>();
            var strRepo = new Mock<IRepository<string>>();
            var unitOfWork = new Fake(new IRepository[] { objRepo.Object, strRepo.Object });

            Assert.Same(strRepo.Object, generic
                ? unitOfWork.Repository<string>()
                : unitOfWork.Repository(typeof(string)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public void Returns_Null_For_No_Repository(bool generic)
        {
            var objRepo = new Mock<IRepository<object>>();
            var unitOfWork = new Fake(new[] { objRepo.Object });

            Assert.Null(generic
                ? unitOfWork.Repository<string>()
                : unitOfWork.Repository(typeof(string)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public void Doesnt_Throw_For_NonGeneric_Repository(bool generic)
        {
            var mockRepo = new Mock<IRepository>();
            var unitOfWork = new Fake(new[] { mockRepo.Object });

            Assert.Null(generic
                ? unitOfWork.Repository<object>()
                : unitOfWork.Repository(typeof(object)));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public async Task Save_For_No_Repositories(bool async)
        {
            var unitOfWork = new Fake();

            Assert.True(async
                ? await unitOfWork.SaveChangesAsync()
                : unitOfWork.SaveChanges());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public async Task Save_Single_Repository_Success(bool async)
        {
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.SaveChanges()).Returns(true);
            mockRepo.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(true);
            var unitOfWork = new Fake(new[] { mockRepo.Object });

            Assert.True(async
                ? await unitOfWork.SaveChangesAsync()
                : unitOfWork.SaveChanges());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public async Task Save_Single_Repository_Fail(bool async)
        {
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(x => x.SaveChanges()).Returns(false);
            mockRepo.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(false);
            var unitOfWork = new Fake(new[] { mockRepo.Object });

            Assert.False(async
                ? await unitOfWork.SaveChangesAsync()
                : unitOfWork.SaveChanges());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public async Task Save_Multiple_Repository_Success(bool async)
        {
            var mockRepo1 = new Mock<IRepository>();
            var mockRepo2 = new Mock<IRepository>();
            mockRepo1.Setup(x => x.SaveChanges()).Returns(true);
            mockRepo1.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(true);
            mockRepo2.Setup(x => x.SaveChanges()).Returns(true);
            mockRepo2.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(true);
            var unitOfWork = new Fake(new[] { mockRepo1.Object, mockRepo2.Object });

            Assert.True(async
                ? await unitOfWork.SaveChangesAsync()
                : unitOfWork.SaveChanges());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public async Task Save_Multiple_Repository_Fail(bool async)
        {
            var mockRepo1 = new Mock<IRepository>();
            var mockRepo2 = new Mock<IRepository>();
            mockRepo1.Setup(x => x.SaveChanges()).Returns(false);
            mockRepo1.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(false);
            mockRepo2.Setup(x => x.SaveChanges()).Returns(false);
            mockRepo2.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(false);
            var unitOfWork = new Fake(new[] { mockRepo1.Object, mockRepo2.Object });

            Assert.False(async
                ? await unitOfWork.SaveChangesAsync()
                : unitOfWork.SaveChanges());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "Unit")]
        public async Task Save_One_Repository_Fail(bool async)
        {
            var mockRepo1 = new Mock<IRepository>();
            var mockRepo2 = new Mock<IRepository>();
            mockRepo1.Setup(x => x.SaveChanges()).Returns(false);
            mockRepo1.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(false);
            mockRepo2.Setup(x => x.SaveChanges()).Returns(true);
            mockRepo2.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(true);
            var unitOfWork = new Fake(new[] { mockRepo1.Object, mockRepo2.Object });

            Assert.False(async
                ? await unitOfWork.SaveChangesAsync()
                : unitOfWork.SaveChanges());
        }

        private class Fake : UnitOfWorkBase
        {
            public Fake() { }

            public Fake(IEnumerable<IRepository> repositories)
                : base(repositories) { }

            public IEnumerable<IRepository> GetRepositories() => Repositories;
        }
    }
}
