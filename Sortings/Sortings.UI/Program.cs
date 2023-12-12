using Sortings.Core;


int x = 100000; 
Random rnd = new Random();
//ObservableArray<int> array = new ObservableArray<int>(Enumerable.Range(1, x).Select(_ => rnd.Next()).ToArray());
ObservableArray<int> array = new ObservableArray<int>(new []{ 5, 6, 4, 1, 8, 3, 2, 9, 7 });
//ObservableArray<int> array = new ObservableArray<int>(new []{ 16, 10, 5, 11, 6, 4, 18, 17, 1, 8, 19, 3, 14, 2, 9, 15, 7, 12, 20, 13 });

array.Swapped += PlotArray;

var bubble = new SortDecorator(new BubbleSort());
var selection = new SortDecorator(new SelectionSort());
var merge = new SortDecorator(new MergeSort());

//DisplayArray(array);

var bubbleArray = bubble.Sort(array);
//DisplayArray(bubbleArray);

var selectionArray = selection.Sort(array);
//DisplayArray(selectionArray);

var mergeArray = merge.Sort(array);
//DisplayArray(mergeArray);

void DisplayArray(ObservableArray<int> input)
{
    if (input.Length <= 20)
    {
        Console.WriteLine(string.Join(", ", input));    
    }
}

void PlotArray(object? sender, SwapEventArgs eventArgs)
{
    if (sender is ObservableArray<int> sortArray)
    {
        int left = 1;
        int top = 1;
        Console.Clear();
        Console.SetCursorPosition(left,top);
        for (int i = sortArray.Length - 1; i >= 0; i--)
        {
            Console.SetCursorPosition(left+sortArray[i], top + sortArray.Length - i);
            Console.Write(".");
            Console.SetCursorPosition(left + sortArray.Length + 2, top + sortArray.Length - i);
            if (eventArgs.FirstIndex == i || eventArgs.SecondIndex == i)
            {
                Console.ForegroundColor = eventArgs.Color;
            }
            
            Console.WriteLine(sortArray[i]);
            Console.ResetColor();
        } 
        
        
        //Thread.Sleep(1000);
        Console.ReadKey();
    }
}