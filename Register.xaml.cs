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
using System.Windows.Shapes;
using SmartfonManager.ViewModel;
using MediaDevices;
using SmartfonManager.Services;

namespace SmartfonManager
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
            Loaded += Register_Loaded;
        }

        private void Register_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new RegisterViewModel();
        }

        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            // Если элемент выбран
            if (this.DevicesListBox.SelectedItem != null)
            {
                MediaDevice selectedDevice = DevicesListBox.SelectedItem as MediaDevice;
                if (selectedDevice != null)
                {
                    try
                    {
                        MyMediaDevice device = new MyMediaDevice(selectedDevice);
                        MainWindow wnd = new MainWindow { DataContext = new MainWindowViewModel(device) };
                        wnd.Show();
                        App.Current.MainWindow = wnd;
                        this.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Для корректной передачи файлов в настройках USB подключения своего устройства выберите пункт \"Передача данных\"",
                            ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                   
                    
                }             
            }
            else
            {
                MessageBox.Show("Устройство не выбрано", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
