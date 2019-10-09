using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UnMango.Extensions.Specification;

namespace UnMango.Extensions.Repository
{
    public static class DbContextSpecificationExtensions
    {
        public static IQueryable<T> Apply<T>(this DbContext context, ISpecification<T> specification)
            where T : class
        {
            return context.Set<T>().Apply(specification);
        }

        public static IQueryable<T> Apply<T>(this IQueryable<T> set, ISpecification<T> specification)
            where T : class
        {
            if (specification.ReadOnly)
                set = set.AsNoTracking();

            return set
                .Apply(specification.Includes)
                .Apply(specification.IncludeStrings)
                .Where(specification.Criteria);
        }

        private static IQueryable<T> Apply<T>(this IQueryable<T> set, IEnumerable<Expression<Func<T, object>>> includes)
            where T : class
        {
            return includes.Aggregate(set, (current, include) => current.Include(include));
        }

        private static IQueryable<T> Apply<T>(this IQueryable<T> set, IEnumerable<string> includes)
            where T : class
        {
            return includes.Aggregate(set, (current, include) => current.Include(include));
        }
    }
}
