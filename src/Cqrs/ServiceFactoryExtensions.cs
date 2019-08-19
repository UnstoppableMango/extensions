using System;

namespace KG.DCX.Extensions.Cqrs
{
    internal static class ServiceFactoryExtensions
    {
        public static T GetService<T>(this ServiceFactory serviceFactory)
        {
            if (serviceFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceFactory));
            }

            return (T)serviceFactory(typeof(T));
        }

        public static T GetRequiredService<T>(this ServiceFactory serviceFactory)
        {
            var service = serviceFactory.GetService<T>();

            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {typeof(T)} was unable to be resolved");
            }

            return service;
        }

        public static object GetRequiredService(this ServiceFactory serviceFactory, Type type)
        {
            var service = serviceFactory(type);

            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {type} was unable to be resolved");
            }

            return service;
        }
    }
}
