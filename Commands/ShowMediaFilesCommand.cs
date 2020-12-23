using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SmartfonManager.Data;
using SmartfonManager.Services;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Data;
using System.Collections;

namespace SmartfonManager.Commands
{
    public class ShowListMediaFilesCommand : BaseCommand
    {
        private readonly string[] _filterTextFiles = new string[6] { ".doc", ".docx", ".pdf", ".txt", ".xls", ".xlsx" };
        private readonly string[] _filterMusic = new string[1] { ".mp3" };
        private readonly string[] _filterVideos = new string[2] { ".mp4", ".MOV" };
        private readonly string[] _filterPhotos = new string[5] { ".png", ".jpg", ".jpeg", ".bmp", ".djvu" };
        private readonly Action EnableVisibility;
        private readonly Action DisableVisibility;

        private readonly MyMediaDevice _myDevice;
        private readonly ObservableCollection<MyMediaFile> _files;
        private readonly Dispatcher _mainDispatcher = Application.Current.MainWindow.Dispatcher;
        private Thread _thread = null;
        public ShowListMediaFilesCommand() : this(null, null, null, null) { }
        public ShowListMediaFilesCommand(MyMediaDevice device, ObservableCollection<MyMediaFile> files, Action EnableVisibilty, Action DisableVisibilty)
        {
            _myDevice = device;
            _files = files;
            this.EnableVisibility = EnableVisibilty; 
            this.DisableVisibility = DisableVisibilty; 
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            // Отчистка списка файлов
            _files.Clear();
            if (_thread != null)
            {
                // Закончить выполнение потока
                _thread.Abort();
            }
            // Установить видимость текста для загрузки
            EnableVisibility?.Invoke();
            _thread = new Thread(() => Working(parameter));
            _thread.Priority = ThreadPriority.Highest;
            _thread.IsBackground = true;
            _thread.Start();
        }
        private void Working(object parameter)
        {
            try
            {
                ExecuteWork(@"\\", (TypeFile)parameter);
            }
            catch
            {
            }
            MessageBox.Show("Поиск файлов окончен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            DisableVisibility?.Invoke();
        }

        private void ExecuteWork(string root, TypeFile type)
        {
            // Для каждого файла, расположенного в текущей директории root
            foreach (var file in _myDevice.Device.GetFiles(root))
            {
                // Получить информацию о файле по его имени
                var infoMediaFile = _myDevice.Device.GetFileInfo(file);
                // Получение расширения файла. Если расширение файла отсутсвует, то оно остается пустым
                var extentionFileString = file.Contains('.') ? file.Remove(0, file.LastIndexOf('.')) : "";

                // Если расширение рассматриваемого файла находится в фитльтре,
                // то создать экземпляр файла и поместить в список
                // Фильтр выбирается в зависимости от типа искомого файла
                if (
                    (type == TypeFile.TextFile & _filterTextFiles.Any((elem) => String.Equals(elem, extentionFileString, StringComparison.CurrentCultureIgnoreCase))) |
                    (type == TypeFile.Photo & _filterPhotos.Any((elem) => String.Equals(elem, extentionFileString, StringComparison.CurrentCultureIgnoreCase))) |
                    (type == TypeFile.Video & _filterVideos.Any((elem) => String.Equals(elem, extentionFileString, StringComparison.CurrentCultureIgnoreCase))) |
                    (type == TypeFile.Music & _filterMusic.Any((elem) => String.Equals(elem, extentionFileString, StringComparison.CurrentCultureIgnoreCase))) |
                    (type == TypeFile.Others & !_filterTextFiles.Any((elem) => String.Equals(elem, extentionFileString, StringComparison.CurrentCultureIgnoreCase)) &
                    !_filterPhotos.Any((elem) => String.Equals(elem, extentionFileString, StringComparison.CurrentCultureIgnoreCase)) & 
                    !_filterMusic.Any((elem) => String.Equals(elem, extentionFileString, StringComparison.CurrentCultureIgnoreCase)) & !_filterVideos.Any((elem) => String.Equals(elem, extentionFileString, StringComparison.CurrentCultureIgnoreCase)))
                   )
                {
                    try
                    {
                        _mainDispatcher.Invoke(() =>
                        {
                            var myMediaFile = new MyMediaFile(infoMediaFile.Name, infoMediaFile.FullName, type,
                                (DateTime)infoMediaFile.LastWriteTime, infoMediaFile.Length);
                            switch (myMediaFile.Extension)
                            {
                                case TypeFile.TextFile:
                                    if (String.Equals(myMediaFile.ExtensionString, ".doc", StringComparison.CurrentCultureIgnoreCase)
                                    ||  String.Equals(myMediaFile.ExtensionString, ".docx", StringComparison.CurrentCultureIgnoreCase))
                                        myMediaFile.Image = new BitmapImage(new Uri(@"\Images\doc.png", UriKind.Relative));

                                    else if (String.Equals(myMediaFile.ExtensionString, ".pdf", StringComparison.CurrentCultureIgnoreCase))
                                        myMediaFile.Image = new BitmapImage(new Uri(@"\Images\pdf.png", UriKind.Relative));
                                    else if (String.Equals(myMediaFile.ExtensionString, ".xls", StringComparison.CurrentCultureIgnoreCase) 
                                    || String.Equals(myMediaFile.ExtensionString, ".xlsx", StringComparison.CurrentCultureIgnoreCase))
                                        myMediaFile.Image = new BitmapImage(new Uri(@"\Images\excel.png", UriKind.Relative));
                                    else
                                        myMediaFile.Image = new BitmapImage(new Uri(@"\Images\txt.png", UriKind.Relative));
                                    break;
                                case TypeFile.Others:
                                    if (String.Equals(extentionFileString,".zip", StringComparison.CurrentCultureIgnoreCase) ||
                                        String.Equals(extentionFileString, ".rar", StringComparison.CurrentCultureIgnoreCase))
                                        myMediaFile.Image = new BitmapImage(new Uri(@"\Images\archive.png", UriKind.Relative));
                                    else
                                        myMediaFile.Image = new BitmapImage(new Uri(@"\Images\unknow_file.png", UriKind.Relative));
                                    break;
                                case TypeFile.Music:
                                    myMediaFile.Image = new BitmapImage(new Uri(@"\Images\mp3.png", UriKind.Relative));
                                    break;
                                case TypeFile.Video:
                                    myMediaFile.Image = new BitmapImage(new Uri(@"\Images\mp4.png", UriKind.Relative));
                                    break;
                                case TypeFile.Photo:
                                    myMediaFile.Image = new BitmapImage(new Uri(@"\Images\photo.png", UriKind.Relative));
                                    break;
                            }
                            _files.Add(myMediaFile);
                        },
                        DispatcherPriority.Render);
                    }
                    catch
                    {
                        // Задача прерывается, путемы выхода из программы
                        Environment.Exit(0);
                    }

                }
            }
            // Рекурсия по всем директориям внутри данный папки root
            foreach (var newRoot in _myDevice.Device.GetDirectories(root))
            {
                if (!newRoot.Contains("Android"))
                {
                    // Осуществлять поиск текстовых, ... файлов кроме папки DCIM
                    if ((type == TypeFile.Photo || type == TypeFile.Video) ||
                        ((type == TypeFile.Music || type == TypeFile.TextFile || type == TypeFile.Others) && !newRoot.Contains("DCIM")))
                        ExecuteWork(newRoot, type);
                }
            }
        }
    }
}
