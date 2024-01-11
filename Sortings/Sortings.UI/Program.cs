using Sortings.Core;
using Sortings.Core.Display;

//ObservableArray<int> array = new ObservableArray<int>(new []{ 5, 6, 4, 1, 8, 3, 2, 9, 7 });
//ObservableArray<int> array = new ObservableArray<int>(new []{ 16, 10, 5, 11, 6, 4, 18, 17, 1, 8, 19, 3, 14, 2, 9, 15, 7, 12, 20, 13 });

//array.Swapped += PlotArray;


Console.WriteLine("Willkommen zum Beispiel für Sortieralgorithmen.");
ConsoleKeyInfo key;
do
{
    Console.Write("Möchtest Du die Zahlen zufällig erzeugen lassen (J/N)?");
    key = Console.ReadKey();
    Console.WriteLine();
} while ( !(key.Key is ConsoleKey.J or ConsoleKey.N or ConsoleKey.Q));

if (key.Key == ConsoleKey.Q)
{
    Console.WriteLine("Dann nicht...");
    return;
}

int countOfNumbers;
ObservableArray<int> array;

if (key.Key == ConsoleKey.J)
{
    do
    {
        Console.Write("Ok. Wie viele Zahlen sollen generiert werden?");     
    } while (!int.TryParse(Console.ReadLine(), out countOfNumbers));

    Random rnd = new Random();
    array = new ObservableArray<int>(Enumerable.Range(1, countOfNumbers).Select(_ => rnd.Next()).ToArray());    
}
else
{
    do
    {
        Console.Write("Ok. Wie viele Zahlen sollen eingegeben werden?");
    } while (!int.TryParse(Console.ReadLine(), out countOfNumbers));

    array = new ObservableArray<int>(countOfNumbers);
    
    for (int i = 0; i < countOfNumbers; i++)
    {
        int singleNumber;
        
        do
        {
            Console.Write($"Gib die {i+1}. Zahl ein: ");
        } while (!int.TryParse(Console.ReadLine(), out singleNumber));

        array[i] = singleNumber;
    }
}

DisplayArray(array, new AscendingDisplay<int>());

ConsoleKeyInfo sortAlgo;
do
{
    Console.Write("Welcher Algorithmus soll benutzt werden (bitte Anfangsbuchstaben eintippen)? Mergesort, Bubblesort, Selectionsort?");
    sortAlgo = Console.ReadKey();
    Console.WriteLine();
} while ( !(sortAlgo.Key is ConsoleKey.M or ConsoleKey.S or ConsoleKey.B));

var algo = sortAlgo.Key switch
{
    ConsoleKey.B => new SortDecorator(new BubbleSort()),
    ConsoleKey.S => new SortDecorator(new SelectionSort()),
    ConsoleKey.M => new SortDecorator(new MergeSort())
};

var sortedArray = algo.Sort(array);

ConsoleKeyInfo howToDisplay;
do
{
    Console.Write("Wie soll das sortierte Array angezeigt werden (bitte Anfangsbuchstaben eintippen)? Ascending, Descending, Zickzack?");
    howToDisplay = Console.ReadKey();
    Console.WriteLine();
} while ( !(howToDisplay.Key is ConsoleKey.A or ConsoleKey.D or ConsoleKey.Z));

IDisplay<int> displayStrategy = howToDisplay.Key switch
{
    ConsoleKey.A => new AscendingDisplay<int>(),
    ConsoleKey.D => new DescendingDisplay<int>(),
    ConsoleKey.Z => new ZickZackDisplay<int>()
};

DisplayArray(sortedArray, displayStrategy,false);

void DisplayArray(ObservableArray<int> input, IDisplay<int> displayStrategy, bool unsorted=true)
{
    if (input.Length <= 20)
    {
        var vorsilbe = unsorted ? "un" : string.Empty;
        Console.WriteLine($"Dies ist das {vorsilbe}sortierte Array: ");
        Console.WriteLine(displayStrategy.Display(input.ToArray()));
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
        
        Console.ReadKey();
    }
}