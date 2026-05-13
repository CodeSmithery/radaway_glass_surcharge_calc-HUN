using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using radaway_surcharge_calc_HUN.Services;
using radaway_surcharge_calc_HUN.Views.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class ToolBarMenu : UserControl
    {
        public ToolBarMenu()
        {
            InitializeComponent();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item && item.Tag is string url)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nem sikerült megnyitni a hivatkozást:\n " + ex.Message);
                }
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            CustomMessageBox_Exit exit = new CustomMessageBox_Exit(parentWindow);
            exit.ShowDialog();
        }

        private void Database_Reset_Click(object sender, RoutedEventArgs e)
        {
            // var parentWindow = Window.GetWindow(this); 

            var resetService = App.AppHost.Services.GetService<DBReset>();

            if (resetService != null)
            {
                resetService.ResetDatabase();
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var exportService = App.AppHost.Services.GetService<Export>();
            string[] parts;
            string menuItemName = sender is MenuItem menuItem ? menuItem.Name : "";
            if (menuItemName.Length == 0)
                return;
            parts = menuItemName.Split('_');
            if (exportService != null)
            {
                exportService.ExportTable(parts[0]);
            }
        }

        private void miCRUD_Click(object sender, RoutedEventArgs e)
        {
            var window = App.AppHost.Services.GetService<CrudWindow>();
            window?.ShowDialog();
        }
    }
}