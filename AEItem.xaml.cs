using DiplomStoreHouse.ModelDbase;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AEItem.xaml
    /// </summary>
    public partial class AEItem : Window
    {
        StoreHouseContext _db = new();
        Item _item;
        OpenFileDialog open = new OpenFileDialog();
        public AEItem()
        {
            InitializeComponent();
        }

        private void WinItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (Data.item == null)
            {
                WinItem.Title = "Добавление товара";
                btnAddItem.Content = "Добавить";
                _item = new Item();
            }
            else
            {
                WinItem.Title = "Изменение товара";
                btnAddItem.Content = "Изменить";
                _item = _db.Items.Find(Data.item.ItemId);
            }
            WinItem.DataContext = _item;
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(tbName.Text)) errors.Append("Укажите название товара");
            if (string.IsNullOrEmpty(tbType.Text)) errors.Append("Укажите тип упаковки");
            if (string.IsNullOrEmpty(tbUnit.Text)) errors.Append("Укажите вес");
            if (string.IsNullOrEmpty(tbMeasurement.Text)) errors.Append("Укажите единицу измерения");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            try
            {
                if (open.SafeFileName.Length != 0)
                {
                    string newNamePhoto = Directory.GetCurrentDirectory() +
                         "\\images\\" + "_" + open.SafeFileName;
                    File.Copy(open.FileName, newNamePhoto, true);
                    _item.Image = "_" + open.SafeFileName;

                }
            }
            catch { }

            try
            {
                if (Data.item == null)
                {
                    _db.Items.Add(_item);
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

        private void btnSelectedPhoto_Click(object sender, RoutedEventArgs e)
        {
            open.Filter = "Все файлы |*.*| Файлы *.jpg|*.jpg";
            open.FilterIndex = 2;
            if (open.ShowDialog() == true)
            {
                BitmapImage photoImage = new BitmapImage(new Uri(open.FileName));
                img.Source = photoImage;
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddItem_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
