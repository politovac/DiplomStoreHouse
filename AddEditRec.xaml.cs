﻿using DiplomStoreHouse.ModelDbase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    /// Логика взаимодействия для AddEditRec.xaml
    /// </summary>
    public partial class AddEditRec : Window
    {
        StoreHouseContext _db = new();
        Reception _reception;
        public AddEditRec()
        {
             InitializeComponent();
            _db.Items.Load();
            _db.Receptions.Load();

        }

        private void WinRec_Loaded(object sender, RoutedEventArgs e)
        {
            cbItem.ItemsSource = _db.Items.ToList();
            cbItem.DisplayMemberPath = "ItemId";
            if (Data.reception == null)
            {
                WinRec.Title = "Принятие товара";
                btnAddEditRec.Content = "Принять";
                _reception = new Reception();
                _reception.ReceiptDate = DateTime.Now;
            }
            else
            {
                WinRec.Title = "Изменить поставку";
                btnAddEditRec.Content = "Изменить";
                _reception = _db.Receptions.Find(Data.reception.ReceptionId);

            }
            WinRec.DataContext = _reception;
        }

        private void btnAddEditRec_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (cbItem.SelectedItem == null) errors.AppendLine("Выберите товар");
            if (dpData.SelectedDate == null)
            {
                errors.AppendLine("Пожалуйста, укажите дату.");
            }
            else if (dpData.SelectedDate.Value < DateTime.Today)
            {
                errors.AppendLine("Дата не может быть раньше сегодняшнего дня.");
            }
            if(tbInv.Text.IsNullOrEmpty()) errors.AppendLine("Укажите накладную");
            if (!string.IsNullOrWhiteSpace(tbQ.Text))
            {
                //Регулярное выражение: начинаются цифры, потом возможны пробелы, затем идут буквы
                string pattern = @"^\d+\s*[a-zA-Zа-яА-Я]{1,}$";

                if (!Regex.IsMatch(tbQ.Text, pattern))
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
               
                if (Data.reception == null)
                {
                    _db.Receptions.Add(_reception);
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
