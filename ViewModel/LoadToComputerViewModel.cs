using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaDevices;
using SmartfonManager.Services;
using SmartfonManager.Data;
using System.Windows.Input;
using SmartfonManager.Commands;
using System.Windows;
using System.Windows.Data;
using System.Collections;
using SmartfonManager.Source.Interfaces;

namespace SmartfonManager.ViewModel
{
    public enum TypeSortOrder
    {
        Asceding,
        Desceding
    }
    public enum TypeSort
    {
        OnFileName,
        OnFileSize,
        OnFileDate,
        NoSort
    }

    public class LoadToComputerViewModel : INotifyPropertyChanged
    {
        public ITakingSortParameters giveSortParametres { get; set; }
        public MyMediaDevice Device { get; set; }

        public ObservableCollection<MyMediaFile> _myMediaFiles;
        public ObservableCollection<MyMediaFile> Files => _myMediaFiles?? (_myMediaFiles = new ObservableCollection<MyMediaFile>());

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nameProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));
        }

        private string _pathToLoad;
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

        private Visibility _loadingTextVisibilty = Visibility.Collapsed;
        public Visibility LoadingTextVisibilty
        {
            get => _loadingTextVisibilty;
            set
            {
                if (_loadingTextVisibilty == value) return;
                _loadingTextVisibilty = value;
                OnPropertyChanged(nameof(LoadingTextVisibilty));
            }
        }
        private void EnableVisibilityLoadingText()
        {
            LoadingTextVisibilty = Visibility.Visible;
        }
        private void DisableVisibilityLoadingText()
        {
            LoadingTextVisibilty = Visibility.Collapsed;
        }

        private TypeSortOrder _typeOfSortOdred;
        public TypeSortOrder TypeOfSortOrder
        {
            get => _typeOfSortOdred;
            set
            {
                _typeOfSortOdred = value;
                giveSortParametres?.TakeSortParameters(_typeOfSort, value);

            }
        }
        private TypeSort _typeOfSort;
        public TypeSort TypeOfSort
        {
            get => _typeOfSort;
            set
            {
                _typeOfSort = value;
                giveSortParametres?.TakeSortParameters(value, _typeOfSortOdred);
            }
        }
        private Visibility _listViewVisibility;
        public Visibility ListViewVisibility
        {
            get => _listViewVisibility;
            set
            {
                if (_listViewVisibility == value) return;
                _listViewVisibility = value;
                OnPropertyChanged(nameof(ListViewVisibility));
            }
        }
        private Visibility _gridVisibility;
        public Visibility GridViewVisibility
        {
            get => _gridVisibility;
            set
            {
                if (_gridVisibility == value) return;
                _gridVisibility = value;
                OnPropertyChanged(nameof(GridViewVisibility));
            }
        }
        

        private ShowListMediaFilesCommand _showMediaFiles = null;
        public ShowListMediaFilesCommand ShowMediaFiles => _showMediaFiles ?? (_showMediaFiles = new ShowListMediaFilesCommand(Device, 
            Files, EnableVisibilityLoadingText, DisableVisibilityLoadingText));
      
        private LoadFileToComputerCommand _loadMediaFile = null;
        public LoadFileToComputerCommand LoadMediaFile => _loadMediaFile ?? (_loadMediaFile = new LoadFileToComputerCommand(Device) { PathToLoad = this._pathToLoad });

        private DeleteFileFromPhoneCommand _deleteMediaFile = null;
        public DeleteFileFromPhoneCommand DeleteMediaFile => _deleteMediaFile?? (_deleteMediaFile = new DeleteFileFromPhoneCommand(Device, Files));

        private OpenMediaFileCommand _openMediaFile = null;
        public OpenMediaFileCommand OpenMediaFile => _openMediaFile ?? (_openMediaFile = new OpenMediaFileCommand(Device, Files));

    }
}
