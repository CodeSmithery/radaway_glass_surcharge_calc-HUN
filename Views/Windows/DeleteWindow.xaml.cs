using radaway_surcharge_calc_HUN.Services.Command;
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

namespace radaway_surcharge_calc_HUN.Views.Windows
{
    /// <summary>
    /// Interaction logic for DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        public bool IsSaveOn { get; set; } = false;
        public bool IsCrudMenuOn { get; set; } = false;
        public ICommand SaveCommand { get; }
        public DeleteWindow(string tableName)
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
    
