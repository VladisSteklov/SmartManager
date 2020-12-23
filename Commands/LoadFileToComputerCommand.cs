using SmartfonManager.Data;
using SmartfonManager.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SmartfonManager.Source;

namespace SmartfonManager.Commands
{
    public class LoadFileToComputerCommand
    {
        private readonly MyMediaDevice _myMediaDevice;
        public string PathToLoad { get; set; }
        public LoadFileToComputerCommand() { }
        public LoadFileToComputerCommand(MyMediaDevice device)
        {
            _myMediaDevice = device;
        }
        async public void ExecuteAsync(object parameter)
        {
            await Task.Run(() =>
            {
                // Загрузка медифайла
                MyMediaFile file = parameter as MyMediaFile;
               
                if (file == null || !_myMediaDevice.Device.FileExists(file.FilePath))
                {
                    MessageBox.Show("Не удается выполнить операцию.\r\nВозможно указанный файл не существует.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Если отсутствует директория для загрузки, создать ее
                if (!Directory.Exists(PathToLoad))
                    FileFrameWork.CreateDirrectroyIfNotExits(PathToLoad);

                // Скачать файл в область загрузки PathToLoad
                using (FileStream sourceStream = File.Create(PathToLoad + @"\\" + file.FileName + file.ExtensionString))
                {
                    _myMediaDevice.Device.DownloadFile(file.FilePath, sourceStream);
                }
                MessageBox.Show($"Файл {file.FileName} был установлен в папку {PathToLoad}", "Информация", MessageBoxButton.OK,
                    MessageBoxImage.Information);
               
            });
        }
    }
}
