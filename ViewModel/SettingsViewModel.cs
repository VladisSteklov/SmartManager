using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using SmartfonManager.Commands;
using SmartfonManager.Source.Interfaces;
using SmartfonManager.Source;

namespace SmartfonManager.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel() : this(null) { }
        public SettingsViewModel(IGivingData givingData)
        {
            _giveSettings = givingData;
            // Настройки по умолчанию
            SetActiveTelephone();
            DeleteAuto = true;
            ListView = true;
            NoSorted = true;
            SortedDescending = true;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private IGivingData _giveSettings;
        private void OnPropertyChanged(string nameProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProperty));
        }
        // Свойства
        private bool _activeTelephone;
        public bool ActiveTelephone  // Левый чекбокс
        {
            get => _activeTelephone;
            set
            {
                if (_activeTelephone == value) return;
                _activeTelephone = value;
                OnPropertyChanged(nameof(ActiveTelephone));
            }
        }

        private bool _noActiveTelephone;
        public bool NoActiveTelephone  // Правый чекбокс
        {
            get => _noActiveTelephone;
            set
            {
                if (_noActiveTelephone == value) return;
                _noActiveTelephone = value;
                OnPropertyChanged(nameof(NoActiveTelephone));
            }
        }

        private bool _deleteAutoCache;
        public bool DeleteAuto  
        {
            get => _deleteAutoCache;
            set
            {
                if (_deleteAutoCache == value) return;
                _deleteAutoCache = value;
                OnPropertyChanged(nameof(DeleteAuto));
                _giveSettings?.GiveDeleteCacheSettings(value);
            }
        }
        private bool _noDeleteAutoCache;
        public bool NoDelete
        {
            get => _noDeleteAutoCache;
            set
            {
                if (_noDeleteAutoCache == value) return;
                _noDeleteAutoCache = value;
                OnPropertyChanged(nameof(NoDelete));
            }
        }

        private bool _listViewPresentation;
        public bool ListView
        {
            get => _listViewPresentation;
            set
            {
                if (_listViewPresentation == value) return;
                _listViewPresentation = value;
                OnPropertyChanged(nameof(ListView));
                _giveSettings?.GiveViewSettings(value , !value);
            }
        }
        private bool _gridPresentation;
        public bool GridView
        {
            get => _gridPresentation;
            set
            {
                if (_gridPresentation == value) return;
                _gridPresentation = value;
                OnPropertyChanged(nameof(GridView));
            }
        }
        private bool _noSorted;
        public bool NoSorted
        {
            get => _noSorted;
            set
            {
                if (_noSorted == value) return;
                _noSorted = value;
                OnPropertyChanged(nameof(NoSorted));
                // Передача осуществляется, если флажок установлен
                if (value) _giveSettings?.GiveSortedTypeSettings(TypeSort.NoSort);
            }
        }
        private bool _sortedByDate;
        public bool SortedByDate
        {
            get => _sortedByDate;
            set
            {
                if (_sortedByDate == value) return;
                _sortedByDate = value;
                OnPropertyChanged(nameof(SortedByDate));
                // Передача осуществляется, если флажок установлен
                if (value) _giveSettings?.GiveSortedTypeSettings(TypeSort.OnFileDate);
            }
        }
        private bool _sortedByLength;
        public bool SortedByLength
        {
            get => _sortedByLength;
            set
            {
                if (_sortedByLength == value) return;
                _sortedByLength = value;
                OnPropertyChanged(nameof(SortedByLength));
                // Передача осуществляется, если флажок установлен
                if (value) _giveSettings?.GiveSortedTypeSettings(TypeSort.OnFileSize);
            }
        }
        private bool _sortedByName;
        public bool SortedByName
        {
            get => _sortedByName;
            set
            {
                if (_sortedByName == value) return;
                _sortedByName = value;
                OnPropertyChanged(nameof(SortedByName));
                if (value) _giveSettings?.GiveSortedTypeSettings(TypeSort.OnFileName);
            }
        }
        private bool _sortedAsceding;
        public bool SortedAsceding
        {
            get => _sortedAsceding;
            set
            {
                if (_sortedAsceding == value) return;
                _sortedAsceding = value;
                OnPropertyChanged(nameof(SortedAsceding));
                // Передача осуществляется, если флажок установлен
                if (value)
                    _giveSettings?.GiveSortedTypeOrderSettings(TypeSortOrder.Asceding);
            }
        }
        private bool _sortedDesceding;
        public bool SortedDescending
        {
            get => _sortedDesceding;
            set
            {
                if (_sortedDesceding == value) return;
                _sortedDesceding = value;
                OnPropertyChanged(nameof(SortedDescending));
                if (value)
                    _giveSettings?.GiveSortedTypeOrderSettings(TypeSortOrder.Desceding);
            }
        }

        private string _pathToLoad;
        public string PathToLoad
        {
            get => _pathToLoad;
            set
            {
                if (_pathToLoad == value) return;
                _pathToLoad = value;
                OnPropertyChanged(nameof(PathToLoad));
                // Передача в ViewModelLoadToComputer пути загрузки, если он существует
                if (FileFrameWork.ExistDirectory(PathToLoad)) _giveSettings?.GivePathToLoad();
            }
        }

        // Команды
        private ICommand _dirPathAdd = null;
        public ICommand DirPathAdd => _dirPathAdd ?? (_dirPathAdd = new LoadDirPathCommand());

        // Методы
        public void SetActiveTelephone()
        {
            ActiveTelephone = true;
            NoActiveTelephone = false;
        }
        public void SetNoActiveTelephone()
        {
            ActiveTelephone = false;
            NoActiveTelephone = true;
        }
    }
}
