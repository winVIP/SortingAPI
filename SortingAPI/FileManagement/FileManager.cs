using Newtonsoft.Json;
using SortingAPI.Interfaces;

namespace SortingAPI.FileManagement
{
    public class FileManager : IFileManager
    {
        public void SaveStringToFile(string path, string text)
        {
            if (File.Exists(path))
            {
                for (int i = 1; i < int.MaxValue; i++)
                {
                    string newPath = Path.GetDirectoryName(path)
                        + "\\" + Path.GetFileNameWithoutExtension(path)
                        + $"({i})"
                        + Path.GetExtension(path);

                    bool fileExists = File.Exists(newPath);

                    if (!fileExists)
                    {
                        File.WriteAllText(newPath, text);
                        break;
                    }
                }
            }
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, text);
        }

        public string GetFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else if(Directory.Exists(path))
            {
                var directoryInfo = new DirectoryInfo(path);
                var files = directoryInfo.GetFiles();
                if(files.Length > 0)
                {
                    return File.ReadAllText(files.OrderByDescending(x => x.CreationTime).First().FullName);
                }
            }
            return "";
        }
    }
}
