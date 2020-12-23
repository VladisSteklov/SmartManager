using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartfonManager.ViewModel;
using SmartfonManager.Source;
using System.Windows;
using System.IO;

namespace SmartfonManager.Commands
{
    public class LoadFilesToPhoneCommand : BaseCommand
    {
        // Если _isWorking = true (команда работает), то блокируется ее выполнение
        private bool _isWorking = false;
        public override bool CanExecute(object parameter)
        {
            return _isWorking == false ? true : false;
        }

        public override void Execute(object parameter)
        {
            this.WorkCommandAsync(parameter);
        }

        async private void WorkCommandAsync(object parameter)
        {
            _isWorking = true;
            await Task.Run(() => ExecuteWork(parameter));
            _isWorking = false;
        }
        private void ExecuteWork(object parameter)
        {
            // Загрузить выбранные файлы в телефон
            LoadToTelephoneViewModel vM = parameter as LoadToTelephoneViewModel;
            if (vM == null) return;

            var dir = vM.Device.DirectoryToLoad + @"\SmartfonManager";
            if (!vM.Device.Device.DirectoryExists(dir))
            {
                // Если директория не существует, создать директорию
                vM.Device.Device.CreateDirectory(dir);
            }
            // Разделить название файлов от \r\n, если нет валидации и строка не "Путь к файлу"
            if (vM["FilePath"] == string.Empty && vM.FilePath != "Путь к файлу")
            {
                var files = FileFrameWork.GetEnumerateFiles(vM.FilePath);
                // Загрузить файлы на телефон в директорию dir
                foreach (var file in files)
                {
                    using (FileStream sourceStream = File.OpenRead(file))
                    {
                        // Получение имени файла путем удаление из него части строки до последних \\
                        var name = file.Remove(0, file.LastIndexOf('\\'));
                        vM.Device.Device.UploadFile(sourceStream, dir + name);
                    }
                }
                MessageBox.Show("Файлы были успешно установлены на Ваше устройство",
                    "Информация", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
            }
            else
            {
                var str = vM["FilePath"] == String.Empty ? "Файл не указан" : vM["FilePath"];
                MessageBox.Show(str,
                    "Ошибка", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
            }
        }
    }
}
