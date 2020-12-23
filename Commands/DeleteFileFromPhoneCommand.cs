using SmartfonManager.Data;
using SmartfonManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartfonManager.Commands
{
    public class DeleteFileFromPhoneCommand
    {
        private readonly MyMediaDevice _myMediaDevice;
        private readonly ObservableCollection<MyMediaFile> _files;

        private readonly Action<int> _changeShowFile = null;
        public DeleteFileFromPhoneCommand() { }
        public DeleteFileFromPhoneCommand(MyMediaDevice device, ObservableCollection<MyMediaFile> files) : this(device, files, null) { }

        public DeleteFileFromPhoneCommand(MyMediaDevice device, ObservableCollection<MyMediaFile> files, Action<int> action)
        {
            _myMediaDevice = device;
            _files = files;
            _changeShowFile = action;
        }
        public void Execute(object parameter)
        {
           
            MyMediaFile file = parameter as MyMediaFile;
            if (file == null || !_myMediaDevice.Device.FileExists(file.FilePath))
            {
                MessageBox.Show("Не удается выполнить операцию.\r\nВозможно указанный файл не существует.",
                       "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Взятие индекса удаленного файла
            var indexDeletedFile = _files.IndexOf(file);
            // Удаление файла из списка
            _files.Remove(file);
            // Удаление файла из устройства
            _myMediaDevice.Device.DeleteFile(file.FilePath);
            // Изменение порядка отображаемых данных
            _changeShowFile?.Invoke(indexDeletedFile);
        }
    }
}
