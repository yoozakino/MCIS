using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Instructions : Page
    {
        public Instructions()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateInstructions();
        }

        private void UpdateInstructions()
        {
            if (InstructionsGrid == null || SearchRecordIDTextBox == null || MedicationFilterComboBox == null)
                return;

            var context = Medical_ClinicEntities.GetContext();
            var prescriptions = context.Prescriptions.ToList();

            // Поиск по номеру мед. карты
            if (!string.IsNullOrWhiteSpace(SearchRecordIDTextBox.Text))
            {
                string search = SearchRecordIDTextBox.Text.ToLower();
                prescriptions = prescriptions
                    .Where(p => p.RecordID != null && p.RecordID.ToString().Contains(search))
                    .ToList();
            }

            // Фильтрация по лекарству
            if (MedicationFilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedMedication = selectedItem.Content.ToString();
                if (selectedMedication != "Все")
                {
                    prescriptions = prescriptions
                        .Where(p => p.MedicationName != null && p.MedicationName.Equals(selectedMedication, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            InstructionsGrid.ItemsSource = prescriptions;
        }

        private void SearchFiltersChanged(object sender, EventArgs e)
        {
            UpdateInstructions();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchRecordIDTextBox.Text = string.Empty;
            MedicationFilterComboBox.SelectedIndex = 0;
        }
    }
}
