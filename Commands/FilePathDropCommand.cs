using SmartfonManager.Source;
using SmartfonManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SmartfonManager.Commands
{
    public static class FilePathDropCommand
    {
        public static string Execute(DragEventArgs e)
        {

            // Файл вставляется в область контрола
            // Получение имен файлов с преобразованием, если возможно
            string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            if (fileNames == null) return null;

            string fieldNamesFileToLoad = null;
            foreach (string nameFile in fileNames)
            {
                //FileInfo file = new FileInfo(nameFile);
                // Если расширение файла подхрдит под фильтр
                //if (FileFrameWork.Filter.Any(file.Extension.Contains))
                fieldNamesFileToLoad += nameFile + Environment.NewLine;
            }
            // Удалить последний перенос строки
            fieldNamesFileToLoad = fieldNamesFileToLoad?.Remove(fieldNamesFileToLoad.Length - 2, 2);
            e.Handled = true;
            return fieldNamesFileToLoad;
        }
    }
}