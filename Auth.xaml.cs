using DiplomStoreHouse.ModelDbase;
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

namespace DiplomStoreHouse
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBox_Username.Text;
            string password = TextBox_Password.Password;

            if (ValidateUser(username, password))
            {
                MainWindow mainWindow = new MainWindow(CurrentRole);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private static string CurrentRole { get; set; }
        private bool ValidateUser(string username, string password)
        {
            using (StoreHouseContext db = new StoreHouseContext())
            {

                var user = db.Users.SingleOrDefault(u => u.UserName == username && u.Password == password);

                if (user != null)
                {
                    CurrentRole = user.Role;
                }

                return user != null;
            }
        }

        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox_Username.Text))
            {
                UsernamePlaceholder.Visibility = Visibility.Visible; // Показать плейсхолдер, если TextBox пуст
            }
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox_Password.Password))
            {
                PasswordPlaceholder.Visibility = Visibility.Visible; // Показать плейсхолдер, если TextBox пуст
            }
        }

    }
}
