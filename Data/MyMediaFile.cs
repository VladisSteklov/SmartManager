using System;
using System.Linq;
using System.Windows.Media;
using SmartfonManager.Commands;

namespace SmartfonManager.Data
{
    public enum TypeFile
    {
        TextFile,
        Music,
        Video,
        Photo,
        Others
    }
    public class MyMediaFile
    {
        public MyMediaFile() { }
        public MyMediaFile(string fileName, string filePath, TypeFile ext, DateTime dt, ulong size)
        {
            // Для других файлов раширение сохраняется в имени
            if (fileName.Contains('.'))
            {
                ExtensionString = fileName.Remove(0, fileName.LastIndexOf('.'));
                FileName = ext == TypeFile.Others ? fileName : fileName.Remove(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
            }
            else
            {
                FileName = fileName;
                ExtensionString = "";
            }
            FilePath = filePath;
            Extension = ext;
            LastChangeDate = dt;
            FileSize = size;

            // Обрезка FileName
            if (FileName.Length > 30)
            {
                FileName = FileName.Remove(30, FileName.Length - 30);
            }
        }
 
        public TypeFile Extension { get; private set; }
        public string ExtensionString { get; private set; }
        public string FileName { get; private set; }
        
        public string FilePath { get; private set; }
        public DateTime LastChangeDate { get; private set; }
        public ulong FileSize { get; private set; }
        public ImageSource Image { get; set; }

    }
}
