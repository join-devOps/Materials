using Materials.Manage;
using Materials.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Word =  Microsoft.Office.Interop.Word;

namespace Materials
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> MaterialsTypeList;

        public static List<string> listSort = new List<string> { "Наименование", "Остаток на складе", "Стоимость" };
        public static List<string> listReport = new List<string> { "Не экспортировать", "Список материалов", "Список поставщики", "Топ 5 дорогих материалов", "Топ 5 дешевых материалов", "Материалы с поставщиками" };

        private byte _CodeSort;
        public byte CodeSort
        {
            get => _CodeSort;
            set
            {
                _CodeSort = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MaterialsList"));
            }
        }

        private List<Material> _Materials;
        public List<Material> MaterialsList
        {
            get
            {
                List<Material> materialsSearched = _Materials;

                if (SearchInfo != null)
                    materialsSearched = materialsSearched.Where(item =>
                    item.Title.IndexOf(SearchInfo, StringComparison.OrdinalIgnoreCase) != -1).ToList();

                if (FilterItems != null)
                    materialsSearched = materialsSearched.Where(item =>
                    item.MaterialType.Title.IndexOf(FilterItems, StringComparison.OrdinalIgnoreCase) != -1).ToList();

                CountMaterials = (byte)materialsSearched.Count;

                if (CodeSort == 0 && ComboBox_Sort.SelectedIndex == 0)
                    return materialsSearched.OrderBy(item => item.Title).ToList();
                else if (CodeSort == 1 && ComboBox_Sort.SelectedIndex == 0)
                    return materialsSearched.OrderByDescending(item => item.Title).ToList();
                else if (CodeSort == 0 && ComboBox_Sort.SelectedIndex == 1)
                    return materialsSearched.OrderBy(item => item.CountInStock).ToList();
                else if (CodeSort == 1 && ComboBox_Sort.SelectedIndex == 1)
                    return materialsSearched.OrderByDescending(item => item.CountInStock).ToList();
                else return materialsSearched.ToList();
            }
            set
            {
                _Materials = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MaterialsList"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CountMaterials"));
                    PropertyChanged(this, new PropertyChangedEventArgs("MaxCountMaterials"));
                }
            }
        }

        public List<string> ReportList
        {
            get => listReport.ToList();
        }

        private bool _IsChanged = false;
        public bool IsChanged
        {
            get => _IsChanged;
            set
            {
                _IsChanged = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChanged"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ReportVisible"));
                }
            }
        }

        public Visibility ReportVisible
        {
            get => IsChanged ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ComboBox_Report_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_Report.SelectedIndex == 0)
                IsChanged = false;
            else IsChanged = true;
        }

        private void Button_AddReport_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Report.SelectedIndex == 1)
                ListMaterials_Report();
        }

        private void ListMaterials_Report()
        {
            var application = new Word.Application();
            Word.Document document = application.Documents.Add();
            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table newTable = document.Tables.Add(tableRange, MaterialsList.Count + 1, 3);
            newTable.Borders.InsideLineStyle = newTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            newTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = newTable.Cell(1, 1).Range;
            cellRange.Text = "Название материала";
            cellRange = newTable.Cell(1, 2).Range;
            cellRange.Text = "Стоимость материала";
            cellRange = newTable.Cell(1, 3).Range;
            cellRange.Text = "Минимальное количество";

            newTable.Rows[1].Range.Bold = 1;
            newTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (byte i = 0; i < MaterialsList.Count; i++)
            {
                var currentMaterial = MaterialsList[i];

                cellRange = newTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentMaterial.Title;

                cellRange = newTable.Cell(i + 2, 2).Range;
                cellRange.Text = (currentMaterial.Cost).ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                cellRange = newTable.Cell(i + 2, 3).Range;
                cellRange.Text = (currentMaterial.MinCount).ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            }

            application.Visible = true;

            document.SaveAs(Environment.CurrentDirectory);
        }

        private void ListSuppliers_Report()
        {
            List<Supplier> suppliers = Base.EM.Supplier.ToList();

            var application = new Word.Application();
            Word.Document document = application.Documents.Add();
            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table newTable = document.Tables.Add(tableRange, suppliers.Count + 1, 3);
            newTable.Borders.InsideLineStyle = newTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            newTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = newTable.Cell(1, 1).Range;
            cellRange.Text = "Название материала";
            cellRange = newTable.Cell(1, 2).Range;
            cellRange.Text = "Стоимость материала";
            cellRange = newTable.Cell(1, 3).Range;
            cellRange.Text = "Минимальное количество";

            newTable.Rows[1].Range.Bold = 1;
            newTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (byte i = 0; i < MaterialsList.Count; i++)
            {
                var currentMaterial = MaterialsList[i];

                cellRange = newTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentMaterial.Title;

                cellRange = newTable.Cell(i + 2, 2).Range;
                cellRange.Text = (currentMaterial.Cost).ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                cellRange = newTable.Cell(i + 2, 3).Range;
                cellRange.Text = (currentMaterial.MinCount).ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            }

            application.Visible = true;

            document.SaveAs(Environment.CurrentDirectory);
        }

        public List<string> listFiltrItems
        {
            get => GetItems.listFiltr.ToList();
        }

        public List<string> listSortItems
        {
            get => listSort.ToList();
        }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            MaterialsList = Base.EM.Material.ToList();
        }

        public byte MaxCountMaterials
        {
            get => (byte)MaterialsList.Count;
        }

        private byte _CountMaterials;
        public byte CountMaterials
        {
            get => _CountMaterials;
            set
            {
                _CountMaterials = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CountMaterials"));
            }
        }

        private byte _Pages;
        public byte Pages
        {
            get => _Pages;
            set
            {
                _Pages = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Pages"));
            }
        }

        private string _SearchInfo;
        public string SearchInfo
        {
            get => _SearchInfo;
            set
            {
                _SearchInfo = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MaterialsList"));
            }
        }

        private string _FilterItems;
        public string FilterItems
        {
            get => _FilterItems;
            set
            {
                _FilterItems = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MaterialsList"));
            }
        }

        private void ComboBox_Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_Filter.SelectedIndex > 0)
                FilterItems = ComboBox_Filter.SelectedItem.ToString();
            else FilterItems = null;
        }

        private void TextBox_Name_KeyUp(object sender, KeyEventArgs e)
        {
            SearchInfo = TextBox_Name.Text;
        }

        private void Button_Sort_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            switch (b.Uid)
            {
                case "SortUp":
                    CodeSort = 0;
                    break;
                case "SortDown":
                    CodeSort = 1;
                    break;
            }
        }

        private void Label__MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label l = (Label)sender;

            switch (l.Uid)
            {
                case "Back":
                    break;
                case "Next":
                    break;
                default:
                    break;
            }
        }

        private void ContextMenu_ChangeMinCount_Click(object sender, RoutedEventArgs e)
        {
            var list = ListView_Materials.SelectedItems;

            ChangeMinCountWindow cmcw = new ChangeMinCountWindow();

            cmcw.ShowDialog();

            if (cmcw.Count > 0)
            {
                foreach (Material m in list)
                {
                    m.MinCount = cmcw.Count;
                }
            }
            else MessageBox.Show("Введите не менее 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            ListView_Materials.Items.Refresh();
        }

        private void Button_ChangeMaterial_List(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender;
            byte index = Convert.ToByte(B.Uid);
            Material m = Base.EM.Material.FirstOrDefault(item => item.ID == index);
            List<MaterialSupplier> ms = Base.EM.MaterialSupplier.Where(item => item.MaterialID == index).ToList();

            MaterialsWindow editMaterials = new MaterialsWindow(m, ms);

            if ((bool)editMaterials.ShowDialog())
                PropertyChanged(this, new PropertyChangedEventArgs("MaterialsList"));
        }

        private void Button_DeleteMaterial_List(object sender, RoutedEventArgs e)
        {
            Button B = (Button)sender;
            byte index = Convert.ToByte(B.Uid);

            Material m = Base.EM.Material.FirstOrDefault(item => item.ID == index);
            Base.EM.Material.Remove(m);
            Base.EM.SaveChanges();

            PropertyChanged(this, new PropertyChangedEventArgs("MaterialsList"));
        }

        private void Button_AddItem_Click(object sender, RoutedEventArgs e)
        {
            Material newItemMaterial = new Material();
            List<MaterialSupplier> newItemMaterialSupplier = new List<MaterialSupplier>();
            MaterialsWindow newMaterial = new MaterialsWindow(newItemMaterial, newItemMaterialSupplier);

            if ((bool)newMaterial.ShowDialog())
            {
                MaterialsList = Base.EM.Material.ToList();
                PropertyChanged(this, new PropertyChangedEventArgs("MaterialsList"));
                PropertyChanged(this, new PropertyChangedEventArgs("MaxCountMaterials"));
            }
        }
    }
}