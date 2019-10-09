using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UnMango.Extensions.Specification
{
    public abstract class Specification<T> : ISpecification<T>
    {
        private readonly List<Expression<Func<T, object>>> _includes;
        private readonly List<string> _includeStrings;

        protected Specification(Expression<Func<T, bool>> criteria, bool readOnly = default)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
            _includes = new List<Expression<Func<T, object>>>();
            _includeStrings = new List<string>();
            ReadOnly = readOnly;
        }

        protected Specification(ISpecification<T> specification, bool readOnly = default)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            Criteria = specification.Criteria;
            _includes = specification.Includes.ToList();
            _includeStrings = specification.IncludeStrings.ToList();
            ReadOnly = readOnly;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public IReadOnlyCollection<Expression<Func<T, object>>> Includes => _includes;

        public IReadOnlyCollection<string> IncludeStrings => _includeStrings;

        public bool ReadOnly { get; }

        protected virtual void Include(Expression<Func<T, object>> include)
        {
            if (include == null)
            {
                throw new ArgumentNullException(nameof(include));
            }

            _includes.Add(include);
        }

        protected virtual void Include(string include)
        {
            if (include == null)
            {
                throw new ArgumentNullException(nameof(include));
            }

            _includeStrings.Add(include);
        }
    }
}
