using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Информационная_система_медицинской_клиники.Windows
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userLogin = login.Text;
            string userPassword = password.Password;

            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(userPassword))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми.");
                return;
            }

            var context = Medical_ClinicEntities.GetContext();
            try
            {
                var user = context.Users.FirstOrDefault(u => u.User_login == userLogin);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с таким логином не найден.");
                    return;
                }

                string hashedPassword = HashPassword(userPassword);

                if (user.User_password != hashedPassword)
                {
                    MessageBox.Show("Невозможно авторизоваться. Возможно, неверны логин или пароль.");
                    return;
                }

                // Проверяем, является ли пользователь администратором (UserID == 1)
                bool isAdmin = user.UserID == 1;

                Program programWindow = new Program(isAdmin);
                programWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}");
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