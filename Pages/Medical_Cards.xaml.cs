using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Medical_Cards : Page
    {
        public Medical_Cards()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateMedicalCards();
        }

        private void UpdateMedicalCards()
        {
            if (MedCardsGrid == null || SearchPatientName == null || DiagnosisFilterComboBox == null)
                return;

            var context = Medical_ClinicEntities.GetContext();
            var cards = context.MedicalRecords.ToList();

            // Поиск по имени пациента
            if (!string.IsNullOrWhiteSpace(SearchPatientName.Text))
            {
                string search = SearchPatientName.Text.ToLower();
                cards = cards
                    .Where(c => c.PatientName != null && c.PatientName.ToLower().Contains(search))
                    .ToList();
            }

            // Фильтрация по диагнозу
            if (DiagnosisFilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedDiagnosis = selectedItem.Content.ToString();
                if (selectedDiagnosis != "Все")
                {
                    cards = cards
                        .Where(c => c.Diagnosis != null && c.Diagnosis.Equals(selectedDiagnosis, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            MedCardsGrid.ItemsSource = cards;
        }

        private void SearchPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateMedicalCards();
        }

        private void DiagnosisFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMedicalCards();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchPatientName.Text = string.Empty;
            DiagnosisFilterComboBox.SelectedIndex = 0; // "Все"
        }
    }
}
