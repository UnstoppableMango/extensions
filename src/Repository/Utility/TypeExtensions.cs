using System;

namespace UnMango.Extensions.Repository
{
    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    internal static class TypeExtensions
    {
        public static bool IsConcrete(this Type type) => type.IsClass && !type.IsAbstract;
    }
}
