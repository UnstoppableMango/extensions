using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KG.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace SimpleInjector
{
    /// <summary>
    ///     Helper methods for registering <see cref="KG.Data"/> types using the
    ///     <see cref="SimpleInjector"/> container.
    /// </summary>
    public static class RepositoryContainerExtensions
    {
        /// <summary>
        ///     <para>
        ///         Registers the <typeparamref name="T"/> as an <see cref="IRepository"/>.
        ///         If <typeparamref name="T"/> implements <see cref="IDisposable"/>, it will
        ///         be registered with <see cref="Lifestyle.Scoped"/>. Otherwise, it will be
        ///         registered with <see cref="Lifestyle.Transient"/>.
        ///     </para>
        /// </summary>
        /// <typeparam name="T"> The <see cref="IRepository"/> to register. </typeparam>
        /// <param name="container"> The <see cref="Container"/> to add the <see cref="IRepository"/> to. </param>
        public static void RegisterRepository<T>(this Container container)
            where T : class, IRepository
            => container.Register<IRepository, T>(GetLifestyle(typeof(T)));

        /// <summary>
        ///     <para>
        ///         Registers the <paramref name="repoType"/> as an <see cref="IRepository"/> if it implements
        ///         <see cref="IRepository"/>. If <paramref name="repoType"/> implements <see cref="IDisposable"/>,
        ///         it will be registered with <see cref="Lifestyle.Scoped"/>. Otherwise, it will be
        ///         registered with <see cref="Lifestyle.Transient"/>.
        ///     </para>
        /// </summary>
        /// <param name="container"> The <see cref="Container"/> to add the <see cref="IRepository"/> to. </param>
        /// <param name="repoType"> The <see cref="IRepository"/> to register. </param>
        public static void RegisterRepository(this Container container, Type repoType)
        {
            var irepo = typeof(IRepository);
            if (!irepo.IsAssignableFrom(repoType)) return;
            container.Register(irepo, repoType, GetLifestyle(repoType));
        }

        /// <summary>
        ///     <para>
        ///         Registers all <see cref="IRepository"/>s in the specified assemblies.
        ///         If an <see cref="IRepository"/> implements <see cref="IDisposable"/>, it will
        ///         be registered with <see cref="Lifestyle.Scoped"/>. Otherwise, it will be
        ///         registered with <see cref="Lifestyle.Transient"/>.
        ///     </para>
        /// </summary>
        /// <param name="container"> The <see cref="Container"/> to add the <see cref="IRepository"/>s to. </param>
        /// <param name="assemblies"> <see cref="Assembly"/>s to look for types in. </param>
        public static void RegisterRepositories(this Container container, IEnumerable<Assembly> assemblies)
        {
            var repoTypes = container.GetTypesToRegister(typeof(IRepository), assemblies);
            foreach (var type in repoTypes)
                container.Register(typeof(IRepository<>), type, GetLifestyle(type));
        }

        /// <summary>
        ///     Registers all implementations of <see cref="IRepository"/> in the specified assemblies
        ///     as an <see cref="IEnumerable{IRepository}"/>.
        /// </summary>
        /// <param name="container"> The <see cref="Container"/> to add the <see cref="IRepository"/>s to. </param>
        /// <param name="assemblies"> <see cref="Assembly"/>s to look for types in. </param>
        public static void RegisterRepositoryCollection(this Container container, IEnumerable<Assembly> assemblies)
        {
            var repoTypes = container.GetTypesToRegister(typeof(IRepository), assemblies);
            var repoRegistrations = repoTypes.Select(x => GetLifestyle(x).CreateRegistration(x, container));
            container.Collection.Register<IRepository>(repoRegistrations);
        }

        /// <summary>
        ///     Registers all implementations of <see cref="IRepository"/> in the specified assemblies
        ///     as an <see cref="IEnumerable{IRepository}"/> with the specified <see cref="Lifestyle"/>.
        /// </summary>
        /// <param name="container"> The <see cref="Container"/> to add the <see cref="IRepository"/>s to. </param>
        /// <param name="assemblies"> <see cref="Assembly"/>s to look for types in. </param>
        /// <param name="lifestyle">
        ///     The <see cref="Lifestyle"/> to use for the registered <see cref="IRepository"/>s.
        /// </param>
        public static void RegisterRepositoryCollection(
            this Container container,
            IEnumerable<Assembly> assemblies,
            Lifestyle lifestyle)
        {
            var repoTypes = container.GetTypesToRegister(typeof(IRepository), assemblies);
            var repoRegistrations = repoTypes.Select(x => lifestyle.CreateRegistration(x, container));
            container.Collection.Register<IRepository>(repoRegistrations);
        }

        /// <summary>
        ///     Registers the <typeparamref name="T"/> as an <see cref="IUnitOfWork"/> with
        ///     <see cref="Lifestyle.Scoped"/>. 
        /// </summary>
        /// <typeparam name="T"> The <see cref="IUnitOfWork"/> to register. </typeparam>
        /// <param name="container"> The <see cref="Container"/> to add the <typeparamref name="T"/> to. </param>
        /// <param name="injectionType"> How to inject <see cref="IRepository"/> instances. </param>
        public static void RegisterUnitOfWork<T>(
            this Container container,
            InjectionType injectionType = InjectionType.Constructor)
            where T : class, IUnitOfWork
            => RegisterUnitOfWork<T>(container, new[] {typeof(T).Assembly}, injectionType);

        /// <summary>
        ///     Registers the <typeparamref name="T"/> as an <see cref="IUnitOfWork"/> with
        ///     <see cref="Lifestyle.Scoped"/>. 
        /// </summary>
        /// <typeparam name="T"> The <see cref="IUnitOfWork"/> to register. </typeparam>
        /// <param name="container"> The <see cref="Container"/> to add the <typeparamref name="T"/> to. </param>
        /// <param name="repositoryAssemblies"> Assemblies containing repositories to register. </param>
        /// <param name="injectionType"> How to inject <see cref="IRepository"/> instances. </param>
        public static void RegisterUnitOfWork<T>(
            this Container container,
            IEnumerable<Assembly> repositoryAssemblies,
            InjectionType injectionType = InjectionType.Constructor)
            where T : class, IUnitOfWork
        {
            container.Register<IUnitOfWork, T>(Lifestyle.Scoped);

            if (!container.GetCurrentRegistrations().Any(x => RepoUtility.ImplementsIRepository(x.ServiceType)))
                RegisterRepositoryCollection(container, repositoryAssemblies);

            if (injectionType != InjectionType.Property) return;

            container.RegisterInitializer<IUnitOfWork>(x => InjectRepositoryProperties(container, x));
        }

        private static Lifestyle GetLifestyle(Type repositoryType)
            => typeof(IDisposable).IsAssignableFrom(repositoryType)
                ? Lifestyle.Scoped
                : Lifestyle.Transient;

        private static void InjectRepositoryProperties(Container container, IUnitOfWork unitOfWork)
        {
            var repoProperties = unitOfWork
                .GetType()
                .GetProperties()
                .Where(prop => RepoUtility.ImplementsIRepository(prop.PropertyType));

            foreach (var prop in repoProperties)
            {
                try
                {
                    var instance = container.GetInstance(prop.PropertyType);
                    prop.SetValue(unitOfWork, instance);
                }
                catch (ActivationException e)
                {
                    var message = $"{prop.PropertyType} has not been registered in the container and could not be injected";
                    throw new InvalidOperationException(message, e);
                }
                catch (Exception e)
                {
                    // Throw on any other failures
                    throw new Exception("Failed to inject repository properties", e);
                }
            }
        }
    }
}
