using SmartfonManager.Commands;
using SmartfonManager.Data;
using SmartfonManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartfonManager.ViewModel
{
    public class ShowImageWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        public MyMediaDevice Device { get; set; }
        public string PathToLoad
        {
            get => _pathToLoad;
            set
            {
                if (_pathToLoad == value) return;
                _pathToLoad = value;
                // Обновление пути загрузки в команде
                LoadMediaFile.PathToLoad = value;
            }
        }
        public MyMediaFile CurrentFileToShow { get; private set; }
        private ObservableCollection<MyMediaFile> _files;
        private int _currentIndexFile;

        private string _pathToLoad;
        private MemoryStream _memoryStream = new MemoryStream();

        public ShowImageWindowViewModel() { }
        public void SetData(ObservableCollection<MyMediaFile> files, MyMediaFile curFile)
        {
            _files = files;
            // Необходимо создать команду занаво, чтобы передать новые файлы
            _deleteMediaFile = new DeleteFileFromPhoneCommand(Device, _files, OnDeleteFile);
            CurrentFileToShow = curFile;
            _currentIndexFile = _files.IndexOf(CurrentFileToShow);

            _memoryStream.Dispose();
            _memoryStream = new MemoryStream();
            this.ChangeData();
        }

        // Изменение размера (уделение файла) коллекции приводит к переключению к следующей или предыдущей картинке
        private void OnDeleteFile(int index)
        {
            // Если был удален первый файл, то новый файл становится за ним
            // Если был удален полследний файл, то новый файл становится перед ним
            // Иначе новый файл становится следующим после удаления
            if (index == 0)
            {
                CurrentFileToShow = _files[0];
                _currentIndexFile = 0;
            }
            else if (_currentIndexFile == _files.Count)
            {
                CurrentFileToShow = _files[_files.Count - 1];
                _currentIndexFile = _files.Count - 1;
            }
            else
            {
                CurrentFileToShow = _files[_currentIndexFile];
            }
            _memoryStream.Dispose();
            _memoryStream = new MemoryStream();
            this.ChangeData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nameProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));
        }

        private bool _isActiveLeftButton;
        public bool ActiveLeftButton
        {
            get => _isActiveLeftButton;
            set
            {
                if (_isActiveLeftButton == value) return;
                _isActiveLeftButton = value;
                OnPropertyChanged(nameof(ActiveLeftButton));
            }
        }
        private bool _isActiveRightButton;
        public bool ActiveRightButton
        {
            get => _isActiveRightButton;
            set
            {
                if (_isActiveRightButton == value) return;
                _isActiveRightButton = value;
                OnPropertyChanged(nameof(ActiveRightButton));
            }
        }
        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set
            {
                if (_image == value) return;
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName == value) return;
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }
        private void ChangeData()
        {
            ActiveLeftButton = _currentIndexFile == 0 ? false : true;
            ActiveRightButton = _currentIndexFile == _files.Count - 1 ? false : true;

            // Преобразование строки
            FileName = CurrentFileToShow.FileName;

            // Преобразование изображения
            Device.Device.DownloadFile(CurrentFileToShow.FilePath, _memoryStream);
            if (CurrentFileToShow.ExtensionString == ".jpg" || CurrentFileToShow.ExtensionString == ".jpeg")
            {
                // JPG декодирование
                _memoryStream.Seek(0, SeekOrigin.Begin);
                JpegBitmapDecoder decoder = new JpegBitmapDecoder(_memoryStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                Image = decoder.Frames[0];
            }
            else if (CurrentFileToShow.ExtensionString == ".bmp" || CurrentFileToShow.ExtensionString == ".png")
            {
                var imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = _memoryStream;
                imageSource.EndInit();
                Image = imageSource;
            }       

        }
        public void SetNext()
        {
            _memoryStream.Dispose();
            _memoryStream = new MemoryStream();
            CurrentFileToShow = _files[++_currentIndexFile];
            this.ChangeData();
        }
        public void SetPreview()
        {
            _memoryStream.Dispose();
            _memoryStream = new MemoryStream();
            CurrentFileToShow = _files[--_currentIndexFile];
            this.ChangeData();
        }

        public void Dispose()
        {
            _memoryStream.Dispose();
        }

        private LoadFileToComputerCommand _loadMediaFile = null;
        public LoadFileToComputerCommand LoadMediaFile => _loadMediaFile ?? (_loadMediaFile = new LoadFileToComputerCommand(Device) { PathToLoad = this.PathToLoad });

        private DeleteFileFromPhoneCommand _deleteMediaFile = null;
        public DeleteFileFromPhoneCommand DeleteMediaFile => _deleteMediaFile ?? (_deleteMediaFile = new DeleteFileFromPhoneCommand(Device, _files, OnDeleteFile));
    }
}
