using Microsoft.EntityFrameworkCore;
using radaway_surcharge_calc_HUN.Data;
using radaway_surcharge_calc_HUN.Models;
using radaway_surcharge_calc_HUN.Services.Command;
using radaway_surcharge_calc_HUN.Views.User_Controls;
using System;
using System.Collections.Generic;
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
        public ICommand SaveCommand { get; }

        private DbEntityText pr_Fam;
        private DbEntityText pr_Thick;
        private DbEntityText gl_Type;
        private DbEntityText gl_Thick;
        private DbEntityText gl_Surch;

        private List<ProductFamilies> productFamilies;
        private List<GlassSurcharges> glassSurcharges;

        public readonly dataContext _dbcontext;
        public CrudWindow(dataContext dbcontext)
        {
            InitializeComponent();
            _dbcontext = dbcontext;

            SaveCommand = new RelayCommand(Save, () => true);
            DataContext = this;


            LoadDataFromDb();
            CreateGridTextBoxes();
            LoadData();


        }
        private void Save()
        {
            if (Validation.GetHasError(dgGlassSurcharges) || Validation.GetHasError(dgProductFamilies))
            {
                MessageBox.Show("Hibás adat van a táblázatban. Javítsd ki mentés előtt.");
                return;
            }

            int thicknessHelper;
            int surchargeHelper;

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
                MessageBox.Show("A termékcsalád sikeresen létrehozva!");
            }
            else
            {
                MessageBox.Show("Minden termékcsaládhoz tartozó mező kitöltése kötelező annak mentéséhez!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
            }


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
                MessageBox.Show("Az üvegtípus felára sikeresen létrehozva!");
            }
            else
            {
                MessageBox.Show("Minden üvegtípushoz tartozó mező kitöltése kötelező annak mentéséhez!", "Információ", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            LoadDataFromDb();
            LoadData();
        }

        private void LoadData()
        {
            dgProductFamilies.ItemsSource = productFamilies;
            dgGlassSurcharges.ItemsSource = glassSurcharges;
        }

        private void CreateGridTextBoxes()
        {
            Random rnd = new Random();
            int prRnd;
            int glRnd;

            prRnd = rnd.Next(0, productFamilies.Count);
            glRnd = rnd.Next(0, glassSurcharges.Count);

            pr_Fam = new DbEntityText();
            pr_Thick = new DbEntityText();
            gl_Type = new DbEntityText();
            gl_Thick = new DbEntityText();
            gl_Surch = new DbEntityText();

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

        public void LoadDataFromDb()
        {
            productFamilies = _dbcontext.ProductFamilies.ToList();
            glassSurcharges = _dbcontext.GlassSurcharges.ToList();
        }
    }
}
