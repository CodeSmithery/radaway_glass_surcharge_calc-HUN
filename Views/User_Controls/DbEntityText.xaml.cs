using radaway_surcharge_calc_HUN.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
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
    /// Interaction logic for DbEntityText.xaml
    /// </summary>
    public partial class DbEntityText : UserControl
    {
        public event EventHandler textChanged;
        public DbEntityText()
        {
            InitializeComponent();
        }
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
                               DependencyProperty.Register("Label", typeof(string), typeof(DbEntityText));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
                               DependencyProperty.Register("Placeholder", typeof(string), typeof(DbEntityText));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
                               DependencyProperty.Register("Value", typeof(string), typeof(DbEntityText));    


        private void tbxInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbxInput.Text))
                tblExample.Visibility = Visibility.Visible;

            else
                tblExample.Visibility = Visibility.Collapsed;

            textChanged?.Invoke(sender, EventArgs.Empty);
            
        }
    }
}
