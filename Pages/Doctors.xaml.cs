using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Doctors : Page
    {
        public Doctors()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSpecializations();
            UpdateDoctors();
        }

        private void LoadSpecializations()
        {
            var context = Medical_ClinicEntities.GetContext();
            var specializations = context.Doctors
                                         .Where(d => d.Specialization != null)
                                         .Select(d => d.Specialization)
                                         .Distinct()
                                         .OrderBy(s => s)
                                         .ToList();

            FilterSpecializationComboBox.Items.Clear();

            var allItem = new ComboBoxItem { Content = "Все", IsSelected = true };
            FilterSpecializationComboBox.Items.Add(allItem);

            foreach (var spec in specializations)
            {
                FilterSpecializationComboBox.Items.Add(new ComboBoxItem { Content = spec });
            }
        }

        private void UpdateDoctors()
        {
            if (DoctorsGrid == null || SearchDoctorName == null || FilterSpecializationComboBox == null)
                return;

            var context = Medical_ClinicEntities.GetContext();
            var doctors = context.Doctors.ToList();

            if (!string.IsNullOrWhiteSpace(SearchDoctorName.Text))
            {
                string search = SearchDoctorName.Text.ToLower();
                doctors = doctors
                    .Where(d => d.FullName != null && d.FullName.ToLower().Contains(search))
                    .ToList();
            }

            if (FilterSpecializationComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string specialization = selectedItem.Content.ToString();
                if (specialization != "Все")
                {
                    doctors = doctors
                        .Where(d => d.Specialization != null && d.Specialization.ToLower() == specialization.ToLower())
                        .ToList();
                }
            }

            DoctorsGrid.ItemsSource = doctors;
        }

        private void SearchDoctorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDoctors();
        }

        private void FilterSpecializationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDoctors();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchDoctorName.Text = string.Empty;
            if (FilterSpecializationComboBox.Items.Count > 0)
                FilterSpecializationComboBox.SelectedIndex = 0;
        }
    }
}
