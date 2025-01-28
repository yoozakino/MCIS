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

namespace Информационная_система_медицинской_клиники
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private string AdminLogin = "ADMIN";
        private string AdminPassword = "admin";

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                enter.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text == "" && password.Text == "")
            {
                MessageBox.Show("Значения полей пустые");
            }


            else if (login.Text == AdminLogin && password.Text == AdminPassword)
            {
                //MessageBox.Show("Ну наконец-то, тупорылая обезьяна");
                Program program = new Program();

                program.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != program)
                    {
                        window.Close();
                    }
                }
            }

            else
            {
                MessageBox.Show("Неверные логин или пароль");
            }

            
        }
    }
}
