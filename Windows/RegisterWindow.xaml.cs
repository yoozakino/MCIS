using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Информационная_система_медицинской_клиники.Windows
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

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

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            string userLogin = login.Text;
            string userPassword = password.Text;

            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(userPassword))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми.");
                return;
            }

            var context = Medical_ClinicEntities.GetContext();
            try
            {
                if (context.Users.Any(u => u.User_login == userLogin))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return;
                }

                string hashedPassword = HashPassword(userPassword);

                var user = new Users
                {
                    User_login = userLogin,
                    User_password = hashedPassword
                };

                context.Users.Add(user);
                context.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
                Medical_ClinicEntities.ResetContext();
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}