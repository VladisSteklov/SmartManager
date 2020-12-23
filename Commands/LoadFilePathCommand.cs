using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartfonManager.ViewModel;

namespace SmartfonManager.Commands
{
    public class LoadFilePathCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            LoadToTelephoneViewModel vM = parameter as LoadToTelephoneViewModel;
            if (vM != null)
            {
                // Загрузить расположение файла в текстбокс
                var openDlgBox = new OpenFileDialog
                {
                    //Filter = "Файлы (*.bmp, *.jpg, *jpeg, *.png, *.txt *.doc, *.docx, *.pdf, *.mp3, *.mp4, *.MOV, *djvu, *xls," +
                    //"*zip, *rar) | *.bmp; *.jpg; *.jpeg; *.png; *.txt; *.doc; *.docx; *.pdf; *.mp3; *.mp4; *.MOV; *.djvu; *.xls; *.zip; *.rar",
                    Multiselect = true,
                    CheckPathExists = true,
                    InitialDirectory = @"C:\",
                    Title = "Загрузка файлов"
                };
                // Был ли совершен щелчок на кнопке ОК?
                if (true == openDlgBox.ShowDialog())
                {
                    var fileNames = openDlgBox.FileNames;
                    // Отчисить элемент TextBox
                    vM.FilePath = "";
                    // Вывод загруженных файлов в TextBox
                    foreach (var fileName in fileNames)
                    {
                        vM.FilePath += fileName + Environment.NewLine;
                    }
                    // Удалить последний переход на новую строку
                    vM.FilePath = vM.FilePath.Remove(vM.FilePath.Length - 2, 2);
                }
            }
        }
    }
}
