using JetBrains.Annotations;
using KG.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Xunit;

namespace KG.Data.Extensions.SimpleInjector
{
    public class ContainerExtensionsTest
    {
        //[Fact]
        [Trait("Category", "Unit")]
        public void Injects_Repository_Properties()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register<FakeContext>();
            container.RegisterUnitOfWork<Fake>(InjectionType.Property);

            container.Verify();
            Assert.NotNull(container.GetInstance<IUnitOfWork>());
        }

        private class Fake : UnitOfWorkBase
        {
            public IRepository<object> Objects { get; set; }

            public IRepository<string> Strings { get; set; }
        }

        private class ObjectRepo : Repository<object>
        {
            public ObjectRepo([NotNull] FakeContext context) : base(context)
            {
            }
        }

        private class StringRepo : Repository<string>
        {
            public StringRepo([NotNull] FakeContext context) : base(context)
            {
            }
        }

        private class FakeContext : DbContext { }
    }
}
