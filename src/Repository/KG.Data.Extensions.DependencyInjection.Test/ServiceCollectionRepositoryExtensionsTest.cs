using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace KG.Data.Extensions.DependencyInjection
{
    public class ServiceCollectionRepositoryExtensionsTest
    {
        private readonly Mock<IServiceCollection> _services = new Mock<IServiceCollection>();

        private readonly Mock<IRepository> _repository = new Mock<IRepository>();
        private readonly Mock<IRepository<FakeEntity>> _genericRepository = new Mock<IRepository<FakeEntity>>();

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Throws_When_NonRepo_Type_Used(ServiceLifetime lifetime)
        {
            var type = typeof(object);

            Assert.Throws<ArgumentException>(() =>
                ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Throws_When_NonConcrete_Type_Used(ServiceLifetime lifetime)
        {
            var type = typeof(IRepository);

            Assert.Throws<ArgumentException>(() =>
                ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Returns_The_Original_ServiceCollection(ServiceLifetime lifetime)
        {
            var type = _repository.Object.GetType();

            var services = ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            Assert.Same(_services.Object, services);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_RepoType_As_Itself(ServiceLifetime lifetime)
        {
            var type = _repository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.ServiceType == type &&
                d.ImplementationType == type)));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_RepoType_As_IRepository(ServiceLifetime lifetime)
        {
            var type = _repository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.ServiceType == typeof(IRepository))));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_RepoType_With_Passed_Lifetime(ServiceLifetime lifetime)
        {
            var type = _repository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            _services.Verify(
                x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.Lifetime == lifetime)
                ),
                Times.Exactly(2));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Does_Not_Add_NonGeneric_RepoType_As_GenericIRepository(ServiceLifetime lifetime)
        {
            var type = _repository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            _services.Verify(
                x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.ServiceType == typeof(IRepository<>)
                )),
                Times.Never);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_GenericRepoType_As_Itself(ServiceLifetime lifetime)
        {
            var type = _genericRepository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.ServiceType == type &&
                d.ImplementationType == type)));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_GenericRepoType_As_IRepository(ServiceLifetime lifetime)
        {
            var type = _genericRepository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.ServiceType == typeof(IRepository))));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_GenericRepoType_As_GenericIRepository(ServiceLifetime lifetime)
        {
            var type = _genericRepository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.ServiceType == typeof(IRepository<FakeEntity>))));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_GenericRepoType_With_Passed_Lifetime(ServiceLifetime lifetime)
        {
            var type = _genericRepository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type, lifetime);

            _services.Verify(
                x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.Lifetime == lifetime)
                ),
                Times.Exactly(3));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_Disposable_RepoType_With_Scoped_Lifetime()
        {
            _genericRepository.As<IDisposable>();
            var type = _genericRepository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Scoped)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_NonDisposable_RepoType_With_Transient_Lifetime()
        {
            var type = _genericRepository.Object.GetType();

            ServiceCollectionExtensions.AddRepository(_services.Object, type);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Transient)));
        }

        public class FakeEntity { }
    }
}
