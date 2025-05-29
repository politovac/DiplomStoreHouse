using DiplomStoreHouse.ModelDbase;
using Microsoft.IdentityModel.Tokens;
using NPOI.OpenXmlFormats.Dml.Diagram;
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
    /// Логика взаимодействия для AddEditEmpl.xaml
    /// </summary>
    public partial class AddEditEmpl : Window
    {
        public AddEditEmpl()
        {
            InitializeComponent();
        }
        StoreHouseContext _db = new();
        Employee _employee;

        private void WinEmpl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Data.employee ==  null)
            {
                WinEmpl.Title = "Добавление сотрудника";
                btnUse.Content = "Добавить";
                _employee = new Employee();
            }
            else
            {
                WinEmpl.Title = "Изменение данных";
                btnUse.Content = "Обновить";
                _employee = _db.Employees.Find(Data.employee.EmployeeId);
            }
            WinEmpl.DataContext = _employee;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUse_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            // Проверка фамилии
            if (tbLastName.Text.IsNullOrEmpty())
            {
                errors.AppendLine("Введите фамилию нового сотрудника");
            }
            else if (!Regex.IsMatch(tbLastName.Text, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("Фамилия должна содержать только буквы.");
            }

            // Проверка имени
            if (tbFirstName.Text.IsNullOrEmpty())
            {
                errors.AppendLine("Укажите имя нового сотрудника");
            }
            else if (!Regex.IsMatch(tbFirstName.Text, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("Имя должно содержать только буквы.");
            }

            // Проверка должности
            if (tbPosititon.Text.IsNullOrEmpty())
            {
                errors.AppendLine("Поставьте на должность");
            }
            else if (!Regex.IsMatch(tbPosititon.Text, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("Должность должна содержать только буквы.");
            }

            // Проверка почты
            if (tbEmail.Text.IsNullOrEmpty())
            {
                errors.AppendLine("Введите почту сотрудника");
            }
            else if (!Regex.IsMatch(tbEmail.Text, @"^[a-zA-Z0-9]{4,}@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$"))
            {
                errors.AppendLine("Введите корректный адрес электронной почты.");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                if (Data.employee == null)
                {
                    _db.Employees.Add(_employee);
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
    }
}
