using DiplomStoreHouse.ModelDbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для EditLoc.xaml
    /// </summary>
    public partial class EditLoc : Window
    {
        StoreHouseContext _db = new();
        Location _location;
        public EditLoc()
        {
            InitializeComponent();
        }

        private void WinLoc_Loaded(object sender, RoutedEventArgs e)
        {
            if (Data.location != null)
            {
                WinLoc.Title = "Отредактировать места хранения";
                btnEditLoc.Content = "Изменить";
                _location = _db.Locations.Find(Data.location.LocationId);
            }
            WinLoc.DataContext = _location;
        }

        private void btnEditLoc_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(tbDes.Text))
            {
                if (tbDes.Text.Length < 10)
                {
                    errors.AppendLine("Описание должно содержать минимум 10 символов.");
                }
                else if (tbDes.Text.Length > 200)
                {
                    errors.AppendLine("Максимальная длина описания — 200 символов.");
                }
            }

            
            if (!string.IsNullOrWhiteSpace(tbMax.Text))
            {
                //Регулярное выражение: начинаются цифры, потом возможны пробелы, затем идут буквы
                string pattern = @"^\d+\s*[a-zA-Zа-яА-Я]{1,}$"; 

                if (!Regex.IsMatch(tbMax.Text, pattern))
                {
                    errors.AppendLine("Неверный формат вместимости. Например, правильный формат: '100 кг', '5 литров'.");
                }
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                if (Data.location != null)
                {
                    _db.SaveChanges();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}\n\nПодробная информация:\n{ex.StackTrace}\n\nВнутренняя ошибка: {ex.InnerException?.Message ?? "Нет внутренней ошибки"}");
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
