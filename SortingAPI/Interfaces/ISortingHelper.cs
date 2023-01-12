using SortingAPI.Enumerators;

namespace SortingAPI.Interfaces
{
    public interface ISortingHelper<T>
    {
        public IList<T>? Sort(IList<T> unsorted, SortingAlgorithm chosenSortingAlgorithm = SortingAlgorithm.BubbleSort);
    }
}
