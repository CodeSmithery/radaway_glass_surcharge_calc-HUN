using radaway_surcharge_calc_HUN.Data;
using radaway_surcharge_calc_HUN.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace radaway_surcharge_calc_HUN.Services
{
    public class DBReset
    {
        private readonly dataContext _context;
        private readonly string rootPath = "OriginalDBContent\\";
        private readonly string productFile = "termekek.csv";
        private readonly string glassFile = "uvegfelarak.csv";
        public DBReset(dataContext context)
        {
            _context = context;
        }

        public void ResetDatabase()
        {
            // 1) Törlés
            if (File.Exists(rootPath + productFile))
                _context.ProductFamilies.RemoveRange(_context.ProductFamilies);
            else {
                MessageBox.Show($"Nem található a fájl a {rootPath + productFile} helyen!");
                return;
            }
            if (File.Exists(rootPath + glassFile))
                _context.GlassSurcharges.RemoveRange(_context.GlassSurcharges);
            else {
                MessageBox.Show($"Nem található a fájl a {rootPath + glassFile} helyen!");
                return;
            }
            _context.SaveChanges();

            // 2) Újratöltés CSV-ből
            try
            {
            LoadProductFamilies();
            LoadGlassSurcharges();

            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt az adatbázis újratöltése során:\n " + ex.Message);
                return;
            }
            MessageBox.Show("Adatbázis sikeresen visszaállítva az eredeti állapotára.");
        }

        private void LoadProductFamilies()
        {
            var lines = File.ReadAllLines(rootPath + productFile).Skip(1);

            foreach (var line in lines)
            {
                var parts = line.Split(';');

                _context.ProductFamilies.Add(new ProductFamilies
                {
                    Termek_Csalad = parts[1],
                    Uveg_Vastagsag_mm = int.Parse(parts[2])
                });
            }
        }

        private void LoadGlassSurcharges()
        {
            var lines = File.ReadAllLines(rootPath + glassFile).Skip(1);

            foreach (var line in lines)
            {
                var parts = line.Split(';');

                _context.GlassSurcharges.Add(new GlassSurcharges
                {
                    Uveg_Tipus = parts[1],
                    Uveg_Vastagsag_mm = int.Parse(parts[2]),
                    Felar_ft = int.Parse(parts[3])
                });
            }
        }
    }
}
