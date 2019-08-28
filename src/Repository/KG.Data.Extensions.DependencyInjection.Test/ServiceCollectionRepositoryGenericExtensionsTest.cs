using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestShared;
using Xunit;

namespace KG.Data.Extensions.DependencyInjection
{
    public class ServiceCollectionRepositoryGenericExtensionsTest
    {
        private readonly Mock<IServiceCollection> _services = new Mock<IServiceCollection>();
        
        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Throws_When_NonConcrete_Type_Used(ServiceLifetime lifetime)
        {
            Assert.Throws<ArgumentException>(() =>
                ServiceCollectionExtensions.AddRepository<IRepository>(_services.Object, lifetime));

            Assert.Throws<ArgumentException>(() =>
                ServiceCollectionExtensions.AddRepository<IRepository<FakeEntity>>(_services.Object, lifetime));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Returns_The_Original_ServiceCollection(ServiceLifetime lifetime)
        {
            var services = ServiceCollectionExtensions.AddRepository<FakeRepo>(_services.Object, lifetime);

            Assert.Same(_services.Object, services);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_RepoType_As_Itself(ServiceLifetime lifetime)
        {
            var type = typeof(FakeRepo);

            ServiceCollectionExtensions.AddRepository<FakeRepo>(_services.Object, lifetime);

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
            ServiceCollectionExtensions.AddRepository<FakeRepo>(_services.Object, lifetime);

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
            ServiceCollectionExtensions.AddRepository<FakeRepo>(_services.Object, lifetime);

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
            ServiceCollectionExtensions.AddRepository<FakeRepo>(_services.Object, lifetime);

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
            var type = typeof(FakeGenericRepository);

            ServiceCollectionExtensions.AddRepository<FakeGenericRepository>(_services.Object, lifetime);

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
            ServiceCollectionExtensions.AddRepository<FakeGenericRepository>(_services.Object, lifetime);

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
            ServiceCollectionExtensions.AddRepository<FakeGenericRepository>(_services.Object, lifetime);

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
            ServiceCollectionExtensions.AddRepository<FakeGenericRepository>(_services.Object, lifetime);

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
            ServiceCollectionExtensions.AddRepository<FakeRepo>(_services.Object);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Scoped)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AddRepository_Adds_NonDisposable_RepoType_With_Transient_Lifetime()
        {
            ServiceCollectionExtensions.AddRepository<FakeGenericRepository>(_services.Object);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.Lifetime == ServiceLifetime.Transient)));
        }

        private class FakeRepo : FakeRepositoryBase, IDisposable
        {
            public void Dispose() { }
        }
    }
}
