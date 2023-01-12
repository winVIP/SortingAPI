namespace SortingAPI.Interfaces
{
    public interface IFileManager
    {
        public void SaveStringToFile(string path, string text);
        public string GetFile(string path);
    }
}
