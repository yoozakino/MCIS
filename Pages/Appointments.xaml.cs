﻿using System;
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
    /// Логика взаимодействия для Appointments.xaml
    /// </summary>
    public partial class Appointments : Page
    {
        public Appointments()
        {
            InitializeComponent();
            AppointmentsGrid.ItemsSource = Medical_ClinicEntities1.GetContext().Appointments.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
