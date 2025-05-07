using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Medications : Page
    {
        public Medications()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateMedications();
        }

        private void UpdateMedications()
        {
            if (MedicationsGrid == null || SearchMedicationNameTextBox == null)
                return;

            var context = Medical_ClinicEntities.GetContext();
            var medications = context.Medications.ToList();

            // Поиск по наименованию
            if (!string.IsNullOrWhiteSpace(SearchMedicationNameTextBox.Text))
            {
                string search = SearchMedicationNameTextBox.Text.ToLower();
                medications = medications
                    .Where(m => m.MedicationName != null && m.MedicationName.ToLower().Contains(search))
                    .ToList();
            }

            MedicationsGrid.ItemsSource = medications;
        }

        private void SearchMedicationName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMedications();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchMedicationNameTextBox.Text = string.Empty;
        }
    }
}
