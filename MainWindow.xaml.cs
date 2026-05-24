using radaway_surcharge_calc_HUN.Data;
using radaway_surcharge_calc_HUN.Models;
using radaway_surcharge_calc_HUN.Views.User_Controls;
using radaway_surcharge_calc_HUN.Views.Windows;
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

namespace radaway_surcharge_calc_HUN
{
    public partial class MainWindow : Window
    {
        public bool IsCrudMenuOn { get; set; } = true;
        public readonly dataContext _dbcontext;

        private List<ProductFamilies> productFamilies;
        private List<GlassSurcharges> glassSurcharges;

        private ComboBox prCBox;
        private ComboBox sidesCBox;
        private TextBlock prGlassThickness;
        private ComboBox glCBox;
        private TextBox userInputPrice;
        private TextBox priceOutput;
        private static void Placement (UIElement element, int row, int column, Grid parent) {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            parent.Children.Add(element);
        }
        private void OnPrComboBoxChanged() {
            if (!productFamilies.Select(x => x.Termek_Csalad).ToList().Contains(prCBox.SelectedItem))
            {
                prCBox.ClearValue(ComboBox.TextProperty);
                glCBox.ClearValue(ComboBox.ItemsSourceProperty);
                glCBox.ClearValue(ComboBox.TextProperty);
                prGlassThickness.Text = "-";
                userInputPrice.ClearValue(TextBox.TextProperty);
                glCBox.IsEnabled = false;
                userInputPrice.IsEnabled = false;
                priceOutput.ClearValue(TextBox.TextProperty);
                return;
            }
            else if (productFamilies.Select(x => x.Termek_Csalad).ToList().Contains(prCBox.SelectedItem))
            { 
            prGlassThickness.Text = productFamilies
                .Where(x => x.Termek_Csalad == prCBox.SelectedItem.ToString())
                .Select(x => x.Uveg_Vastagsag_mm)
                .FirstOrDefault()
                .ToString() + " mm";
            userInputPrice.IsEnabled = true;
            glCBox.ItemsSource =glassSurcharges.Where(x => x.Uveg_Vastagsag_mm == int.Parse(prGlassThickness.Text.First().ToString()))
                .Select(x => x.Uveg_Tipus)
                .ToList()
                .Distinct();                      
            }
        }
        private void OnGlComboBoxChanged() {
            if (!glassSurcharges.Select(x => x.Uveg_Tipus).ToList().Contains(glCBox.SelectedItem))
            {
                priceOutput.ClearValue(TextBox.TextProperty);
                glCBox.ClearValue(ComboBox.TextProperty);
                btnCalculate.IsEnabled = false;
                return;
            }
            else if (glassSurcharges.Select(x => x.Uveg_Tipus).ToList().Contains(glCBox.SelectedItem))
            {
                btnCalculate.IsEnabled = true;   
            }
        }
        private void InputTextChanged() {
            if (Int32.TryParse(userInputPrice.Text, out int result)) {
                glCBox.IsEnabled = true;
                glCBox.Text = "Válassz!";
            }
            else {
                priceOutput.ClearValue(TextBox.TextProperty);
                glCBox.ClearValue(ComboBox.TextProperty);
                glCBox.IsEnabled = false;
                userInputPrice.ClearValue(TextBox.TextProperty);
                btnCalculate.IsEnabled = false;
                return;
            }
        }

        public MainWindow(dataContext dbcontext)
        {
            InitializeComponent();

            btnCalculate.IsEnabled = false;

            DataContext = this;

            _dbcontext = dbcontext;

            LoadDataFromDb();

            CreateGridContent();
            
        }

        private void LoadDataFromDb()
        {
            productFamilies = [.. _dbcontext.ProductFamilies];
            glassSurcharges = [.. _dbcontext.GlassSurcharges];
        }

        private void CreateGridContent()
        {
            prCBox = new()
            {
                ItemsSource = productFamilies?.Select(x => x.Termek_Csalad).ToList().Distinct(),
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 200,
                IsEditable = true,
                Text = "Válassz!",
            };

            sidesCBox = new()
            {
                ItemsSource = new List<int> { 1, 2, 3, 4 },
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 200,
                IsEditable = true,
                Text = "Válassz!",
            };

            prGlassThickness = new()
            {
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 200,
                Text = "-"
            };

            glCBox = new()
            {  
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 200,
                IsEnabled = false,
                IsEditable = true,
            };

            userInputPrice = new()
            {
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 200,
                IsEnabled = false
            };

            priceOutput = new()
            {
                FontSize = 24,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 200,
                IsReadOnly = true,
                IsReadOnlyCaretVisible = false,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                FontWeight = FontWeights.Bold,
                
            };

            Placement(priceOutput, 0, 0, gdOutput);
            Placement(prCBox, 0, 1, gdCalculator);
            Placement(prGlassThickness, 1, 1, gdCalculator);
            Placement(sidesCBox, 2, 1, gdCalculator);
            Placement(userInputPrice, 3, 1, gdCalculator);
            Placement(glCBox, 4, 1, gdCalculator);

            prCBox.SelectionChanged += (s, e) => OnPrComboBoxChanged();
            prCBox.LostFocus += (s,e) => OnPrComboBoxChanged();
            glCBox.SelectionChanged += (s,e) => OnGlComboBoxChanged();
            glCBox.LostFocus += (s,e) => OnGlComboBoxChanged();
            userInputPrice.TextChanged += (s,e) => InputTextChanged();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            int sides = Int32.Parse(sidesCBox.Text);
            int inputPrice = Int32.Parse(userInputPrice.Text);
            int surcharge = glassSurcharges.Where(
                x => x.Uveg_Tipus == glCBox.SelectedItem.ToString() 
                && x.Uveg_Vastagsag_mm == Int32.Parse (prGlassThickness.Text
                .First()
                .ToString()))
                .Select(y => y.Felar_ft)
                .FirstOrDefault(0);

            int price = inputPrice + surcharge * sides;
            priceOutput.Text = price.ToString("N2");
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Biztosan kilépsz?","Kilépés",MessageBoxButton.YesNo,MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }

        }
    }

}