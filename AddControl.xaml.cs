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
    /// Логика взаимодействия для AddControl.xaml
    /// </summary>
    public partial class AddControl : Window
    {
        StoreHouseContext _db = new();
        QualityControl _qualityControl;
        public AddControl()
        {
            InitializeComponent();
            _db.Items.Load();
            _db.Employees.Load();
            _db.QualityControls.Load();
        }

        private void WinControl_Loaded(object sender, RoutedEventArgs e)
        {
            cbItem.ItemsSource = _db.Items.ToList();
            cbItem.DisplayMemberPath = "Name";
            cbItem.SelectedValuePath = "ItemId";

            cbEmpl.ItemsSource = _db.Employees.ToList();
            cbEmpl.DisplayMemberPath = "LastName";
            cbEmpl.SelectedValuePath = "EmployeeId";

            if (Data.qualityControl == null)
            {
                WinControl.Title = "Добаваить контроль";
                btnAdd.Content = "Добавить";
                _qualityControl = new QualityControl();
                _qualityControl.ControlDate = DateTime.Now;
            }
            WinControl.DataContext = _qualityControl;

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (cbEmpl.SelectedItem == null) errors.Append("Укажите ответственного сотрудника");
            if (cbItem.SelectedItem == null) errors.Append("Выберите товар для проверки");

            if (dpDate.SelectedDate == null)
            {
                errors.Append("Пожалуйста, укажите дату проверки.");
            }
            else if (dpDate.SelectedDate.Value < DateTime.Today)
            {
                errors.Append("Дата проверки не может быть раньше сегодняшнего дня.");
            }

            if (tbRes.Text.IsNullOrEmpty()) errors.Append("Введите результат проверки");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                if (cbItem.SelectedValue != null)
                {
                    if (int.TryParse(cbItem.SelectedValue.ToString(), out int idItem))
                    {
                        _qualityControl.ItemId = idItem;
                    }
                    else
                    {
                        MessageBox.Show("Значение некоректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (cbEmpl.SelectedValue != null)
                {
                    if (int.TryParse(cbEmpl.SelectedValue.ToString(), out int idEmpl))
                    {
                        _qualityControl.ResponsibleEmployee = idEmpl;
                    }
                    else
                    {
                        MessageBox.Show("Значение некоректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (Data.qualityControl == null)
                {
                    _db.QualityControls.Add(_qualityControl);
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
