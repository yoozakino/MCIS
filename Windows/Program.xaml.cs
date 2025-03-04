using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace Информационная_система_медицинской_клиники.Windows
{
    /// <summary>
    /// Логика взаимодействия для Program.xaml
    /// </summary>
    public partial class Program : Window
    {
        public Program()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void Patients_btn_сlick(object sender, RoutedEventArgs e)
        {
            Patients_btn.IsEnabled = false;
            Doctors_btn.IsEnabled = true;
            Appointments_btn.IsEnabled = true;
            Medical_cards_btn.IsEnabled = true;
            Services_btn.IsEnabled = true;
            Schedules_btn.IsEnabled = true;
            Rooms_btn.IsEnabled = true;
            Invoices_btn.IsEnabled = true;
            Medications_btn.IsEnabled = true;
            Instructions_btn.IsEnabled = true;
            MainFrame.Navigate(new Uri("Pages/Patients.xaml", UriKind.Relative));
        }

        private void Doctors_btn_сlick(object sender, RoutedEventArgs e)
        {
            Patients_btn.IsEnabled = true;
            Doctors_btn.IsEnabled = false;
            Appointments_btn.IsEnabled = true;
            Medical_cards_btn.IsEnabled = true;
            Services_btn.IsEnabled = true;
            Schedules_btn.IsEnabled = true;
            Rooms_btn.IsEnabled = true;
            Invoices_btn.IsEnabled = true;
            Medications_btn.IsEnabled = true;
            Instructions_btn.IsEnabled = true;
            MainFrame.Navigate(new Uri("Pages/Doctors.xaml", UriKind.Relative));
        }

        

    }
}
