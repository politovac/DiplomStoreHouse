using DiplomStoreHouse.ModelDbase;
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
    /// Логика взаимодействия для EditWareDiv.xaml
    /// </summary>
    public partial class EditWareDiv : Window
    {
        StoreHouseContext _db = new();
        WarehouseDivision _warehouseDivision;
        public EditWareDiv()
        {
            InitializeComponent();
        }

        private void WinWare_Loaded(object sender, RoutedEventArgs e)
        {
            if (Data.warehouseDivision != null)
            {
                WinWare.Title = "Заменить руководителя";
                btnEditWare.Content = "Заменить";
                _warehouseDivision = _db.WarehouseDivisions.Find(Data.warehouseDivision.DivisionId);
            }
            WinWare.DataContext = _warehouseDivision;
        }

        private void btnEditWare_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (tbMan.Text.IsNullOrEmpty()) errors.Append("Укажите руководителя верно");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                if (Data.warehouseDivision != null)
                {
                    _db.SaveChanges();
                }
                
                this.Close();
            }
            catch (Exception ex)
            {
                // Выводим полное сообщение об ошибке, включая внутреннее исключение
                MessageBox.Show($"Произошла ошибка: {ex.Message}\n\nПодробная информация:\n{ex.StackTrace}\n\nВнутренняя ошибка: {ex.InnerException?.Message ?? "Нет внутренней ошибки"}");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
