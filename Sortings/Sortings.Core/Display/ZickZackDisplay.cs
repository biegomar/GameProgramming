namespace Sortings.Core.Display;

/// <summary>
/// The zick-zack display.
/// </summary>
/// <typeparam name="T">The type.</typeparam>
public class ZickZackDisplay<T> : IDisplay<T>
{
    /// <inheritdoc/>
    public string Display(T[] array)
    {
        return this.RecursiveDisplay(array);
    }

    /// <summary>
    /// The recursive approach.
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    private string RecursiveDisplay(T[] array)
    {
        var result = string.Empty;
        
        if (array.Length > 2)
        {
            result = this.RecursiveDisplay(array[1..^1]);
        }

        if (array.Length == 1)
        {
            return array[0]!.ToString()!;
        }

        var arr0 = array[0]!.ToString();
        var arrL = array[^1]!.ToString();
        
        result = arrL + "," + arr0 + (result == string.Empty ? string.Empty : $",{result}"); 
        return  result;
    }
}