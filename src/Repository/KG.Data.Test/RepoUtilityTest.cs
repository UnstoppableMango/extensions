using Moq;
using System;
using Xunit;

namespace KG.Data
{
    public class RepoUtilityTest
    {
        private readonly Mock<IRepository> _repository = new Mock<IRepository>();
        private readonly Mock<IRepository<FakeEntity>> _genericRepository = new Mock<IRepository<FakeEntity>>();

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsIRepository_Type_Param_Returns_True()
        {
            var param = _repository.Object.GetType();

            var result = RepoUtility.ImplementsIRepository(param);

            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsIRepository_Type_Param_Returns_False()
        {
            var param = typeof(FakeEntity);

            var result = RepoUtility.ImplementsIRepository(param);

            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsIRepository_Null_Type_Param_Returns_False()
        {
            Type param = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var result = RepoUtility.ImplementsIRepository(param);

            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsIRepository_Object_Param_Returns_True()
        {
            var param = _repository.Object;

            var result = RepoUtility.ImplementsIRepository(param);

            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsIRepository_Object_Param_Returns_False()
        {
            var param = new FakeEntity();

            var result = RepoUtility.ImplementsIRepository(param);

            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsGenericIRepository_Type_Param_Returns_True()
        {
            var param = _genericRepository.Object.GetType();

            var result = RepoUtility.ImplementsGenericIRepository(param);

            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsGenericIRepository_Direct_Type_Param_Returns_True()
        {
            var param = typeof(IRepository<>);

            var result = RepoUtility.ImplementsGenericIRepository(param);

            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsGenericIRepository_Type_Param_Returns_False()
        {
            var param = typeof(FakeEntity);

            var result = RepoUtility.ImplementsGenericIRepository(param);

            Assert.False(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsGenericIRepository_Object_Param_Returns_True()
        {
            var param = _genericRepository.Object;

            var result = RepoUtility.ImplementsGenericIRepository(param);

            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void ImplementsGenericIRepository_Object_Param_Returns_False()
        {
            var param = new FakeEntity();

            var result = RepoUtility.ImplementsGenericIRepository(param);

            Assert.False(result);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public class FakeEntity { }
    }
}
