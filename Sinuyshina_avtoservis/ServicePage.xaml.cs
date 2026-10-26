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

namespace Sinuyshina_avtoservis
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent(); 
            var currentServices = Sinuyshina_avtoservisEntities.GetContext().Service.ToList();
            ServiceListView.ItemsSource = currentServices;
            ComboType.SelectedIndex = 0;
            UpdateServices();
        }
        private void UpdateServices()
        {
            var currentServices = Sinuyshina_avtoservisEntities.GetContext().Service.ToList();

            if(ComboType.SelectedIndex==0)
            {
                currentServices = currentServices.Where(p => (p.Discount == null || p.Discount >= 0 && p.Discount <= 1)).ToList();
            }
            if(ComboType.SelectedIndex==1)
            {
                currentServices = currentServices.Where(p => (p.Discount == null || p.Discount >= 0 && p.Discount < 0.05)).ToList();
            }
            if(ComboType.SelectedIndex==2)
            {
                currentServices = currentServices.Where(p => (p.Discount >= 0.05 && p.Discount < 0.15)).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => (p.Discount >= 0.15 && p.Discount < 0.3)).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => (p.Discount >= 0.3 && p.Discount < 0.7)).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentServices = currentServices.Where(p => (p.Discount >= 0.7 && p.Discount < 1)).ToList();
            }

            currentServices = currentServices.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            if(RButtonDown.IsChecked.Value)
            {
                currentServices = currentServices.OrderByDescending(p => p.Cost).ToList();
            }
            if(RButtonUp.IsChecked.Value)
            {
                currentServices = currentServices.OrderBy(p => p.Cost).ToList();
            }
            ServiceListView.ItemsSource = currentServices;
        }
        
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //открыть окно редактирования/добавления услуг
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //кнопка редактировакния 
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Service));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                Sinuyshina_avtoservisEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ServiceListView.ItemsSource = Sinuyshina_avtoservisEntities.GetContext().Service.ToList();
            }
        }
    }

       
    }
