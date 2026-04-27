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
    /// Interaction logic for CustomMessageBox_Exit.xaml
    /// </summary>
    public partial class CustomMessageBox_Exit : Window
    {
        private Window _parent;
        public CustomMessageBox_Exit(Window parent)
        {
            InitializeComponent();
            _parent = parent;
        }
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            _parent.Close();
            this.Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
