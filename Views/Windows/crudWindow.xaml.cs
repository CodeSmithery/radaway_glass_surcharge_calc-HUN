using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using radaway_surcharge_calc_HUN.Data;
using radaway_surcharge_calc_HUN.Models;
using radaway_surcharge_calc_HUN.Views.User_Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
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
        private bool tbHasChanges = false;
        private bool dgHasChanges = false;

        private DbEntityText pr_Fam;
        private DbEntityText pr_Thick;
        private DbEntityText gl_Type;
        private DbEntityText gl_Thick;
        private DbEntityText gl_Surch;

        private ObservableCollection<ProductFamilies> productFamilies;
        private ObservableCollection<GlassSurcharges> glassSurcharges;

        public readonly dataContext _dbcontext;


        public CrudWindow(dataContext dbcontext)
        {
            InitializeComponent();

            _dbcontext = dbcontext;
            DataContext = this;


            LoadDataFromDb();
            CreateGridTextBoxes();
            LoadData();


        }
        private void btSave_Click(object? sender, RoutedEventArgs e)
        {
            string activeTab = (tabcCreate.SelectedItem as TabItem)?.Name.ToString();
            int thicknessHelper;
            int surchargeHelper;

            if (Validation.GetHasError(dgGlassSurcharges) || Validation.GetHasError(dgProductFamilies))
            {
                MessageBox.Show("Hibás adat van a táblázatban. Javítsd ki mentés előtt.");
                return;
            }

            switch(activeTab)
            {
                case "tabProductFamilies":
                    if (!string.IsNullOrEmpty(pr_Fam.tbxInput.Text)
                        && !string.IsNullOrEmpty(pr_Thick.tbxInput.Text))
                    {
                        if (Int32.TryParse(pr_Thick.tbxInput.Text, out int prThick))
                            thicknessHelper = prThick;                
                        else
                        {
                            MessageBox.Show("A vastagság mezőbe csak számot lehet írni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        Reseed();

                        var newProductFamily = new ProductFamilies
                        {
                            Termek_Csalad = pr_Fam.tbxInput.Text.ToUpper(),
                            Uveg_Vastagsag_mm = thicknessHelper
                        };

                        _dbcontext.ProductFamilies.Add(newProductFamily);
                        _dbcontext.SaveChanges();
                        pr_Fam.tbxInput.Text = string.Empty;
                        pr_Thick.tbxInput.Text = string.Empty;
                        MessageBox.Show("A termékcsalád sikeresen létrehozva!");
                    }
                    else if (!string.IsNullOrEmpty(pr_Fam.tbxInput.Text)
                        || !string.IsNullOrEmpty(pr_Thick.tbxInput.Text))
                    {
                        MessageBox.Show("Minden termékcsaládhoz tartozó mező kitöltése kötelező annak mentéséhez!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
                    }     
                    else if (dgHasChanges)
                    {
                        _dbcontext.SaveChanges();
                    }
                    break; 

                case "tabGlassSurcharges":
                    if (!string.IsNullOrEmpty(gl_Type.tbxInput.Text)
                        && !string.IsNullOrEmpty(gl_Thick.tbxInput.Text)
                        && !string.IsNullOrEmpty(gl_Surch.tbxInput.Text))
                    {


                        if (Int32.TryParse(gl_Thick.tbxInput.Text, out int glThick))
                            thicknessHelper = glThick;
                        else
                        {
                            MessageBox.Show("A vastagság mezőbe csak számot lehet írni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (Int32.TryParse(gl_Surch.tbxInput.Text, out int glSurch))
                            surchargeHelper = glSurch;
                        else
                        {
                            MessageBox.Show("A felár mezőbe csak számot lehet írni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        Reseed();
                        var newGlassSurcharge = new GlassSurcharges
                        {
                            Uveg_Tipus = gl_Type.tbxInput.Text.ToUpper(),
                            Uveg_Vastagsag_mm = thicknessHelper,
                            Felar_ft = surchargeHelper
                        };

                        _dbcontext.GlassSurcharges.Add(newGlassSurcharge);
                        _dbcontext.SaveChanges();
                        gl_Surch.tbxInput.Text = string.Empty;
                        gl_Thick.tbxInput.Text = string.Empty;
                        gl_Type.tbxInput.Text = string.Empty;
                        MessageBox.Show("Az üvegtípus felára sikeresen létrehozva!");
                    }
                    else if (!string.IsNullOrEmpty(gl_Type.tbxInput.Text)
                        || !string.IsNullOrEmpty(gl_Thick.tbxInput.Text)
                        || !string.IsNullOrEmpty(gl_Surch.tbxInput.Text))
                    {
                        MessageBox.Show("Minden üvegtípushoz tartozó mező kitöltése kötelező annak mentéséhez!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (dgHasChanges)
                    {
                        _dbcontext.SaveChanges();
                    }
                    break;

                default:
                    MessageBox.Show("Valami hiba történt a mentés során. Próbáld újra!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            LoadDataFromDb();
            LoadData();
            tbHasChanges = false;
            dgHasChanges = false;
            btSave.IsEnabled = false;
        }

        private void LoadData()
        {
            dgProductFamilies.ItemsSource = productFamilies;
            dgGlassSurcharges.ItemsSource = glassSurcharges;
        }

        private void CreateGridTextBoxes()
        {
            Random rnd = new();
            int prRnd;
            int glRnd;

            prRnd = rnd.Next(0, productFamilies.Count);
            glRnd = rnd.Next(0, glassSurcharges.Count);

            pr_Fam = new();
            pr_Fam.textChanged += TbHasChanges;

            pr_Thick = new();
            pr_Thick.textChanged += TbHasChanges;

            gl_Type = new();
            gl_Type.textChanged += TbHasChanges;

            gl_Thick = new();
            gl_Thick.textChanged += TbHasChanges;

            gl_Surch = new();
            gl_Surch.textChanged += TbHasChanges;

            Grid.SetRow(pr_Fam, 0);
            Grid.SetRow(pr_Thick, 1);
            Grid.SetRow(gl_Type, 0);
            Grid.SetRow(gl_Thick, 1);
            Grid.SetRow(gl_Surch, 2);

            pr_Fam.SetValue(DbEntityText.LabelProperty, "Termékcsalád:");
            pr_Fam.SetValue(DbEntityText.PlaceholderProperty, productFamilies[prRnd].Termek_Csalad);
            pr_Thick.SetValue(DbEntityText.LabelProperty, "Vastagság:");
            pr_Thick.SetValue(DbEntityText.PlaceholderProperty, productFamilies[prRnd].Uveg_Vastagsag_mm.ToString());
            gl_Type.SetValue(DbEntityText.LabelProperty, "Üvegtípus:");
            gl_Type.SetValue(DbEntityText.PlaceholderProperty, glassSurcharges[glRnd].Uveg_Tipus);
            gl_Thick.SetValue(DbEntityText.LabelProperty, "Vastagság:");
            gl_Thick.SetValue(DbEntityText.PlaceholderProperty, glassSurcharges[glRnd].Uveg_Vastagsag_mm.ToString());
            gl_Surch.SetValue(DbEntityText.LabelProperty, "Felár:");
            gl_Surch.SetValue(DbEntityText.PlaceholderProperty, glassSurcharges[glRnd].Felar_ft.ToString());

            gdInputPr.Children.Add(pr_Fam);
            gdInputPr.Children.Add(pr_Thick);
            gdInputGl.Children.Add(gl_Type);
            gdInputGl.Children.Add(gl_Thick);
            gdInputGl.Children.Add(gl_Surch);
        }

        private void Reseed()
        {
            _dbcontext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('TermekCsalad', RESEED)");
            _dbcontext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('UvegFelar', RESEED)");
        }

        private void LoadDataFromDb()
        {
            _dbcontext.ChangeTracker.Clear();
            productFamilies = new ObservableCollection<ProductFamilies>([.. _dbcontext.ProductFamilies]);
            glassSurcharges = new ObservableCollection<GlassSurcharges>([.. _dbcontext.GlassSurcharges]);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Biztosan kilépsz?", "Kilépés", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgProductFamilies.SelectedItem != null)
                DeleteProductFamily();
            else if (dgGlassSurcharges.SelectedItem != null)
                DeleteGlassSurcharge();
        }

        private void TbHasChanges(object sender, EventArgs e)
        {
            bool tb = (sender as TextBox).Text.ToString().IsNullOrEmpty();

            if (tb)
            {
                tbHasChanges = false;
                btSave.IsEnabled = false;
                return;
            }
            else if (!tb)
            {
                tbHasChanges = true;
                btSave.IsEnabled = true;
            }
        }
        private void DgHasChanges(object sender, EventArgs e)
        {
            dgHasChanges = true;
            btSave.IsEnabled = true;
        }

        private void dgProductFamilies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btDelete.IsEnabled = dgProductFamilies.SelectedItem != null;
        }

        private void dgGlassSurcharges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btDelete.IsEnabled = dgGlassSurcharges.SelectedItem != null;
        }

        private void DeleteProductFamily()
        {
            var selectedItems = dgProductFamilies.SelectedItems
                .Cast<ProductFamilies>()
                .ToList();

            if (selectedItems.Count == 0)
                return;

            if (MessageBox.Show(
                $"{selectedItems.Count} elem törlése?",
                "Megerősítés",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            foreach (var item in selectedItems)
            {
                _dbcontext.ProductFamilies.Remove(item);
                productFamilies.Remove(item);
            }

            _dbcontext.SaveChanges();
        }

        private void DeleteGlassSurcharge()
        {
            var selectedItems = dgGlassSurcharges.SelectedItems
            .Cast<GlassSurcharges>()
            .ToList();

            if (selectedItems.Count == 0)
                return;

            if (MessageBox.Show(
                $"{selectedItems.Count} elem törlése?",
                "Megerősítés",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            foreach (var item in selectedItems)
            {
                _dbcontext.GlassSurcharges.Remove(item);
                glassSurcharges.Remove(item);
            }

            _dbcontext.SaveChanges();
        }
    }
}
