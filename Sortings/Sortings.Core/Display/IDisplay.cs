namespace Sortings.Core.Display;

public interface IDisplay<in T>
{
    string Display(T[] array);
}