namespace Sortings.Core.Display;

public class AscendingDisplay<T> : IDisplay<T>
{
    public string Display(T[] array)
    {
        return string.Join(',', array);
    }
}