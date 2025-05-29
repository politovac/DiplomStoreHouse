using DiplomStoreHouse.ModelDbase;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace DiplomStoreHouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(string role)
        {
            InitializeComponent();
            if (role != "admin")
            {

            }
        }

        private void SaveEx_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Сохранить данные в Excel"
            };

            if (saveDialog.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(saveDialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet = workbook.CreateSheet("Сотрудники");

                    // Добавляем заголовки
                    IRow headerRow = sheet.CreateRow(0);
                    headerRow.CreateCell(0).SetCellValue("Фамилия");
                    headerRow.CreateCell(1).SetCellValue("Имя");
                    headerRow.CreateCell(2).SetCellValue("Должность");
                    headerRow.CreateCell(3).SetCellValue("Email");
                    // Получаем данные из базы данных
                    using (StoreHouseContext _db = new StoreHouseContext())
                    {
                        var employees = _db.Employees.ToList();

                        // Добавляем данные
                        int rowIndex = 1;
                        foreach (var employee in employees)
                        {
                            IRow row = sheet.CreateRow(rowIndex++);
                            row.CreateCell(0).SetCellValue(employee.LastName);
                            row.CreateCell(1).SetCellValue(employee.FirstName);
                            row.CreateCell(2).SetCellValue(employee.Posititon);
                            row.CreateCell(3).SetCellValue(employee.Email);
                        }
                    }

                    workbook.Write(fs);
                }
            }
        }
        private void mainWin_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInDataGrid();
            LoadInListView();
        }
        void LoadInDataGrid()
        {
            using (StoreHouseContext _db = new StoreHouseContext())
            {
                int index = dGridEmp.SelectedIndex;
                index = dGridControl.SelectedIndex;
                index = dGridLocation.SelectedIndex;
                index = dGridReception.SelectedIndex;
                index = dGridTransfer.SelectedIndex;
                index = dGridShipment.SelectedIndex;
                index = dGridOperation.SelectedIndex;
                index = dGridTask.SelectedIndex;

                _db.Employees.Load();
                _db.QualityControls.Load();
                _db.Locations.Load();
                _db.Receptions.Load();
                _db.Transfers.Load();
                _db.Shipments.Load();
                _db.Operations.Load();
                _db.TaskAssignments.Load();
                _db.Items.Load();

                dGridEmp.ItemsSource = _db.Employees.ToList();
                dGridControl.ItemsSource = _db.QualityControls.ToList();
                dGridLocation.ItemsSource = _db.Locations.ToList();
                dGridReception.ItemsSource = _db.Receptions.ToList();
                dGridTransfer.ItemsSource = _db.Transfers.ToList();
                dGridShipment.ItemsSource = _db.Shipments.ToList();
                dGridOperation.ItemsSource = _db.Operations.ToList();
                dGridTask.ItemsSource = _db.TaskAssignments.ToList();

                if (index != -1)
                {
                    if (index == dGridEmp.Items.Count && index == dGridControl.Items.Count
                        && index == dGridLocation.Items.Count && index == dGridReception.Items.Count
                        && index == dGridTransfer.Items.Count && index == dGridShipment.Items.Count
                        && index == dGridOperation.Items.Count && index == dGridTask.Items.Count) index--;

                    dGridEmp.SelectedIndex = index;
                    dGridControl.SelectedIndex = index;
                    dGridLocation.SelectedIndex = index;
                    dGridReception.SelectedIndex = index;
                    dGridTransfer.SelectedIndex = index;
                    dGridShipment.SelectedIndex = index;
                    dGridOperation.SelectedIndex = index;
                    dGridTask.SelectedIndex = index;

                    dGridEmp.ScrollIntoView(dGridEmp.SelectedItem);
                    dGridControl.ScrollIntoView(dGridControl.SelectedItem);
                    dGridLocation.ScrollIntoView(dGridLocation.SelectedItem);
                    dGridReception.ScrollIntoView(dGridReception.SelectedItem);
                    dGridTransfer.ScrollIntoView(dGridTransfer.SelectedItem);
                    dGridShipment.ScrollIntoView(dGridShipment.SelectedItem);
                    dGridOperation.ScrollIntoView(dGridOperation.SelectedItem);
                    dGridTask.ScrollIntoView(dGridTask.SelectedItem);
                }
                dGridEmp.Focus();
            }
        }
        void LoadInListView()
        {
            using (StoreHouseContext _db = new StoreHouseContext())
            {
                _db.Items.Load();
                _db.WarehouseDivisions.Load();
                int index = lvItem.SelectedIndex;
                index = lvDivis.SelectedIndex;

                lvItem.ItemsSource = _db.Items.ToList();
                lvDivis.ItemsSource = _db.WarehouseDivisions.ToList();

                if (index != -1)
                {
                    if (index == lvItem.Items.Count && index == lvDivis.Items.Count) index--;
                    lvItem.SelectedIndex = index;
                    lvDivis.SelectedIndex = index;

                    lvItem.ScrollIntoView(lvItem.SelectedItem);
                    lvDivis.ScrollIntoView(lvDivis.SelectedItem);
                }
            }
        }

        //Сотрудники
        private void AddEmpl_Click(object sender, RoutedEventArgs e)
        {
            Data.employee = null;
            AddEditEmpl w = new();
            w.Owner = this;
            w.ShowDialog();
            LoadInDataGrid();
        }


        private void resetEmpl_Click(object sender, RoutedEventArgs e)
        {
            LoadInDataGrid();
        }
        private bool fix = false;
        private void dGridEmp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!fix && dGridEmp.SelectedItem != null)
            {
                fix = true;

                Data.employee = (Employee)dGridEmp.SelectedItem;
                AddEditEmpl w = new();
                w.Owner = this;
                w.ShowDialog();
                LoadInDataGrid();

                fix = false;
            }
        }
        private void DelEmpl_Click(object sender, RoutedEventArgs e)
        {
            if (dGridEmp.SelectedItem != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        //получаем текущую запись
                        Employee row = (Employee)dGridEmp.SelectedItem;
                        if (row != null)
                        {
                            using (StoreHouseContext db = new())
                            {
                                //удаляем запись
                                db.Employees.Remove(row);
                                db.SaveChanges();
                            }
                            LoadInDataGrid();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка удаления!", "Ошибка");
                    }
                }
                else
                    dGridEmp.Focus();
            }
            else MessageBox.Show("Нужно выбрать запись для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //Задачи
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            Data.assignment = null;
            AddTask w = new();
            w.Owner = this;
            w.ShowDialog();
            LoadInDataGrid();
        }

        private void calcTask_Click(object sender, RoutedEventArgs e)
        {
            using (StoreHouseContext _db = new StoreHouseContext())
            {
                var tasks = dGridTask.ItemsSource as IEnumerable<TaskAssignment>;

                if (tasks != null)
                {
                    // Подсчитываем количество выполненных заданий
                    int completedTasksCount = tasks.Count(t => t.EndDate.HasValue);

                    // Выводим результат в текстблок
                    answerTask.Text = $"{completedTasksCount}";
                }
                else
                {
                    answerTask.Text = "Нет данных для подсчета.";
                }
            }
        }

        //Отделы склада
        private void editWare_Click(object sender, RoutedEventArgs e)
        {
            if (lvDivis.SelectedItem != null)
            {
                Data.warehouseDivision = (WarehouseDivision)lvDivis.SelectedItem;
                EditWareDiv w = new();
                w.Owner = this;
                w.ShowDialog();
                LoadInListView();
            }
            else MessageBox.Show("Выберите элемент для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //Контроль качества
        private void AddControl_Click(object sender, RoutedEventArgs e)
        {
            Data.qualityControl = null;
            AddControl w = new();
            w.Owner = this;
            w.ShowDialog();
            LoadInDataGrid();
        }

        //Товары
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            Data.item = null;
            AEItem w = new();
            w.Owner = this;
            w.ShowDialog();
            LoadInListView();
        }
        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            if (lvItem.SelectedItem != null)
            {
                Data.item = (Item)lvItem.SelectedItem;
                AEItem w = new();
                w.Owner = this;
                w.ShowDialog();
                LoadInListView();
            }
            else MessageBox.Show("Выберите элемент для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void DelItem_Click(object sender, RoutedEventArgs e)
        {
            if (lvItem.SelectedItem != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Удалить товар?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        Item row = (Item)lvItem.SelectedItem;
                        if (row != null)
                        {
                            using (StoreHouseContext _db = new())
                            {
                                _db.Items.Remove(row);
                                _db.SaveChanges();
                            }
                            LoadInListView();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка удаления!", "Ошибка");
                    }
                }
                else lvItem.Focus();

            }
            else MessageBox.Show("Выберите элемент для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        //Место хранения
        private void editLocation_Click(object sender, RoutedEventArgs e)
        {
            if (dGridLocation.SelectedItem != null)
            {
                Data.location = (Location)dGridLocation.SelectedItem;
                EditLoc w = new();
                w.Owner = this;
                w.ShowDialog();
                LoadInDataGrid();
            }
            else MessageBox.Show("Выберите элемент для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        //Приёмка
        private void AddReception_Click(object sender, RoutedEventArgs e)
        {
            Data.reception = null;
            AddEditRec w = new();
            w.Owner = this;
            w.ShowDialog();
            LoadInDataGrid();
        }

        private void EditReception_Click(object sender, RoutedEventArgs e)
        {
            if (dGridReception.SelectedItem != null)
            {
                Data.reception = (Reception)dGridReception.SelectedItem;
                AddEditRec w = new();
                w.Owner = this;
                w.ShowDialog();
                LoadInDataGrid();
            }
            else MessageBox.Show("Выберите элемент для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        //Перемещение
        private void AddTransfer_Click(object sender, RoutedEventArgs e)
        {
            Data.transfer = null;
            AddTrans w = new();
            w.Owner = this;
            w.ShowDialog();
            LoadInDataGrid();
        }

        //Отгрузка
        private void AddShipment_Click(object sender, RoutedEventArgs e)
        {
            Data.shipment = null;
            AddShipment w = new();
            w.Owner = this;
            w.ShowDialog();
            LoadInDataGrid();
        }

        private void EditShipment_Click(object sender, RoutedEventArgs e)
        {
            if (dGridShipment.SelectedItem != null)
            {
                Data.shipment = (Shipment)dGridShipment.SelectedItem;
                AddShipment w = new();
                w.Owner = this;
                w.ShowDialog();
                LoadInDataGrid();
            }
            else MessageBox.Show("Выберите элемент для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void DelShipment_Click(object sender, RoutedEventArgs e)
        {
            if (dGridShipment.SelectedItem != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        //получаем текущую запись
                        Shipment row = (Shipment)dGridShipment.SelectedItem;
                        if (row != null)
                        {
                            using (StoreHouseContext db = new())
                            {
                                //удаляем запись
                                db.Shipments.Remove(row);
                                db.SaveChanges();
                            }
                            LoadInDataGrid();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка удаления!", "Ошибка");
                    }
                }
                else
                    dGridShipment.Focus();
            }
            else MessageBox.Show("Выберите элемент для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        //Дополнения
        private bool isDarkMode = false;
        private void darkLightmode_Click(object sender, RoutedEventArgs e)
        {
            string lightThemeHex = "#D2E3E9";
            string darkThemeHex = "#9B9B9B";

            var currentBackground = grid1.Background as SolidColorBrush;

            // Проверяем, какая сейчас тема
            if (isDarkMode)
            {
                SetBackground(lightThemeHex);
                isDarkMode = false;
            }
            else
            {
                SetBackground(darkThemeHex);
                isDarkMode = true;
            }
        }
        private void SetBackground(string hexColor)
        {
            grid1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid3.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid4.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid5.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid6.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid7.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid8.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid9.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid10.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid11.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid12.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid13.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid14.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            grid15.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));

            darkLightmode.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            lvItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            lvDivis.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            darkLightmode.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            SaveEx.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            SaveEx.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));

            tab1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab3.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab4.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab5.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab6.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab7.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab8.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab9.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab10.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab11.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab12.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab13.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab14.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab15.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab16.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab17.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            

            tab1.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab2.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab3.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab4.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab5.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab6.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab7.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab8.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab9.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab10.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab11.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab12.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab13.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab15.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab16.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            tab17.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            

            resetEmpl.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            searchWare.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            searchItem.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            resetEmpl.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            searchWare.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            searchItem.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            searchEmp.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            searchEmp.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            
        }

        private void searchEmp_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isHaveText = string.IsNullOrWhiteSpace(searchEmp.Text);
            if (isHaveText)
            {
                SearchEmpPlaceHolder.Visibility = Visibility.Visible;
            }
            else
            {
                SearchEmpPlaceHolder.Visibility = Visibility.Hidden;
            }
            using (StoreHouseContext _db = new())
            {
                try
                {
                    if (!isHaveText)
                    {
                        var find = _db.Employees.Where(p => p.FirstName.Contains(searchEmp.Text));
                        if (find.Any())
                        {
                            dGridEmp.ItemsSource = find.ToList();//Отображение найденых элементов
                        }

                        else
                        {
                            dGridEmp.ItemsSource = null; // Очистка списка, так как нет совпадений
                        }
                    }
                    else
                    {
                        LoadInDataGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Возникла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void searchWare_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isHaveText = string.IsNullOrWhiteSpace(searchWare.Text);
            if (isHaveText)
            {
                SearchWare.Visibility = Visibility.Visible;
            }
            else
            {
                SearchWare.Visibility = Visibility.Hidden;
            }
            using (StoreHouseContext _db = new())
            {
                try
                {
                    if (!isHaveText)
                    {
                        var find = _db.WarehouseDivisions.Where(p => p.Name.Contains(searchWare.Text));
                        if (find.Any())
                        {
                            lvDivis.ItemsSource = find.ToList();//Отображение найденых элементов
                        }

                        else
                        {
                            lvDivis.ItemsSource = null; // Очистка списка, так как нет совпадений
                        }
                    }
                    else
                    {
                        LoadInListView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Возникла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void searchItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            var isHaveText = string.IsNullOrWhiteSpace(searchItem.Text);
            if (isHaveText)
            {
                SearchItem.Visibility = Visibility.Visible;
            }
            else
            {
                SearchItem.Visibility = Visibility.Hidden;
            }
            using (StoreHouseContext _db = new())
            {
                try
                {
                    if (!isHaveText)
                    {
                        var find = _db.Items.Where(p => p.Name.Contains(searchItem.Text));
                        if (find.Any())
                        {
                            lvItem.ItemsSource = find.ToList();//Отображение найденых элементов
                        }

                        else
                        {
                            lvItem.ItemsSource = null; // Очистка списка, так как нет совпадений
                        }
                    }
                    else
                    {
                        LoadInListView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Возникла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void lvDivis_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Получаем ScrollViewer из ListView
            var scrollViewer = GetScrollViewer(lvDivis);
            if (scrollViewer != null)
            {
                // Устанавливаем скорость прокрутки (например, 2 элемента за раз)
                double scrollAmount = e.Delta > 0 ? -2 : 2;
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + scrollAmount);
                e.Handled = true;
            }
        }

        private ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            if (obj is ScrollViewer)
            {
                return (ScrollViewer)obj;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var result = GetScrollViewer(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}