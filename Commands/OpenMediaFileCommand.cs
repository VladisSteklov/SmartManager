using SmartfonManager.Controls;
using SmartfonManager.Data;
using SmartfonManager.Services;
using SmartfonManager.Source;
using SmartfonManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartfonManager.Commands
{
    public class OpenMediaFileCommand
    {
        private readonly MyMediaDevice _myMediaDevice;
        private readonly ObservableCollection<MyMediaFile> _files;
        public OpenMediaFileCommand() { }
        public OpenMediaFileCommand(MyMediaDevice device, ObservableCollection<MyMediaFile> files)
        {
            _myMediaDevice = device;
            _files = files;
        }

        public void Execute(object parametr)
        {
        MyMediaFile file = parametr as MyMediaFile;
           
            if (file == null || !_myMediaDevice.Device.FileExists(file.FilePath))
            {
                MessageBox.Show("Не удается выполнить операцию.\r\nВозможно указанный файл не существует.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (file.Extension == TypeFile.Photo)
                ShowPhotos(file);
            else
                ShowContentMediaAsync(file);
        }

        async private void ShowContentMediaAsync(MyMediaFile file)
        {
            // Загрузка файла в Кеш-папку и запуск процесса
            await Task.Run(() =>
            {
            if (!Directory.Exists(FileFrameWork.DirectoryCache))
                FileFrameWork.CreateDirrectroyIfNotExits(FileFrameWork.DirectoryCache);
            var sourcePath = FileFrameWork.DirectoryCache + @"\\" + file.FileName + file.ExtensionString;

                using (FileStream sourceStream = File.Create(sourcePath))
                {
                    try
                    {
                        _myMediaDevice.Device.DownloadFile(file.FilePath, sourceStream);
                    }
                    catch
                    {
                        MessageBox.Show("Телефон не отвечает. Попробуйте переподключить Ваш телефон к компьютеру",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        Environment.Exit(0);
                    }
                }
                Process.Start(sourcePath);
            });
        }

        private void ShowPhotos(MyMediaFile file)
        {
            // Создание нового окна
            var wnd = new ShowImageWindow();
            var viewModel = ((MainWindowViewModel)App.Current.MainWindow.DataContext).ShowImageVM;
            viewModel.SetData(_files, file);
            wnd.DataContext = viewModel;
            wnd.Owner = App.Current.MainWindow as MainWindow;
            wnd.Show();
        }
    }
}