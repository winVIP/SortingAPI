namespace SortingAPI.Interfaces
{
    public interface ISort<T>
    {
        public IList<T>? Sort(IList<T> values);
    }
}
