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
using System.Windows.Shapes;

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
            // Получаем логин и пароль из текстовых полей
            string userLogin = login.Text; // Обращаемся к свойству Text элемента login (TextBox)
            string userPassword = password.Text; // Обращаемся к свойству Text элемента password (TextBox)

            // Проверяем, что логин и пароль не пустые
            if (string.IsNullOrEmpty(userLogin) || string.IsNullOrEmpty(userPassword))
            {
                MessageBox.Show("Логин и пароль не могут быть пустыми.");
                return;
            }

            // Используем контекст базы данных
            using (var context = Medical_ClinicEntities1.GetContext())
            {
                // Проверяем, существует ли пользователь с таким логином
                if (context.Users.Any(u => u.User_login == userLogin))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return;
                }

                // Хэшируем пароль
                string hashedPassword = HashPassword(userPassword);

                // Создаем нового пользователя
                var user = new Users
                {
                    User_login = userLogin,
                    User_password = hashedPassword
                };

                // Добавляем пользователя в таблицу Users
                context.Users.Add(user);

                // Сохраняем изменения в базе данных
                context.SaveChanges();
            }

            // Уведомляем пользователя об успешной регистрации
            MessageBox.Show("Регистрация прошла успешно!");
            this.Close();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Преобразуем пароль в массив байтов и хэшируем
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Преобразуем массив байтов в строку в hex-формате
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
