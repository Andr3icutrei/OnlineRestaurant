using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineRestaurant.Database.Context;
using OnlineRestaurant.Infrastructure.Config;
using OnlineRestaurant.UI.View;
using OnlineRestaurant.UI.ViewModel;
using Microsoft.Extensions.Hosting;
using OnlineRestaurant.Core.Services;
using OnlineRestaurant.Database.Repositories;
using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.Services;
using System.Windows.Threading;

namespace OnlineRestaurant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();
        }
        public IHost HostInstance => _host;
        private void ConfigureServices(IServiceCollection services)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddDbContext<OnlineRestaurantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OnlineRestaurantDatabase")));

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IAllergenRepository, AllergenRepository>();
            services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
            services.AddScoped<IItemRepository,  ItemRepository>();
            services.AddScoped<IItemPictureRepository, ItemPictureRepository>();
            services.AddScoped<IMenuItemConfigurationRepository, MenuItemConfigurationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IAllergenService, AllergenService>();
            services.AddScoped<IFoodCategoryService, FoodCategoryService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemPictureService, ItemPictureService>();
            services.AddScoped<IMenuItemConfigurationService, MenuItemConfigurationService>();
            services.AddScoped<IOrderService, OrderService>();

            // Register navigation service
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INavigationServiceAsync, NavigationServiceAsync>();

            // Register ViewModels
            services.AddTransient<StartupWindowVM>();
            services.AddTransient<RegisterWindowVM>();
            services.AddTransient<AdministrationWindowVM>();
            services.AddTransient<UpsertFoodCategoryVM>();
            services.AddTransient<UpsertAllergenVM>();
            services.AddTransient<UpsertItemVM>();
            services.AddTransient<UpsertMenuVM>();
            services.AddTransient<VariableTextBoxesVM>();
            services.AddTransient<LoginWindowVM>();
            services.AddTransient<UserWindowVM>();
            services.AddTransient<GuestWindowVM>();
            services.AddTransient<OrdersControlVM>();
            services.AddTransient<FoodMenuControlVM>();

            services.AddTransient<StartupWindow>();
            services.AddTransient<RegisterWindow>();
            services.AddTransient<AdministrationWindow>();
            services.AddTransient<UpsertFoodCategoryWindow>();
            services.AddTransient<UpsertAllergenWindow>();
            services.AddTransient<UpsertItemWindow>();
            services.AddTransient<UpsertMenuWindow>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<UserWindow>();
            services.AddTransient<GuestWindow>();
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = $"Dispatcher Exception: {e.Exception}";
            File.WriteAllText("dispatcher_error.log", errorMessage);
            MessageBox.Show(errorMessage);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                string errorMessage = $"Unhandled Exception: {ex}";
                File.WriteAllText("unhandled_error.log", errorMessage);
                MessageBox.Show(errorMessage);
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var startupWindow = _host.Services.GetRequiredService<StartupWindow>();
            startupWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }

    }
}
