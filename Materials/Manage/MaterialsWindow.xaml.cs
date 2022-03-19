using Materials.SQL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Materials.Manage
{
    /// <summary>
    /// Логика взаимодействия для EditItems.xaml
    /// </summary>
    public partial class MaterialsWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Material CurrentMaterial { get; set; }
        public List<MaterialSupplier> CurrenrMaterialSupplier { get; set; }
        public List<MaterialType> CurrentMaterilType { get; set; }

        public string GetNameWindow
        {
            get => CurrentMaterial.ID == 0 ? "Новый материал" : "Редактирование материала";
        }

        public string GetContentButton
        {
            get => CurrentMaterial.ID == 0 ? "Добавить" : "Сохранить";
        }

        public string GetContentImage
        {
            get => CurrentMaterial.ID == 0 ? "Добавить изображение" : "Изменить изображение";
        }

        private bool _GetTrueChangedFunctions = false;
        public bool GetTrueChangedFunctions
        {
            get => _GetTrueChangedFunctions;
            set
            {
                _GetTrueChangedFunctions = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("GetTrueChangedFunctions"));
                    PropertyChanged(this, new PropertyChangedEventArgs("GetChangerFunction"));
                }
            }
        }

        public Visibility GetChangerFunction
        {
            get => GetTrueChangedFunctions ? Visibility.Visible : Visibility.Collapsed;
        }

        public List<string> GetSupplierList
        {
            get => Base.EM.Supplier.Select(item => item.Title).ToList();
        }

        private bool _GetAccess = true;
        public bool GetAccess
        {
            get => _GetAccess;
            set
            {
                _GetAccess = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("GetContentButtonMaterialSuopplier"));
                    PropertyChanged(this, new PropertyChangedEventArgs("GetAccess"));
                }
            }
        }

        public MaterialsWindow(Material mat, List<MaterialSupplier> ms)
        {
            InitializeComponent();

            CurrentMaterial = mat;
            CurrenrMaterialSupplier = ms;
            this.DataContext = this;

            CurrentMaterilType = Base.EM.MaterialType.ToList();
        }

        public List<string> GetItemsType
        {
            get => GetItems.listFiltr.ToList();
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

        private void Button_AddMaterialSupplier_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_EditMaterialSupplier_Click(object sender, RoutedEventArgs e)
        {
            GetTrueChangedFunctions = false;
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

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            GetTrueChangedFunctions = true;
            MaterialSupplier ms = ListView_MaterialSupplier.SelectedItem as MaterialSupplier;
        }

        private void Button_CloseEditMaterialSupplier_Click(object sender, RoutedEventArgs e)
        {
            GetTrueChangedFunctions = false;
        }
    }
}