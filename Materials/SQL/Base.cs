using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Materials.SQL
{
    class Base
    {
        public static Entities EM = new Entities();
    }

    class GetItems
    {
        public static List<string> listFiltr = new List<string> { "Все типы", "Гранулы", "Рулон", "Нарезка", "Пресс" };
    }

    class EditData
    {
        public static List<Material> EditListMaterials { get; set; }
    }

    public partial class Material
    {

        public string GetCountInStock
        {
            get => "Остаток: " + CountInStock;
        }
        public SolidColorBrush GetColor
        {
            get
            {
                if (MinCount > CountInStock)
                    return (SolidColorBrush)(new BrushConverter().ConvertFrom("#f19292"));
                return (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffba01"));
            }
        }

        public string GetPhoto
        {
            get
            {
                if (Image == null)
                {
                    return "\\Materials\\picture.png";
                }
                else
                {
                    return Image;
                }
            }
        }

        public string GetSupplier
        {
            get
            {
                string str = null;

                foreach(var list in MaterialSupplier)
                {
                    str += list.Supplier.Title + " | ";
                }

                return str;
            }
        }

        public string GetMaterialSupplierID
        {
            get
            {
                string id = null;

                foreach (var list in MaterialSupplier)
                {
                    id += list.ID + " | ";
                }

                return id;
            }
        }
    }
}