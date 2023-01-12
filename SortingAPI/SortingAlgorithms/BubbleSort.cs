using SortingAPI.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace SortingAPI.SortingAlgorithms
{
    public class BubbleSort<T> : ISort<T>
    {
        public IList<T>? Sort(IList<T> unsorted) 
        {
            if(typeof(T) == typeof(int)) return (IList<T>)SortInt((IList<int>)unsorted);
            return null;
        }
        private IList<int> SortInt(IList<int> unsorted)
        {
            for (int j = 0; j <= unsorted.Count - 2; j++)
            {
                for (int i = 0; i <= unsorted.Count - 2; i++)
                {
                    if (unsorted[i] > unsorted[i + 1])
                    {
                        var temp = unsorted[i + 1];
                        unsorted[i + 1] = unsorted[i];
                        unsorted[i] = temp;
                    }
                }
            }

            return unsorted;
        }
    }
}
