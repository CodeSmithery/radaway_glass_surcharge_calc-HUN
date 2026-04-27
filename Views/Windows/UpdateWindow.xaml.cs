using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public bool IsSaveOn { get; set; } = true;
        public bool IsCrudMenuOn { get; set; } = false;
        public ICommand SaveCommand { get; }
        public UpdateWindow(string tableName)
        {
            InitializeComponent();
            InitializeComponent();
            SaveCommand = new RelayCommand(Save, () => true);
            DataContext = this;
        }
        public void Save()
        {
        }
    }
}
