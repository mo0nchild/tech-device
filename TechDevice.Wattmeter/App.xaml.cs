using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using TechDevice.Wattmeter.Services;
using TechDevice.Wattmeter.Views;

namespace TechDevice.Wattmeter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Provider { get; private set; } = default!;

        protected virtual IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureServices(services => 
            {
                services.AddServices().AddSingleton<MainWindow>();
            });
        }
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            var applicationHost = this.CreateHostBuilder().Build();

            Provider = applicationHost.Services;
            Provider.GetRequiredService<MainWindow>().Show();
        }
    }

}
