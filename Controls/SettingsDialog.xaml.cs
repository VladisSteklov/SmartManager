using System;
using System.Collections.Generic;
using System.IO;
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
using SmartfonManager.Source;

namespace SmartfonManager.Controls
{
    /// <summary>
    /// Логика взаимодействия для SettingsDialog.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Если путь к указанной папке не существует, то отменить событие
            if (!FileFrameWork.ExistDirectory(DirectroyPath.Text))
            {
                var result = MessageBox.Show("Указанная папка загрузки файлов не существует.\r\n" +
                    "Будет установлен путь по умолчанию", "Ошибка", MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    DirectroyPath.Text = FileFrameWork.DirectoryToLoadFiles;
                    return;
                }
                e.Cancel = true;
            }
            // Иначе установить фокус
            else App.Current.MainWindow?.Focus();
        }
    }
}
