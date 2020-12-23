using Microsoft.Win32;
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
using SmartfonManager.Commands;
using SmartfonManager.ViewModel;
using SmartfonManager.Services;

namespace SmartfonManager.Controls
{
    /// <summary>
    /// Логика взаимодействия для LoadToTelephone.xaml
    /// </summary>
    public partial class LoadToTelephoneControl : UserControl
    {
        public LoadToTelephoneControl()
        {            
            InitializeComponent();
            Loaded += LoadToTelephoneControl_Loaded;
        }

        private void LoadToTelephoneControl_Loaded(object sender, RoutedEventArgs e)
        { 
            ((InfoHelper)InfoBox.DataContext).SetStateInfo(InfoHelper.Info.LOAD_TO_TELEPHONE_INFO);
        }
        private void ButtonLoadTo_MouseEnter(object sender, MouseEventArgs e)
        {
            ((InfoHelper)InfoBox.DataContext).SetStateInfo(InfoHelper.Info.LOAD_TO_BUTTON_INFO);
        }
        private void ButtonFoundFile_MouseEnter(object sender, MouseEventArgs e)
        {
            ((InfoHelper)InfoBox.DataContext).SetStateInfo(InfoHelper.Info.LOAD_FILE_BUTTON_INFO);
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((InfoHelper)InfoBox.DataContext).SetStateInfo(InfoHelper.Info.LOAD_TO_TELEPHONE_INFO);
        }

        async private void FilePath_PreviewDropAsync(object sender, DragEventArgs e)
        {
            // Пользователь переносит файлы в контрол
            this.PathFile.Text = await Task.Run<string>(() => FilePathDropCommand.Execute(e));
        }
    }
}
