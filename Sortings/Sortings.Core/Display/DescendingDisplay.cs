namespace Sortings.Core.Display;

/// <summary>
/// The descending display.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DescendingDisplay<T> : IDisplay<T>
{
    /// <inheritdoc/>
    public string Display(T[] array)
    {
        return string.Join(',', array.Reverse());
    }
}