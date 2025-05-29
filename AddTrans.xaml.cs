using DiplomStoreHouse.ModelDbase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для AddTrans.xaml
    /// </summary>
    public partial class AddTrans : Window
    {
        StoreHouseContext _db = new();
        Transfer _transfer;
        public AddTrans()
        {
            InitializeComponent();
            _db.Locations.Load();
            _db.Transfers.Load();
            _db.Items.Load();
        }
        private void WinTrans_Loaded(object sender, RoutedEventArgs e)
        {
            cbItem.ItemsSource = _db.Items.ToList();
            cbItem.DisplayMemberPath = "ItemId";

            cbStart.ItemsSource = _db.Locations.ToList();
            cbStart.DisplayMemberPath = "LocationId";

            cbEnd.ItemsSource = _db.Locations.ToList();
            cbEnd.DisplayMemberPath = "LocationId";
            if (Data.transfer == null)
            {
                WinTrans.Title = "Создание перемещения";
                btnAddTrans.Content = "Выполнить";
                _transfer = new Transfer();
                _transfer.TransferDate = DateTime.Now;
            }
            WinTrans.DataContext = _transfer;
        }

        private void btnAddTrans_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (cbStart.SelectedItem == null) errors.AppendLine("Место хранения в начале не выбрано.");

            // Проверка места хранения в конце
            if (cbEnd.SelectedItem == null) errors.AppendLine("Место хранения в конце не выбрано.");

            // Проверка мест хранения начала и конца
            if (cbStart.SelectedItem != null && cbEnd.SelectedItem != null &&
                cbStart.SelectedItem.Equals(cbEnd.SelectedItem))
            {
                errors.AppendLine("Место хранения в начале и в конце не могут совпадать.");
            }
            if (dpData.SelectedDate == null)
            {
                errors.AppendLine("Пожалуйста, укажите дату проверки.");
            }
            else if (dpData.SelectedDate.Value < DateTime.Today)
            {
                errors.AppendLine("Дата проверки не может быть раньше сегодняшнего дня.");
            }
            if (!string.IsNullOrWhiteSpace(tbQ.Text))
            {
                //Регулярное выражение: начинаются цифры, потом возможны пробелы, затем идут буквы
                string pattern = @"^\d+\s*[a-zA-Zа-яА-Я]{1,}$";

                if (!Regex.IsMatch(tbQ.Text, pattern))
                {
                    errors.AppendLine("Неверный формат вместимости. Например, правильный формат: '100 кг', '5 литров'.");
                }
            }
            try
            {
                if (Data.transfer == null)
                {
                    _db.Transfers.Add(_transfer);
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
