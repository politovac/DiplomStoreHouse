using DiplomStoreHouse.ModelDbase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NPOI.OpenXmlFormats.Spreadsheet;
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
    /// Логика взаимодействия для AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        StoreHouseContext _db = new();
        TaskAssignment _taskAssignment;
        public AddTask()
        {
            InitializeComponent();
            _db.Employees.Load();
            _db.Operations.Load();
            _db.TaskAssignments.Load();
        }

        private void WinTask_Loaded(object sender, RoutedEventArgs e)
        {
            cbEmpl.ItemsSource = _db.Employees.ToList();
            cbEmpl.DisplayMemberPath = "LastName";
            cbEmpl.SelectedValuePath = "EmployeeId";

            cbOper.ItemsSource = _db.Operations.ToList();
            cbOper.DisplayMemberPath = "TypeOperation";
            cbOper.SelectedValuePath = "OperationId";

            if(Data.assignment == null)
            {
                WinTask.Title = "Новое задание";
                btnTaskAdd.Content = "Назначить";
                _taskAssignment = new TaskAssignment();
                _taskAssignment.StartDate = DateTime.Now;
                _taskAssignment.EndDate = DateTime.Now;
            }
            WinTask.DataContext = _taskAssignment;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnTaskAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (cbEmpl.SelectedItem == null) errors.AppendLine("Укажите ответственного сотрудника");
            if (cbOper.SelectedItem == null) errors.AppendLine("Выберите операцию");

            if (string.IsNullOrWhiteSpace(tbPrior.Text))
            {
                errors.AppendLine("Укажите приоритет от 1 до 5");
            }
            else if (tbPrior.Text.Length != 1 || !char.IsDigit(tbPrior.Text[0]) || tbPrior.Text[0] < '1' || tbPrior.Text[0] > '5')
            {
                errors.AppendLine("Приоритет должен содержать ровно один символ от 1 до 5.");
            }

            if (dpDStart.SelectedDate == null)
            {
                errors.AppendLine("Пожалуйста, укажите дату проверки.");
            }
            else if (dpDStart.SelectedDate.Value < DateTime.Today)
            {
                errors.AppendLine("Дата проверки не может быть раньше сегодняшнего дня.");
            }
            if (dpDEnd.SelectedDate == null)
            {
                errors.AppendLine("Пожалуйста, укажите дату проверки.");
            }
            else if (dpDEnd.SelectedDate.Value < DateTime.Today)
            {
                errors.AppendLine("Дата проверки не может быть раньше сегодняшнего дня.");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                if (cbEmpl.SelectedValue != null)
                {
                    if (int.TryParse(cbEmpl.SelectedValue.ToString(), out int idEmpl))
                    {
                        _taskAssignment.EmployeeId = idEmpl;
                    }
                    else
                    {
                        MessageBox.Show("Значение некоректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (cbOper.SelectedValue != null)
                {
                    if (int.TryParse(cbOper.SelectedValue.ToString(), out int idOper))
                    {
                        _taskAssignment.OperationId = idOper;
                    }
                    else
                    {
                        MessageBox.Show("Значение некоректно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (Data.assignment == null)
                {
                    _db.TaskAssignments.Add(_taskAssignment);
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
