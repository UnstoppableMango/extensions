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
            Exception? e = null;

            if (value is null)
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException($"{parameterName} is an empty string");
            }

            if (e == null) return value!;

            NotEmpty(parameterName, nameof(parameterName));

            throw e;
        }
    }
}
