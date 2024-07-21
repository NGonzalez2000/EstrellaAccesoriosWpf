using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Windows;
using System.IO;

namespace EstrellaAccesoriosWpf
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            CheckDirectories();
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(AppContext.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .Build();
        }

        private static void CheckDirectories()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appPath = Path.Combine(appData,"EstrellaWpf");
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }
            string appImagePath = Path.Combine(appPath,"Images");
            if(!Directory.Exists(appImagePath))
            {
                Directory.CreateDirectory(appImagePath);
            }
        }

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddConfigurations();
            services.AddPersistance(configuration);
            services.AddViewModels();
            services.AddViews();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
