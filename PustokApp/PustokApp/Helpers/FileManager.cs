using PustokApp.Models.Home;

namespace PustokApp.Helpers
{
    public static class FileManager
    {
        public static string SaveImage(this IFormFile file,string path,string folders)
        {
            string fileName = Guid.NewGuid() + file.FileName;
            string fullPath = Path.Combine(path,folders, fileName);
            using FileStream fileStream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(fileStream);
            return fileName;
        }
        public static bool CheckSize(this IFormFile file,int maxSize)
        {
            return file.Length <= maxSize;
        }
        public static bool CheckType(this IFormFile file, string[] types)
        {
            return types.Contains(file.ContentType);
        }
    }
}
