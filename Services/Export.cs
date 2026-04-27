using radaway_surcharge_calc_HUN.Data;
using radaway_surcharge_calc_HUN.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace radaway_surcharge_calc_HUN.Services
{
    public class Export
    {
        private readonly dataContext _context;
        private readonly string folderName = "Exports\\";
        private readonly string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        public Export(dataContext context)
        {
            _context = context;
        }
        public void ExportTable(string dbName) {
            Directory.CreateDirectory(folderName);
            try
            {
                switch (dbName.ToUpper())
                {
                    case "PR":
                        ExpProductFamilies();
                        break;
                    case "GS":
                        ExpGlassSurcharges();
                        break;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Hiba:\n"+ex.Message);
                return;
            }
        }
        private void ExpProductFamilies() {
            string path = Path.Combine(baseDir ,folderName, "exported_product-families_table.txt");
            StreamWriter sw = new StreamWriter(path, false);
            foreach (var item in _context.ProductFamilies)
            {
                sw.WriteLine($"{item.TermekID}\t{item.Termek_Csalad}\t{item.Uveg_Vastagsag_mm}");
            }
            sw.Close();
            FileCreatedAt(path);
        }
        public void ExpGlassSurcharges() {
            string path = Path.Combine(baseDir, folderName, "exported_glass-surcharge_table.txt");
            StreamWriter sw = new StreamWriter(path, false);
            foreach (var item in _context.GlassSurcharges)
            {
                sw.WriteLine($"{item.FelarID}\t{item.Uveg_Tipus}\t{item.Uveg_Vastagsag_mm}\t{item.Felar_ft}");
            }
            sw.Close();
            FileCreatedAt(path);
        }
        public void FileCreatedAt(string fPath)
        {
            MessageBox.Show($"Az exportált fájl elérhető:\n{fPath}","Kész", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}