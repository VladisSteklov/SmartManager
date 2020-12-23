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
using SmartfonManager.Commands;
using SmartfonManager.ViewModel;

namespace SmartfonManager.Controls
{
    /// <summary>
    /// Логика взаимодействия для ShowImageWindow.xaml
    /// </summary>
    public partial class ShowImageWindow : Window
    {
        public ShowImageWindow()
        {
            InitializeComponent();
            Loaded += ShowImageWindow_Loaded;
            Closing += ShowImageWindow_Closing;
        }

        private void ShowImageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        private void ShowImageWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.MainWindow?.Focus();
        }

        private void LoadFileToComputer(object sender, RoutedEventArgs e)
        {
            ((ShowImageWindowViewModel)DataContext).LoadMediaFile.ExecuteAsync(
                ((ShowImageWindowViewModel)DataContext).CurrentFileToShow);
        }

        private void DeleteFileFromPhone(object sender, RoutedEventArgs e)
        {
            ((ShowImageWindowViewModel)DataContext).DeleteMediaFile.Execute(
                ((ShowImageWindowViewModel)DataContext).CurrentFileToShow);
        }

        private void SetNextFile(object sender, RoutedEventArgs e)
        {
            ((ShowImageWindowViewModel)DataContext).SetNext();
        }

        private void SetBeforeFile(object sender, RoutedEventArgs e)
        {
            ((ShowImageWindowViewModel)DataContext).SetPreview();
        }
    }
}
