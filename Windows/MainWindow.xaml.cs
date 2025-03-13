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

namespace Информационная_система_медицинской_клиники.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private SignUpWindow signUpWindow = null;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Sign_up_btn_Click(Sign_up_btn, null);
            }
            else if (e.Key == Key.Escape)
            {
                exit_btn_Click(exit_btn, null);
            }
        }
        private void Sign_up_btn_Click(object sender, RoutedEventArgs e)
        {
            if (signUpWindow == null || !signUpWindow.IsLoaded)
            {
                // Создаем новое окно, если оно еще не открыто
                signUpWindow = new Windows.SignUpWindow();
                signUpWindow.Show();
            }

            else
            {
                // Если окно уже открыто, активируем его
                signUpWindow.Activate();
                MessageBox.Show("Окно входа уже открыто");
            }



        }

        private void exit_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void register_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
