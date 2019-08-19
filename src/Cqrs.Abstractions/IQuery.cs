// ReSharper disable UnusedTypeParameter
namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Represents a query that returns <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type returned by this query.</typeparam>
    public interface IQuery<out T>
    {
    }
}
