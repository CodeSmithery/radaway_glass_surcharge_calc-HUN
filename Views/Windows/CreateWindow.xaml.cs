using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using radaway_surcharge_calc_HUN.Services.Command;

namespace radaway_surcharge_calc_HUN.Views.Windows
{
    /// <summary>
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        public bool IsSaveOn { get; set; } = true;
        public bool IsCrudMenuOn { get; set; } = false;
        public ICommand SaveCommand { get; }
        public CreateWindow(string tableName)
        {
            InitializeComponent();
            SaveCommand = new RelayCommand(Save, () => true);
            DataContext = this;
        }
        public void Save()
        {
        }
    }
}
