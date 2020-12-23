using MediaDevices;
using SmartfonManager.Source;
using SmartfonManager.Commands;
using SmartfonManager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SmartfonManager.ViewModel
{
    public class LoadToTelephoneViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        public MyMediaDevice Device { get; set; }
        public LoadToTelephoneViewModel()
        {
            FilePath = "Путь к файлу";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nameProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));
        }

        private string _path;
        public string FilePath
        {
            get => _path;
            set
            {
                if (_path == value) return;
                _path = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        // Валидация пути к файлу
        public string Error => string.Empty;
        public string this[string columnName]
        {
            // Проверка существует ли заданные файлы на диске компьютера
            get
            {
                bool flag = true;
                FileNotFoundPath = "Файлы не существуют:\r\n";

                // Разделить файлы от \r\n
                var paths = SpliterRow.SplitRow(FilePath);
                foreach (var p in paths)
                {
                    if (!File.Exists(p) && FilePath != "Путь к файлу")
                    {
                        flag = false;
                        FileNotFoundPath += p + Environment.NewLine;
                    }
                        
                }
                return flag ? string.Empty : FileNotFoundPath;
            }

        }
        private string _fileNotFoundPath = "Файлы не существуют:\r\n";
        public string FileNotFoundPath
        {
            get => _fileNotFoundPath;
            set
            {
                if (_fileNotFoundPath == value) return;
                _fileNotFoundPath = value;
                OnPropertyChanged(nameof(FileNotFoundPath));
            }
        }

        private ICommand _filePathAdd = null;
        public ICommand FilePathAdd => _filePathAdd ?? (_filePathAdd = new LoadFilePathCommand());

        private ICommand _loadFilesToPhone = null;
        public ICommand LoadFilesToPhone => _loadFilesToPhone ?? (_loadFilesToPhone = new LoadFilesToPhoneCommand());
    }
}
