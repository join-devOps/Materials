using Materials.SQL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Materials.Manage
{
    /// <summary>
    /// Логика взаимодействия для EditItems.xaml
    /// </summary>
    public partial class MaterialsWindow : Window, INotifyPropertyChanged
    {
        public Material CurrentMaterial { get; set; }
        public List<MaterialSupplier> CurrentMaterialSupplier { get; set; }

        public string GetNameWindow
        {
            get => CurrentMaterial.ID == 0 ? "Новый материал" : "Редактирование материала";
        }

        public string GetContentButton
        {
            get => CurrentMaterial.ID == 0 ? "Добавить" : "Изменить";
        }

        public string GetContentImage
        {
            get => CurrentMaterial.ID == 0 ? "Добавить изображение" : "Изменить изображение";
        }

        public List<string> GetSupplierList
        {
            get => Base.EM.Supplier.Select(item => item.Title).ToList();
        }

        public MaterialsWindow(Material mat, List<MaterialSupplier> ms)
        {
            InitializeComponent();

            CurrentMaterial = mat;
            CurrentMaterialSupplier = ms;
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> GetItemsType
        {
            get => GetItems.listFiltr.ToList();
        }

        public List<Supplier> GetItemsSupplier
        {
            get => Base.EM.Supplier.ToList();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMaterial.MinCount < 0)
                MessageBox.Show("Введите не меньше 0", $"Значение: {CurrentMaterial.MinCount}", MessageBoxButton.OK, MessageBoxImage.Information);

            if (CurrentMaterial.Cost < 0)
                MessageBox.Show("Введите не меньше 0", $"Значение: {CurrentMaterial.Cost}", MessageBoxButton.OK, MessageBoxImage.Information);

            CurrentMaterial.MaterialTypeID = ComboBox_TypeMaterial.SelectedIndex;

            if (CurrentMaterial.ID == 0)
            {
                try { Base.EM.Material.Add(CurrentMaterial); Base.EM.SaveChanges(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            }
            else
            {
                try { Base.EM.SaveChanges(); }
                catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            }

            DialogResult = true;
        }

        private void Bitton_EditImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файлы изображений: (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";
            ofd.InitialDirectory = Environment.CurrentDirectory;

            if (ofd.ShowDialog() == true)
            {
                CurrentMaterial.Image = "\\materials\\" + ofd.FileName.Substring(Environment.CurrentDirectory.Length + 1);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentMaterial"));
                }
            }
        }
    }
}