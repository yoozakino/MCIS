using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Информационная_система_медицинской_клиники.Pages
{
    public partial class Rooms : Page
    {
        public Rooms()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var rooms = Medical_ClinicEntities.GetContext().Rooms.ToList();

            string roomNumberSearch = SearchRoomNumber?.Text?.Trim().ToLower() ?? "";
            string descriptionSearch = SearchDescription?.Text?.Trim().ToLower() ?? "";

            if (!string.IsNullOrWhiteSpace(roomNumberSearch))
            {
                rooms = rooms.Where(r => r.RoomNumber != null && r.RoomNumber.ToLower().Contains(roomNumberSearch)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(descriptionSearch))
            {
                rooms = rooms.Where(r => r.Descriptionn != null && r.Descriptionn.ToLower().Contains(descriptionSearch)).ToList();
            }

            RoomsGrid.ItemsSource = rooms;
        }

        private void SearchFiltersChanged(object sender, TextChangedEventArgs e)
        {
            LoadData();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchRoomNumber.Text = "";
            SearchDescription.Text = "";
        }
    }
}
