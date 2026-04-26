using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using radaway_surcharge_calc_HUN.Data;
using System.Windows;

namespace radaway_surcharge_calc_HUN
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<DataContext>(options =>
                        options.UseSqlServer(
                            context.Configuration.GetConnectionString("RadawayDB")));

                    services.AddSingleton<MainWindow>();
                })
                .Build();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
