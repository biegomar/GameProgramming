namespace Sortings.Core.Display;

/// <summary>
/// The ascending display.
/// </summary>
/// <typeparam name="T">The type.</typeparam>
public class AscendingDisplay<T> : IDisplay<T>
{
    /// <inheritdoc/>
    public string Display(T[] array)
    {
        return string.Join(',', array);
    }
}