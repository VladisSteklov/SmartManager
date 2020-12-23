using SmartfonManager.Source.Interfaces;
using SmartfonManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartfonManager.Controls
{
    /// <summary>
    /// Логика взаимодействия для LoadToComputer.xaml
    /// </summary>
    public partial class LoadToComputerControl : UserControl, ITakingSortParameters
    {
        private TypeSort _typeSort;
        private TypeSortOrder _typeSortOrder;
        public LoadToComputerControl()
        {
            InitializeComponent();
            Loaded += LoadToComputerControl_Loaded;
        }

        private void LoadToComputerControl_Loaded(object sender, RoutedEventArgs e)
        {
            _typeSort = ((LoadToComputerViewModel)DataContext).TypeOfSort;
            _typeSortOrder = ((LoadToComputerViewModel)DataContext).TypeOfSortOrder;
            this.SetBindingSortList(_typeSort, _typeSortOrder);
            ((LoadToComputerViewModel)DataContext).giveSortParametres = this;
        }

        private void LoadFileToComputer(object sender, RoutedEventArgs e)
        {
            ((LoadToComputerViewModel)DataContext).LoadMediaFile.ExecuteAsync(((Button)sender).DataContext);
        }

        private void DeleteFileFromPhone(object sender, RoutedEventArgs e)
        {
            ((LoadToComputerViewModel)DataContext).DeleteMediaFile.Execute(((Button)sender).DataContext);
        }

        private void ShowMediaFile(object sender, RoutedEventArgs e)
        {
            ((LoadToComputerViewModel)DataContext).OpenMediaFile.Execute(((Button)sender).DataContext);
        }
        private void SetBindingSortList(TypeSort typeSort, TypeSortOrder typeSortOrder)
        {
            Binding bindingItems = new Binding();
            if (typeSortOrder == TypeSortOrder.Asceding)
            {
                switch (typeSort)
                {
                    case TypeSort.NoSort:
                        bindingItems.Source = this.Resources["ItemsNoSorted"];
                        break;
                    case TypeSort.OnFileDate:
                        bindingItems.Source = this.Resources["ItemsSortedByDateAsceding"];
                        break;
                    case TypeSort.OnFileName:
                        bindingItems.Source = this.Resources["ItemsSortedByNamesAsceding"];
                        break;
                    case TypeSort.OnFileSize:
                        bindingItems.Source = this.Resources["ItemsSortedByLengthAsceding"];
                        break;
                }

            }
            else
            {
                switch (typeSort)
                {
                    case TypeSort.NoSort:
                        bindingItems.Source = this.Resources["ItemsNoSorted"];
                        break;
                    case TypeSort.OnFileDate:
                        bindingItems.Source = this.Resources["ItemsSortedByDateDesceding"];
                        break;
                    case TypeSort.OnFileName:
                        bindingItems.Source = this.Resources["ItemsSortedByNamesDesceding"];
                        break;
                    case TypeSort.OnFileSize:
                        bindingItems.Source = this.Resources["ItemsSortedByLengthDesceding"];
                        break;
                }

            }
            this.ListViewItems.SetBinding(ListView.ItemsSourceProperty, bindingItems);
            this.GridViewItems.SetBinding(ListView.ItemsSourceProperty, bindingItems);
        }

        void ITakingSortParameters.TakeSortParameters(TypeSort typeSort, TypeSortOrder typeSortOrder)
        {
            this.SetBindingSortList(typeSort, typeSortOrder);
        }
    }
}
