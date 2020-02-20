// ReSharper disable UnusedTypeParameter
namespace KG.DCX.Extensions.Cqrs
{
    /// <summary>
    /// Represents a command with no return value.
    /// </summary>
    public interface ICommand
    {
    }

    /// <summary>
    /// Represents a command with a return value.
    /// </summary>
    public interface ICommand<out T>
    {
    }
}
