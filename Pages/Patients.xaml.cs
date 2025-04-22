using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Patients : Page
    {


        public Patients()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePatients();
        }

        private void UpdatePatients()
        {
            if (PatientsGrid == null || SearchPatientName == null || GenderFilterComboBox == null)
                return;

            var context = Medical_ClinicEntities.GetContext();
            var patients = context.Patients.ToList();

            // Поиск по имени
            if (!string.IsNullOrWhiteSpace(SearchPatientName.Text))
            {
                string search = SearchPatientName.Text.ToLower();
                patients = patients
                    .Where(p => p.FullName != null && p.FullName.ToLower().Contains(search))
                    .ToList();
            }

            // Фильтрация по полу
            if (GenderFilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string gender = selectedItem.Content.ToString();
                if (gender != "Все")
                {
                    patients = patients
                        .Where(p => p.Gender != null && p.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            PatientsGrid.ItemsSource = patients;
        }

        private void SearchPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePatients();
        }

        private void GenderFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePatients();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchPatientName.Text = string.Empty;
            GenderFilterComboBox.SelectedIndex = 0;
        }
    }
}
