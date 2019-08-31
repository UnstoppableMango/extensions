using System;

namespace UnMango.Extensions.Repository
{
    internal static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
            => source?.IndexOf(toCheck, comp) >= 0;
    }
}
