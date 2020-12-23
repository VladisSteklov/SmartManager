using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SmartfonManager.Source
{
    public static class FileFrameWork
    {
        public static readonly string[] Filter = { ".doc", ".pdf", ".mp", ".MOV", ".jpg", ".png", ".jpeg", ".bmp", ".txt", ".djvu",
            ".xls" , ".zip", ".rar" };

        private static readonly DirectoryInfo _curDirrectrory = new DirectoryInfo(".");

        public static string DirectoryToLoadFiles => _curDirrectrory.FullName + @"\Data";
        public static string DirectoryCache => _curDirrectrory.FullName + @"\Cache";


        public static void CreateDirrectroyIfNotExits(string dirPath)
        {
            // Создание директории для загрузки файлов с компьютера на телефон, если она не существует
            // В будущем можно брать из конфигурационных файлов
            
            var directories = _curDirrectrory.GetDirectories();
            var newDir = new DirectoryInfo(dirPath);
            bool flag = false; int i = 0;
            while (!flag && i < directories.Length)
            {
                if (directories[i].Name == newDir.Name) flag = true;
                else i++;
            }
            if (flag == false) newDir.Create();
            
        }
        public static bool ExistDirectory(string p)
        {
            // Проверка на существование директории
            if (string.IsNullOrEmpty(p))
                return false;
            if (new DirectoryInfo(p).Exists) return true;
            return false;
        }

        public static IEnumerable<string> GetEnumerateFiles(string path)
        {
            // Разбить строку файлов в массив строк
            path = path.Replace(Environment.NewLine, "\r");
            return path.Split('\r');
        }
        public static void RemoveCache()
        {
            var dir = new DirectoryInfo(DirectoryCache);
            foreach (var file in dir.GetFiles())
                file.Delete();
        }
    }
}
