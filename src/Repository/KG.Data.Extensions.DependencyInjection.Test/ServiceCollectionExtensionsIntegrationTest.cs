using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TestShared;
using Xunit;
// ReSharper disable InvokeAsExtensionMethod

namespace KG.Data.Extensions.DependencyInjection
{
    public class ServiceCollectionExtensionsIntegrationTest
    {
        private readonly IServiceCollection _services = new ServiceCollection();

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Throws_When_NonRepo_Type_Used(ServiceLifetime lifetime)
        {
            var type = typeof(object);

            Assert.Throws<ArgumentException>(() =>
                ServiceCollectionExtensions.AddRepository(_services, type, lifetime));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Throws_When_NonConcrete_Type_Used(ServiceLifetime lifetime)
        {
            var type = typeof(IRepository);

            Assert.Throws<ArgumentException>(() =>
                ServiceCollectionExtensions.AddRepository(_services, type, lifetime));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Returns_The_Original_ServiceCollection(ServiceLifetime lifetime)
        {
            var type = typeof(FakeRepo);

            var services = ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            Assert.Same(_services, services);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_RepoType_As_Itself(ServiceLifetime lifetime)
        {
            var type = typeof(FakeRepo);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var provider = _services.BuildServiceProvider();
            var service = provider.GetService(type);
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_RepoType_As_IRepository(ServiceLifetime lifetime)
        {
            var type = typeof(FakeRepo);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var provider = _services.BuildServiceProvider();
            var service = provider.GetService<IRepository>();
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_Single_Service(ServiceLifetime lifetime)
        {
            var type = typeof(FakeRepo);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var provider = _services.BuildServiceProvider();
            var service1 = provider.GetService(type);
            var service2 = provider.GetService<IRepository>();
            Assert.NotNull(service1);
            Assert.NotNull(service2);
            Assert.Same(service1, service2);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_RepoType_With_Passed_Lifetime(ServiceLifetime lifetime)
        {
            var type = typeof(FakeRepo);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var services = _services.Where(x => x.ImplementationType == type);
            Assert.NotNull(services);
            Assert.Collection(services,
                service => Assert.Equal(lifetime, service.Lifetime));
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Does_Not_Add_NonGeneric_RepoType_As_GenericIRepository(ServiceLifetime lifetime)
        {
            var type = typeof(FakeRepo);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var provider = _services.BuildServiceProvider();
            var service = provider.GetService(typeof(IRepository<>));
            Assert.Null(service);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_GenericRepoType_As_Itself(ServiceLifetime lifetime)
        {
            var type = typeof(FakeGenericRepository);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var provider = _services.BuildServiceProvider();
            var service = provider.GetService(type);
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_GenericRepoType_As_IRepository(ServiceLifetime lifetime)
        {
            var type = typeof(FakeGenericRepository);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var provider = _services.BuildServiceProvider();
            var service = provider.GetService<IRepository>();
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_GenericRepoType_As_GenericIRepository(ServiceLifetime lifetime)
        {
            var type = typeof(FakeGenericRepository);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var provider = _services.BuildServiceProvider();
            var service = provider.GetService(typeof(IRepository<FakeEntity>));
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Singleton)]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_GenericRepoType_With_Passed_Lifetime(ServiceLifetime lifetime)
        {
            var type = typeof(FakeGenericRepository);

            ServiceCollectionExtensions.AddRepository(_services, type, lifetime);

            var services = _services.Where(x => x.ImplementationType == type);
            Assert.NotNull(services);
            Assert.Collection(services,
                service => Assert.Equal(lifetime, service.Lifetime));
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_Disposable_RepoType_With_Scoped_Lifetime()
        {
            var type = typeof(FakeRepo);

            ServiceCollectionExtensions.AddRepository(_services, type);

            var service = _services.FirstOrDefault(x => x.ImplementationType == type);
            Assert.Equal(ServiceLifetime.Scoped, service?.Lifetime);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public void AddRepository_Adds_NonDisposable_RepoType_With_Transient_Lifetime()
        {
            var type = typeof(FakeGenericRepository);

            ServiceCollectionExtensions.AddRepository(_services, type);

            var service = _services.FirstOrDefault(x => x.ImplementationType == type);
            Assert.Equal(ServiceLifetime.Transient, service?.Lifetime);
        }

        private class FakeRepo : FakeRepositoryBase, IDisposable
        {
            public void Dispose() { }
        }
    }
}
