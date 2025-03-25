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

        public Program(bool isAdmin) : this() // Вызов конструктора по умолчанию
        {
            // Настройка видимости кнопок в зависимости от роли пользователя
            if (!isAdmin)
            {
                create_record.IsEnabled = false;
                delete_record.IsEnabled = false;
                Update_table.IsEnabled = false;
            }

            // Выводим сообщение о успешной авторизации
            MessageBox.Show(isAdmin ? "Вы успешно авторизованы в качестве администратора." : "Вы успешно авторизованы в качестве стороннего пользователя.");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void Patients_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Patients.xaml", UriKind.Relative));
        }

        private void Doctors_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Doctors.xaml", UriKind.Relative));
        }

        private void Appointments_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Appointments.xaml", UriKind.Relative));
        }

        private void Medical_cards_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Medical_Cards.xaml", UriKind.Relative));
        }

        private void Services_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Services.xaml", UriKind.Relative));
        }

        private void Schedules_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Schedules.xaml", UriKind.Relative));
        }

        private void Rooms_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Rooms.xaml", UriKind.Relative));
        }

        private void Invoices_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Invoices.xaml", UriKind.Relative));
        }

        private void Medications_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Medications.xaml", UriKind.Relative));
        }

        private void Instructions_btn_click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("Pages/Instructions.xaml", UriKind.Relative));
        }
    }
}
