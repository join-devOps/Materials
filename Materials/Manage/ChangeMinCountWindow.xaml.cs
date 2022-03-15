using System;
using System.Windows;

namespace Materials.Manage
{
    /// <summary>
    /// Логика взаимодействия для ChangeMinCountWindow.xaml
    /// </summary>
    public partial class ChangeMinCountWindow : Window
    {
        public ChangeMinCountWindow()
        {
            InitializeComponent();
        }

        private double _Count;
        public double Count
        {
            get => _Count;
            set
            {
                _Count = value;
            }
        }

        private void Button_Change_Click(object sender, RoutedEventArgs e)
        {
            Count = Convert.ToDouble(TextBox_Count.Text);
            this.Close();
        }
    }
}
