using DiplomStoreHouse.ModelDbase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    /// Логика взаимодействия для AddEditShip.xaml
    /// </summary>
    public partial class AddEditShip : Window
    {
        StoreHouseContext _db = new();
        Shipment _shipment;
        public AddEditShip()
        {
            InitializeComponent();
            _db.Items.Load();
            _db.Shipments.Load();
        }

        private void WinShip_Loaded(object sender, RoutedEventArgs e)
        {
            cbItem.ItemsSource = _db.Items.ToList();
            cbItem.DisplayMemberPath = "ItemId";
            

            if (Data.shipment == null)
            {
                WinShip.Title = "Отгрузка";
                btnAddEditShip.Content = "Добавить";
                _shipment = new Shipment();
                _shipment.ExpectedDate = DateTime.Now;
            }
            else
            {
                WinShip.Title = "Изменение данных";
                btnAddEditShip.Content = "Обновить";
                _shipment = _db.Shipments.Find(Data.shipment.ShipmentId);
            }
            WinShip.DataContext = _shipment;
        }

        private void btnAddEditShip_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (cbItem.SelectedItem == null) errors.AppendLine("Выберите товар");
            if (dpData.SelectedDate == null)
            {
                errors.AppendLine("Пожалуйста, укажите дату проверки.");
            }
            else if (dpData.SelectedDate.Value < DateTime.Today)
            {
                errors.AppendLine("Дата проверки не может быть раньше сегодняшнего дня.");
            }
            if (tbClient.Text.IsNullOrEmpty()) errors.AppendLine("Укажите клиента");
            if (tbPoint.Text.IsNullOrEmpty()) errors.AppendLine("Введите адрес доставки");
            if (tbStatus.Text.IsNullOrEmpty()) errors.AppendLine("Укажите статус");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {               

                if (Data.shipment == null)
                {
                    _db.Shipments.Add(_shipment);
                    _db.SaveChanges();
                }
                else
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
