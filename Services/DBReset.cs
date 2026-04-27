using Microsoft.EntityFrameworkCore;
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
            try
            {
                if (MessageBox.Show("Biztosan újra szeretné tölteni az adatbázist? Ez a művelet visszafordíthatatlan, és minden jelenlegi adat elveszik!",
                    "Biztos?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _context.ProductFamilies.RemoveRange(_context.ProductFamilies);
                    _context.GlassSurcharges.RemoveRange(_context.GlassSurcharges);
                    _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('TermekCsalad', RESEED, 0)");
                    _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('UvegFelar', RESEED, 0)");
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt az adatbázis törlése során:\n " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Hiba történt az adatbázis újratöltése során:\n " + ex.Message, "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Adatbázis sikeresen visszaállítva az eredeti állapotára.", "Kész", MessageBoxButton.OK, MessageBoxImage.Information);
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
