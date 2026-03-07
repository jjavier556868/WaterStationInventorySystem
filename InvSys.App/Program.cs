using InvSys.App.CRUDForms;
using InvSys.Infrastructure;
using InvSys.Services.Interfaces;
using InvSys.Services.IServices;
using InvSys.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvSys.App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // ?? Build DI container ???????????????????????????????????????
            var services = new ServiceCollection();

            // Database contexts
            services.AddScoped<InventoryDbContext>();
            services.AddScoped<AccountsDbContext>();

            // Services
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IPurchaseService, PurchaseService>();

            // Forms
            services.AddTransient<MainInventory>();
            services.AddSingleton<IServiceProvider>(p => p);
            services.AddTransient<LoginForm>();

            var provider = services.BuildServiceProvider();

            // ?? Launch app — same as before, just via DI ?????????????????
            Application.Run(provider.GetRequiredService<LoginForm>());
        }
    }
}