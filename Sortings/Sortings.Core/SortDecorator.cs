using System.Diagnostics;

namespace Sortings.Core;

public class SortDecorator : ISort
{
    private readonly ISort decoratedSorter;
    
    public SortDecorator(ISort decoratedSorter)
    {
        this.decoratedSorter = decoratedSorter;
    }
    
    /// <inheritdoc/>
    public ObservableArray<int> Sort(ObservableArray<int> input)
    {
        input.Swap(0,0);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        var result = this.decoratedSorter.Sort(input);

        stopwatch.Stop();
        Console.WriteLine($"{decoratedSorter.GetType().Name}: {stopwatch.Elapsed} Sekunden.");
        
        return result;
    }
}