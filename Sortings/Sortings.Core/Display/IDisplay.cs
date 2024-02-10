namespace Sortings.Core.Display;

/// <summary>
/// The display interface.
/// </summary>
/// <typeparam name="T">The type.</typeparam>
public interface IDisplay<in T>
{
    /// <summary>
    /// The display method.
    /// </summary>
    /// <param name="array">The array to display.</param>
    /// <returns>The resulting string.</returns>
    string Display(T[] array);
}