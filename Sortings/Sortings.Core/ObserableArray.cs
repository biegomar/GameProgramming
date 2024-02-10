namespace Sortings.Core
{
    /// <summary>
    /// The observable array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableArray<T>
    {
        private T[] array;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public ObservableArray(int size)
        {
            array = new T[size];
        }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="array">The original array to wrap.</param>
        public ObservableArray(T[] array)
        {
            this.array = array;
        }

        /// <summary>
        /// An indexer.
        /// </summary>
        /// <param name="index">The index of the item to receive.</param>
        public T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        /// <summary>
        /// A range indexer.
        /// </summary>
        /// <param name="range">The range of the items to receive.</param>
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

        /// <summary>
        /// Swap the two items.
        /// </summary>
        /// <param name="sourceIndex">First item to swap.</param>
        /// <param name="destinationIndex">Second item to swap.</param>
        public void Swap(int sourceIndex, int destinationIndex)
        {
            (array[sourceIndex], array[destinationIndex]) = (array[destinationIndex], array[sourceIndex]);
            Swapped?.Invoke(this, new SwapEventArgs() {FirstIndex = sourceIndex, SecondIndex = destinationIndex, Color = ConsoleColor.Red});
        }

        /// <summary>
        /// The implementation of >
        /// </summary>
        /// <param name="sourceIndex">The source index.</param>
        /// <param name="destinationIndex">The destination index.</param>
        /// <returns>True if sourceIndex is greater, otherwise false.</returns>
        public bool GreaterThan(int sourceIndex, int destinationIndex)
        {
            var result = Comparer<T>.Default.Compare(array[sourceIndex], array[destinationIndex]) > 0;
            Swapped?.Invoke(this, new SwapEventArgs() {FirstIndex = sourceIndex, SecondIndex = destinationIndex, Color = ConsoleColor.Green});
            
            return result;
        }

        /// <summary>
        /// The implementation of <
        /// </summary>
        /// <param name="sourceIndex">The source index.</param>
        /// <param name="destinationIndex">The destination index.</param>
        /// <returns>True if sourceIndex is less, otherwise false.</returns>
        public bool LessThan(int sourceIndex, int destinationIndex)
        {
            var result =  Comparer<T>.Default.Compare(array[sourceIndex], array[destinationIndex]) < 0;
            Swapped?.Invoke(this, new SwapEventArgs() {FirstIndex = sourceIndex, SecondIndex = destinationIndex, Color = ConsoleColor.Green});
            
            return result;
        }

        /// <summary>
        /// The length of the array.
        /// </summary>
        public int Length => array.Length;

        /// <summary>
        /// The event handler, that is fired if items are swapped.
        /// </summary>
        public event EventHandler<SwapEventArgs>? Swapped;
        
        /// <summary>
        /// A clone function.
        /// </summary>
        /// <returns>A clone of this.</returns>
        public ObservableArray<T> Clone()
        {
            T[] arrayClone = (T[])array.Clone();

            var result = new ObservableArray<T>(arrayClone);
            result.Swapped = this.Swapped;

            return result;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Join(", ", array);
        }

        /// <summary>
        /// Returns the pure array of the structure.
        /// </summary>
        /// <returns>The array.</returns>
        public T[] ToArray()
        {
            return this.array;
        }
    }
}