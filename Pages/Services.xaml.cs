using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Services : Page
    {
        public Services()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void UpdateServices()
        {
            if (ServicesGrid == null || SearchServiceName == null)
                return;

            var context = Medical_ClinicEntities.GetContext();
            var services = context.Servicess.ToList();

            // Фильтр по названию
            if (!string.IsNullOrWhiteSpace(SearchServiceName.Text))
            {
                string search = SearchServiceName.Text.ToLower();
                services = services
                    .Where(s => s.ServiceName != null && s.ServiceName.ToLower().Contains(search))
                    .ToList();
            }

            ServicesGrid.ItemsSource = services;
        }

        private void SearchServiceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchServiceName.Text = string.Empty;
        }
    }
}
