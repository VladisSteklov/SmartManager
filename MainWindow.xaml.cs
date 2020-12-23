using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartfonManager.Controls;
using SmartfonManager.Services;
using SmartfonManager.ViewModel;
using SmartfonManager.Source;
using System.Threading;

namespace SmartfonManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            foreach (Window w in App.Current.Windows)
                w.Close();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!((MainWindowViewModel)DataContext).ShouldDeleteCache) return;
            if (FileFrameWork.ExistDirectory(FileFrameWork.DirectoryCache))
                FileFrameWork.RemoveCache();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {     
            // Создать директорию для сохранения данных, если она не существует и связать директорию c найстройкой по умолчанию
            FileFrameWork.CreateDirrectroyIfNotExits(FileFrameWork.DirectoryToLoadFiles);
            ((MainWindowViewModel)DataContext).SettingsVM.PathToLoad = FileFrameWork.DirectoryToLoadFiles;

            // Установить начальный UserControl
            var ctrl = new MainControl(((MainWindowViewModel)DataContext).LoadToTelephoneVM,
                ((MainWindowViewModel)DataContext).LoadToComputerVM);
            ctrl.InfoBox.DataContext = ((MainWindowViewModel)DataContext).Helper;
            ConnectionControls.Content = ctrl;
        }

        private void ExitProgramm(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowMain(object sender, RoutedEventArgs e)
        {
            if (ConnectionControls.Content is MainControl) return;
            var ctrl = new MainControl(((MainWindowViewModel)DataContext).LoadToTelephoneVM,
                ((MainWindowViewModel)DataContext).LoadToComputerVM);
            ctrl.InfoBox.DataContext = ((MainWindowViewModel)DataContext).Helper;
            ConnectionControls.Content = ctrl;
        }

        private void ShowLoadFromTelephone(object sender, RoutedEventArgs e)
        {
            if (ConnectionControls.Content is LoadToComputerControl) return;
            ConnectionControls.Content = new LoadToComputerControl { DataContext = ((MainWindowViewModel)DataContext).LoadToComputerVM };
        }
        private void ShowLoadToTelephone(object sender, RoutedEventArgs e)
        {
            if (ConnectionControls.Content is LoadToTelephoneControl) return;
            var ctrl = new LoadToTelephoneControl { DataContext = ((MainWindowViewModel)DataContext).LoadToTelephoneVM };
            ctrl.InfoBox.DataContext = ((MainWindowViewModel)DataContext).Helper;
            ConnectionControls.Content = ctrl;
        }

        private void ButtonMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Helper.SetStateInfo(InfoHelper.Info.ENTER_GO_TO_MENU_INFO);
        }
        private void ButtonLoadFrom_MouseEnter(object sender, MouseEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Helper.SetStateInfo(InfoHelper.Info.ENTER_GO_LOAD_TO_COMPUTER_INFO);
        }
        
        private void ButtonLoadTo_MouseEnter(object sender, MouseEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Helper.SetStateInfo(InfoHelper.Info.ENTER_GO_LOAD_TO_TELEPHONE_INFO);
        }
        
        private void ButtonInformation_MouseEnter(object sender, MouseEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Helper.SetStateInfo(InfoHelper.Info.ENTER_GO_INFORMATION_INFO);
        }
        
        private void ButtonExit_MouseEnter(object sender, MouseEventArgs e)
        {
            ((MainWindowViewModel)DataContext).Helper.SetStateInfo(InfoHelper.Info.ENTER_GO_TO_EXIT_INFO);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ConnectionControls.Content is MainControl)
            {
                ((MainWindowViewModel)DataContext).Helper.SetStateInfo(InfoHelper.Info.MAIN_WINDOW_INFO);
            }
            else if (ConnectionControls.Content is LoadToTelephoneControl)
            {
                ((MainWindowViewModel)DataContext).Helper.SetStateInfo(InfoHelper.Info.LOAD_TO_TELEPHONE_INFO);
            }
        }
        private void SettingsShow(object sender, RoutedEventArgs e)
        {
            // Заблокировать кнопку "Информация"
            SettingsButton.IsEnabled = false;
 
            var setWindow = new SettingsWindow();
            //setWindow.Owner = this;

            setWindow.DataContext = ((MainWindowViewModel)DataContext).SettingsVM;

            setWindow.Closed += SetWindow_Closed;
            setWindow.Show();
        }     
        private void SetWindow_Closed(object sender, EventArgs e)
        {
            // Активировать кнопку "Информация" после закрытия окна
            SettingsButton.IsEnabled = true;
        }
        
    }
}
