using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private string AdminLogin = "ADMIN";
        private readonly string AdminPasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";

        
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

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Преобразуем пароль в массив байт и вычисляем хэш
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Преобразуем массив байт в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string hashedEnteredPassword = HashPassword(password.Password);

            if (login.Text == "" && password.Password == "")
            {
                MessageBox.Show("Значения полей пустые");
            }


            else if (login.Text == AdminLogin && hashedEnteredPassword == AdminPasswordHash)
            {
                Program program = new Windows.Program();

                program.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != program)
                    {
                        window.Close();
                    }
                }

                MessageBox.Show("Вы успешно авторизованы в качестве администратора");
            }

            else
            {
                MessageBox.Show("Неверные логин или пароль");
            }

            
        }
    }
}
