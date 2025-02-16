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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Информационная_система_медицинской_клиники.Pages
{
    /// <summary>
    /// Логика взаимодействия для Instructions.xaml
    /// </summary>
    public partial class Instructions : Page
    {
        public Instructions()
        {
            InitializeComponent();
        }

        private void Patients_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Patients.xaml", UriKind.Relative));
        }

        private void Doctors_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Doctors.xaml", UriKind.Relative));
        }

        private void Appointments_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Appointments.xaml", UriKind.Relative));
        }

        private void Medical_cards_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Medical_Cards.xaml", UriKind.Relative));
        }

        private void Services_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Services.xaml", UriKind.Relative));
        }

        private void Schedules_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Schedules.xaml", UriKind.Relative));
        }

        private void Rooms_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Rooms.xaml", UriKind.Relative));
        }

        private void Invoices_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Invoices.xaml", UriKind.Relative));
        }

        private void Medications_btn_сlick(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Medications.xaml", UriKind.Relative));
        }
    }
}
