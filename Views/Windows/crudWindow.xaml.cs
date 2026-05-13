using Microsoft.EntityFrameworkCore;
using radaway_surcharge_calc_HUN.Data;
using radaway_surcharge_calc_HUN.Services.Command;
using radaway_surcharge_calc_HUN.Views.User_Controls;
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
    /// Interaction logic for CrudWindow.xaml
    /// </summary>
    public partial class CrudWindow : Window
    {
        public bool IsCrudMenuOn { get; set; } = false;
        public ICommand SaveCommand { get; }

        public readonly dataContext _dbcontext;
        public CrudWindow(dataContext dbcontext)
        {
            InitializeComponent();
            _dbcontext = dbcontext;

            SaveCommand = new RelayCommand(Save, () => false);
            DataContext = this;

            /*foreach(var item in _dbcontext.GlassSurcharges)
            { 
                UserControl glass = new GlassElement(item.FelarID.ToString(), item.Uveg_Tipus,item.Uveg_Vastagsag_mm.ToString(),item.Felar_ft.ToString());
                lvGlassSurcharges.Items.Add(glass);
            }
            */
            LoadData();
        }
        public void Save()
       {

       }
        public void LoadData() 
        { 
            dgProductFamilies.ItemsSource= _dbcontext.ProductFamilies.ToList();


        }
    }
}
