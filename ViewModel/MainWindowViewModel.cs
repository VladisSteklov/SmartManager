using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MediaDevices;
using SmartfonManager.Commands;
using SmartfonManager.Services;
using SmartfonManager.Source.Interfaces;

namespace SmartfonManager.ViewModel
{
    public class MainWindowViewModel: IGivingData
    {
        public bool ShouldDeleteCache { get; private set; }
        // Портативное устройство
        public MyMediaDevice Device { get; set; }
        public LoadToComputerViewModel LoadToComputerVM { get; set; }
        public LoadToTelephoneViewModel LoadToTelephoneVM { get;  set; }
        public SettingsViewModel SettingsVM { get; set; }
        public InfoHelper Helper { get; set; }
        public ShowImageWindowViewModel ShowImageVM { get; set; }

        public MainWindowViewModel() {}
        public MainWindowViewModel(MyMediaDevice device)
        {
            Device = device;
            LoadToComputerVM = new LoadToComputerViewModel() { Device = device };
            LoadToTelephoneVM = new LoadToTelephoneViewModel() { Device = device };
            ShowImageVM = new ShowImageWindowViewModel() { Device = device };
            //Передача ссылки на интерфейс SettingsVM
            SettingsVM = new SettingsViewModel(this);
            Helper = new InfoHelper();
        }

        void IGivingData.GivePathToLoad()
        {
            // Передача пути загрузки
            LoadToComputerVM.PathToLoad = SettingsVM.PathToLoad;
            ShowImageVM.PathToLoad = SettingsVM.PathToLoad;
        }

        void IGivingData.GiveDeleteCacheSettings(bool shouldDelete)
        {
            this.ShouldDeleteCache = shouldDelete;
        }

        void IGivingData.GiveSortedTypeSettings(TypeSort typeOfSort)
        {
            LoadToComputerVM.TypeOfSort = typeOfSort;
        }

        void IGivingData.GiveViewSettings(bool visibilityList, bool visibilityGrid)
        {
            LoadToComputerVM.ListViewVisibility = visibilityList ? Visibility.Visible : Visibility.Collapsed;
            LoadToComputerVM.GridViewVisibility = visibilityGrid ? Visibility.Visible : Visibility.Collapsed;
        }

        void IGivingData.GiveSortedTypeOrderSettings(TypeSortOrder typeOfSortOrdering)
        {
            LoadToComputerVM.TypeOfSortOrder = typeOfSortOrdering;
        }
    }
}
