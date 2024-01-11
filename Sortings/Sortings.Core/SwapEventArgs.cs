namespace Sortings.Core;

public class SwapEventArgs : EventArgs
{
    public int FirstIndex { get; set; }
    public int SecondIndex { get; set; }

    public ConsoleColor Color { get; set; }
}