using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Invoices : Page
    {
        public Invoices()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if (InvoicesGrid == null)
                return;

            string searchText = SearchPatientNameTextBox?.Text?.Trim().ToLower() ?? "";
            string selectedStatus = (StatusFilterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            var invoices = Medical_ClinicEntities.GetContext().Invoices.ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                invoices = invoices
                    .Where(i => i.PatientName != null && i.PatientName.ToLower().Contains(searchText))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(selectedStatus) && selectedStatus != "Все")
            {
                invoices = invoices
                    .Where(i => i.Statuss != null && i.Statuss.ToLower() == selectedStatus.ToLower())
                    .ToList();
            }

            InvoicesGrid.ItemsSource = invoices;
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchPatientNameTextBox.Text = "";
            if (StatusFilterComboBox.Items.Count > 0)
                StatusFilterComboBox.SelectedIndex = 0;
        }
    }
}
