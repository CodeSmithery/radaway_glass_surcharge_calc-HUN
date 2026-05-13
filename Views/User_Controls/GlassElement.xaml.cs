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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace radaway_surcharge_calc_HUN.Views.User_Controls
{
    /// <summary>
    /// Interaction logic for GlassElement.xaml
    /// </summary>
    public partial class GlassElement : UserControl
    {
        public GlassElement(string _id, string _glassType, string _glassThickness, string _surchargeAmount)
        {
            InitializeComponent();
            /*
            tbId.Text = _id;
            tbTypeName.Text = _glassType;
            tbThickness.Text = _glassThickness;
            tbSurchargeAmount.Text = _surchargeAmount;
            */
        }
    }
}
