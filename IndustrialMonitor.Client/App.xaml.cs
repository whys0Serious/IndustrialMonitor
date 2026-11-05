using IndustrialMonitor.Client.ViewModel;
using IndustrialMonitor.Infrastructure.Data;
using IndustrialMonitor.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;

namespace IndustrialMonitor.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                }).ConfigureServices((ctx, services) =>
                {
                    //注册Interface层的所有服务
                    services.AddInfrastructure(ctx.Configuration);
                    //注册主窗口
                    services.AddTransient<MainWindow>();
                    services.AddTransient<MainViewModel>();
                }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await _host.StartAsync();
            //初始化数据库
            using(var scope = _host.Services.CreateScope())
            {
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
                using (var dbContext = dbContextFactory.CreateDbContext())
                {
                    await dbContext.Database.EnsureCreatedAsync();
                }
            }
            
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            
        }
    }

}
