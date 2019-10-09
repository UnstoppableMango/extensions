using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnMango.Extensions.Specification
{
    public interface ISpecification
    {
        bool ReadOnly { get; }
    }

    public interface ISpecification<T> : ISpecification
    {
        Expression<Func<T, bool>> Criteria { get; }

        IReadOnlyCollection<Expression<Func<T, object>>> Includes { get; }

        IReadOnlyCollection<string> IncludeStrings { get; }
    }
}
