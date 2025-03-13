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
    /// Логика взаимодействия для Rooms.xaml
    /// </summary>
    public partial class Rooms : Page
    {
        public Rooms()
        {
            InitializeComponent();
            RoomsGrid.ItemsSource = Medical_ClinicEntities.GetContext().Rooms.ToList();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
