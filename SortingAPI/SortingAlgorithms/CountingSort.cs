using SortingAPI.Interfaces;

namespace SortingAPI.SortingAlgorithms
{
    public class CountingSort<T> : ISort<T>
    {
        public IList<T>? Sort(IList<T> unsorted)
        {
            if (typeof(T) == typeof(int)) return (IList<T>)SortInt((IList<int>)unsorted);
            return null;
        }
        private IList<int> SortInt(IList<int> unsorted)
        {
            int max = 0;

            for (int i = 0; i < unsorted.Count; i++)
            {
                if (max < unsorted[i])
                {
                    max = unsorted[i];
                }
            }

            int[] freq = new int[max + 1];
            for (int i = 0; i < max + 1; i++)
            {
                freq[i] = 0;
            }
            for (int i = 0; i < unsorted.Count; i++)
            {
                freq[unsorted[i]]++;
            }

            for (int i = 0, j = 0; i <= max; i++)
            {
                while (freq[i] > 0)
                {
                    unsorted[j] = i;
                    j++;
                    freq[i]--;
                }
            }

            return unsorted;
        }
    }
}
