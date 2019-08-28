using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace KG.Data.Extensions.DependencyInjection
{
    public class ServiceCollectionUnitOfWorkExtensionsTest
    {
        private readonly Mock<IServiceCollection> _services = new Mock<IServiceCollection>();

        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();

        [Fact]
        [Trait("Category", "Unit")]
        public void AddUnitOfWork_Returns_The_Original_ServiceCollection()
        {
            var type = _unitOfWork.Object.GetType();

            var services = ServiceCollectionExtensions.AddUnitOfWork(_services.Object, type);

            Assert.Same(_services.Object, services);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AddUnitOfWork_Throws_When_Type_Is_Not_Concrete()
        {
            var type = typeof(IUnitOfWork);

            Assert.Throws<ArgumentException>(() =>
                ServiceCollectionExtensions.AddUnitOfWork(_services.Object, type));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AddUnitOfWork_Throws_When_Type_Is_Not_IUnitOfWork()
        {
            var type = typeof(object);

            Assert.Throws<ArgumentException>(() =>
                ServiceCollectionExtensions.AddUnitOfWork(_services.Object, type));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AddUnitOfWork_Adds_Type_As_Itself()
        {
            var type = _unitOfWork.Object.GetType();

            ServiceCollectionExtensions.AddUnitOfWork(_services.Object, type);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.ServiceType == type &&
                d.ImplementationType == type)));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AddUnitOfWork_Adds_Type_As_IUnitOfWork()
        {
            var type = _unitOfWork.Object.GetType();

            ServiceCollectionExtensions.AddUnitOfWork(_services.Object, type);

            _services.Verify(x => x.Add(It.Is<ServiceDescriptor>(d =>
                d.ServiceType == typeof(IUnitOfWork))));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void AddUnitOfWork_Uses_Scoped_Lifetime()
        {
            var type = _unitOfWork.Object.GetType();

            ServiceCollectionExtensions.AddUnitOfWork(_services.Object, type);

            _services.Verify(
                x => x.Add(It.Is<ServiceDescriptor>(d =>
                    d.Lifetime == ServiceLifetime.Scoped)
                ),
                Times.Exactly(2));
        }
    }
}
