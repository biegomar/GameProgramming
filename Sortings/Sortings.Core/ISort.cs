namespace Sortings.Core;

/// <summary>
/// The sort interface.
/// </summary>
public interface ISort
{
    /// <summary>
    /// The sort methode.
    /// </summary>
    /// <param name="input">The array to sort.</param>
    /// <returns>The sorted array.</returns>
    ObservableArray<int> Sort(ObservableArray<int> input);
}