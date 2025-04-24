using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Schedules : Page
    {
        public Schedules()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData(); 
        }

        private void LoadData()
        {
            if (SchedulesGrid == null)
                return;

            string searchText = SearchDoctorName?.Text?.Trim().ToLower() ?? "";
            string selectedDay = (FilterDayComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            var schedules = Medical_ClinicEntities.GetContext().Schedules.ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                schedules = schedules
                    .Where(s => s.DoctorName != null && s.DoctorName.ToLower().Contains(searchText))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(selectedDay) && selectedDay != "Все")
            {
                schedules = schedules
                    .Where(s => s.Day_of_week == selectedDay)
                    .ToList();
            }

            SchedulesGrid.ItemsSource = schedules;
        }

        private void SearchDoctorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData();
        }

        private void FilterDayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchDoctorName.Text = "";
            FilterDayComboBox.SelectedIndex = 0;
        }
    }
}
