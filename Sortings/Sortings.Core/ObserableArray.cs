namespace Sortings.Core
{
    public class ObservableArray<T>
    {
        private T[] array;

        public ObservableArray(int size)
        {
            array = new T[size];
        }

        public ObservableArray(T[] array)
        {
            this.array = array;
        }

        public T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public ObservableArray<T> this[Range range]
        {
            get
            {
                var slice = array[range];
                var observableSlice = new ObservableArray<T>(slice.Length);
                for (int i = 0; i < slice.Length; i++)
                {
                    observableSlice[i] = slice[i];
                }
                    
                return observableSlice;
            }
        }

        public void Swap(int sourceIndex, int destinationIndex)
        {
            (array[sourceIndex], array[destinationIndex]) = (array[destinationIndex], array[sourceIndex]);
            Swapped?.Invoke(this, new SwapEventArgs() {FirstIndex = sourceIndex, SecondIndex = destinationIndex, Color = ConsoleColor.Red});
        }

        public bool GreaterThan(int sourceIndex, int destinationIndex)
        {
            var result = Comparer<T>.Default.Compare(array[sourceIndex], array[destinationIndex]) > 0;
            Swapped?.Invoke(this, new SwapEventArgs() {FirstIndex = sourceIndex, SecondIndex = destinationIndex, Color = ConsoleColor.Green});
            
            return result;
        }

        public bool LessThan(int sourceIndex, int destinationIndex)
        {
            var result =  Comparer<T>.Default.Compare(array[sourceIndex], array[destinationIndex]) < 0;
            Swapped?.Invoke(this, new SwapEventArgs() {FirstIndex = sourceIndex, SecondIndex = destinationIndex, Color = ConsoleColor.Green});
            
            return result;
        }

        public int Length => array.Length;

        public event EventHandler<SwapEventArgs>? Swapped;
        
        public ObservableArray<T> Clone()
        {
            T[] arrayClone = (T[])array.Clone();

            var result = new ObservableArray<T>(arrayClone);
            result.Swapped = this.Swapped;

            return result;
        }

        public override string ToString()
        {
            return string.Join(", ", array);
        }

        public T[] ToArray()
        {
            return this.array;
        }
    }
}