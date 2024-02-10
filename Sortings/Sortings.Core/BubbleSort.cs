namespace Sortings.Core;

/// <summary>
/// The bubble sort implementation
/// </summary>
public class BubbleSort : ISort
{
    /// <inheritdoc/>
    public ObservableArray<int> Sort(ObservableArray<int> input)
    {
        var bubbleArray = input.Clone();

        bool swap = true;
        int corrector = 1;
        
        while (swap)
        {
            swap = false;
            for (int i = 0; i < bubbleArray.Length - corrector; i++)
            {
                if (bubbleArray.GreaterThan(i, i +1))
                {
                    bubbleArray.Swap(i, i + 1);
                    swap = true;
                }
            }

            corrector++;
        }

        return bubbleArray;
    }
}