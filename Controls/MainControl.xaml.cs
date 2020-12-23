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
using SmartfonManager.Services;
using SmartfonManager.ViewModel;

namespace SmartfonManager.Controls
{
    /// <summary>
    /// Логика взаимодействия для MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        // Если UserControl сменяется, устанавливается в false
        private bool _isActive;
        // Кисть для сохнанения цвета Label
        private Brush _brusLabel;

        // ViewModels пораждаемых окон
        private readonly LoadToComputerViewModel _vmComp;
        private readonly LoadToTelephoneViewModel _vmTel;

        public MainControl()
        {
            InitializeComponent();
            Loaded += MainControl_Loaded;
        }
        public MainControl(LoadToTelephoneViewModel vmTel, LoadToComputerViewModel vmComp) : this()
        {
            _vmTel = vmTel;
            _vmComp = vmComp;
        }

        private void MainControl_Loaded(object sender, RoutedEventArgs e)
        {
            _isActive = true;
            // Установить датаконтест для панели помощи
            ((InfoHelper)InfoBox.DataContext).SetStateInfo(InfoHelper.Info.MAIN_WINDOW_INFO);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {       
            var button = (Button)sender;
            if (button.Name == "LoadFrom")
            {
                ((InfoHelper)InfoBox.DataContext).SetStateInfo(InfoHelper.Info.ENTER_GO_LOAD_TO_COMPUTER_INFO);
                _brusLabel = LoadFromLabel.Foreground;
                LoadFromLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ((InfoHelper)InfoBox.DataContext).SetStateInfo(InfoHelper.Info.ENTER_GO_LOAD_TO_TELEPHONE_INFO);
                _brusLabel = LoadToLabel.Foreground;
                LoadToLabel.Foreground = new SolidColorBrush(Colors.Red);
            }               
           
        }

        void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_isActive == true)
            {
                ((InfoHelper)InfoBox.DataContext).SetStateInfo(InfoHelper.Info.MAIN_WINDOW_INFO);
                // Изменить цвет шрифта в текстбоксе на обратный
                var button = (Button)sender;
                if (button.Name == "LoadFrom") LoadFromLabel.Foreground = _brusLabel;
                else LoadToLabel.Foreground = _brusLabel;
            }    
        }

        private void LoadControl(object sender, RoutedEventArgs e)
        {
            // Вернуть стиль для нажатой мыши
            Button_MouseLeave(sender, null);
            // Данный UserControl сменяет себя. Это не дает вызвать Button_MouseLeave после смены UserControl
            _isActive = false;
            // Получить экземпляр главного окна и установить контент в LoadToTelephoneControl
            var mainWind = App.Current.MainWindow as MainWindow;

            UserControl ctrl = null;
            if (((Button)sender).Name == "LoadFrom")
            {
                ctrl = new LoadToComputerControl();
                ctrl.DataContext = _vmComp;
            }
            else
            {
                ctrl = new LoadToTelephoneControl();
                ((LoadToTelephoneControl)ctrl).InfoBox.DataContext = this.InfoBox.DataContext;
                ctrl.DataContext = _vmTel;
            }

            if (mainWind != null) { mainWind.ConnectionControls.Content = ctrl; }
            return;
        }
    }
}
