using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Appointments : Page
    {
        public Appointments()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAppointments();
        }

        private void UpdateAppointments()
        {
            if (AppointmentsGrid == null || SearchPatientName == null || FilterStatusComboBox == null)
                return;

            var context = Medical_ClinicEntities.GetContext();
            var appointments = context.Appointments.ToList();

            // Фильтрация по имени пациента
            if (!string.IsNullOrWhiteSpace(SearchPatientName.Text))
            {
                string search = SearchPatientName.Text.ToLower();
                appointments = appointments
                    .Where(a => a.PatientName != null && a.PatientName.ToLower().Contains(search))
                    .ToList();
            }

            // Фильтрация по статусу
            if (FilterStatusComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string status = selectedItem.Content.ToString();
                if (status != "Все")
                {
                    appointments = appointments
                        .Where(a => a.Statuss != null && a.Statuss.ToLower() == status.ToLower())
                        .ToList();
                }
            }

            AppointmentsGrid.ItemsSource = appointments;
        }

        private void SearchPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAppointments();
        }

        private void FilterStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAppointments();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchPatientName.Text = string.Empty;
            FilterStatusComboBox.SelectedIndex = 0;
        }
    }
}
