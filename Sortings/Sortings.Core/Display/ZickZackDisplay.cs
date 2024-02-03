namespace Sortings.Core.Display;

public class ZickZackDisplay<T> : IDisplay<T>
{
    public string Display(T[] array)
    {
        return this.RecursiveDisplay(array);
    }

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