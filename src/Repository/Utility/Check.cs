using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace UnMango.Extensions.Repository
{
    [DebuggerStepThrough]
    internal static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName] [NotNull] string paramterName)
        {
#pragma warning disable IDE0041 // Use 'is null' check
            if (!ReferenceEquals(value, null)) return value;
#pragma warning restore IDE0041 // Use 'is null' check

            NotEmpty(paramterName, nameof(paramterName));
                
            throw new ArgumentNullException(paramterName);
        }

        [UsedImplicitly]
        [ContractAnnotation("value:null => halt")]
        public static string NotEmpty(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
#if NETCOREAPP3_0 || NETSTANDARD2_0
            Exception? e = null;
#else
            Exception e = null;
#endif

            if (value is null)
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException($"{parameterName} is an empty string");
            }

#if NETCOREAPP3_0 || NETSTANDARD2_0
            if (e == null) return value!;
#else
            if (e == null) return value;
#endif

            NotEmpty(parameterName, nameof(parameterName));

            throw e;
        }
    }
}
