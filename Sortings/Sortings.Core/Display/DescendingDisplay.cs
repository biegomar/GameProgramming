namespace Sortings.Core.Display;

public class DescendingDisplay<T> : IDisplay<T>
{
    public string Display(T[] array)
    {
        return string.Join(',', array.Reverse());
    }
}