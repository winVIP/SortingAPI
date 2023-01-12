using SortingAPI.Enumerators;
using SortingAPI.FileManagement;
using SortingAPI.Interfaces;
using SortingAPI.SortingAlgorithms;
using System.Diagnostics;

namespace SortingAPI.Helpers
{
    public class SortingHelper<T> : ISortingHelper<T>
    {
        public IList<T>? Sort(IList<T> unsorted, SortingAlgorithm chosenSortingAlgorithm = SortingAlgorithm.BubbleSort)
        {
            ISort<T> sortingAlgorithm;
            switch (chosenSortingAlgorithm)
            {                
                case SortingAlgorithm.BubbleSort:
                    sortingAlgorithm = new BubbleSort<T>();
                    break;
                case SortingAlgorithm.InsertionSort:
                    sortingAlgorithm = new CountingSort<T>();
                    break;
                case SortingAlgorithm.CountingSort:
                    sortingAlgorithm = new InsertionSort<T>();
                    break;
                default: 
                    return null;
            }

            #if DEBUG
            var watch = new Stopwatch();
            watch.Start();
            #endif

            var sorted = sortingAlgorithm.Sort(unsorted);

            #if DEBUG
            watch.Stop();
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\Logs");
            File.AppendAllLines(
                Environment.CurrentDirectory + "\\Logs\\Log.txt",
                new string[]
                {
                    "---------------------------------------------",
                    string.Format(
                        "Sorting algorithm: {0}\nDataset: {1}\nResult: {2}\nDuration: {3} ticks",
                        Enum.GetName(typeof(SortingAlgorithm), chosenSortingAlgorithm),
                        string.Join(",", unsorted),
                        sorted == null ? "" : string.Join(",", sorted),
                        watch.Elapsed.Ticks
                    ),
                    "---------------------------------------------"
                }
            );
            #endif
            return sorted;
        }
    }
}
