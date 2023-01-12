using SortingAPI.Interfaces;

namespace SortingAPI.SortingAlgorithms
{
    public class InsertionSort<T> : ISort<T>
    {
        public IList<T>? Sort(IList<T> unsorted)
        {
            if (typeof(T) == typeof(int)) return (IList<T>)SortInt((IList<int>)unsorted);
            return null;
        }
        private IList<int> SortInt(IList<int> unsorted)
        {
            for (int i = 1; i < unsorted.Count; i++)
            {
                var key = unsorted[i];
                var flag = 0;
                for (int j = i - 1; j >= 0 && flag != 1;)
                {
                    if (key < unsorted[j])
                    {
                        unsorted[j + 1] = unsorted[j];
                        j--;
                        unsorted[j + 1] = key;
                    }
                    else flag = 1;
                }
            }
            return unsorted;
        }
    }
}
