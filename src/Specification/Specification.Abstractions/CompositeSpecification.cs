using System;
using System.Linq;
using System.Linq.Expressions;

namespace UnMango.Extensions.Specification
{
    public static class CompositeSpecification
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right, bool readOnly = default)
        {
            return new CompositeSpecification<T>(left, right, (l, r) => l && r, readOnly);
        }

        public static ISpecification<T> AndNot<T>(this ISpecification<T> left, ISpecification<T> right, bool readOnly = default)
        {
            return new CompositeSpecification<T>(left, right, (l, r) => l && !r, readOnly);
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right, bool readOnly = default)
        {
            return new CompositeSpecification<T>(left, right, (l, r) => l || r, readOnly);
        }

        public static ISpecification<T> OrNot<T>(this ISpecification<T> left, ISpecification<T> right, bool readOnly = default)
        {
            return new CompositeSpecification<T>(left, right, (l, r) => l || !r, readOnly);
        }
    }

    internal class CompositeSpecification<T> : Specification<T>
    {
        public CompositeSpecification(
            ISpecification<T> left,
            ISpecification<T> right,
            Func<bool, bool, bool> op,
            bool readOnly = default)
            : base(GetCriteria(left, right, op), readOnly)
        {
            var includes = Enumerable.Concat(left.Includes, right.Includes);
            foreach (var include in includes)
                Include(include);

            var includeStrings = Enumerable.Concat(left.IncludeStrings, right.IncludeStrings);
            foreach (var include in includeStrings)
                Include(include);
        }

        private static Expression<Func<T, bool>> GetCriteria(
            ISpecification<T> left,
            ISpecification<T> right,
            Func<bool, bool, bool> op)
        {
            var leftCriteria = left.Criteria.Compile();
            var rightCriteria = right.Criteria.Compile();

            return obj => op(leftCriteria(obj), rightCriteria(obj));
        }
    }
}
