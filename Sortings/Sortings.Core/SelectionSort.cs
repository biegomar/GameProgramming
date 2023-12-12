using System.Diagnostics;

namespace Sortings.Core;

public class SelectionSort : ISort
{
    public ObservableArray<int> Sort(ObservableArray<int> input)
    {
        var selectionArray = input.Clone();
        for (int i = 0; i < selectionArray.Length-1; i++)
        {
            var min = i;
            for (int j = i+1; j < selectionArray.Length; j++)
            {
                if (selectionArray.LessThan(j, min))
                {
                    min = j;
                }
            }
            selectionArray.Swap(min, i);
        }

        return selectionArray;
    }
}