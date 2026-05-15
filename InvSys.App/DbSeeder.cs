using BCrypt.Net;
using InvSys.Domain.Models.Account;
using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InvSys.App
{
    public static class DbSeeder
    {
        public static async Task SeedAsync()
        {
            await SeedAccountsAsync();
            await SeedInventoryAsync();
        }

        // ── Accounts (AccountsDbContext) ──────────────────────────────────
        private static async Task SeedAccountsAsync()
        {
            using var context = new AccountsDbContext();
            await context.Database.EnsureCreatedAsync();

            if (await context.UserAccounts.AnyAsync()) return;

            context.UserAccounts.AddRange(
                new UserAccount
                {
                    Username = "admin",
                    Email = "admin@admin.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "seed"
                },
                new UserAccount
                {
                    Username = "user",
                    Email = "user@user.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    Role = UserRole.User,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "seed"
                }
            );

            await context.SaveChangesAsync();
        }

        // ── Inventory (InventoryDbContext) ────────────────────────────────
        private static async Task SeedInventoryAsync()
        {
            using var context = new InventoryDbContext();
            await context.Database.EnsureCreatedAsync();

            await SeedSuppliersAsync(context);
            await SeedProductsAsync(context);
            await SeedStockAsync(context);
        }

        private static async Task SeedSuppliersAsync(InventoryDbContext context)
        {
            if (await context.Suppliers.AnyAsync()) return;

            context.Suppliers.AddRange(
                new Supplier { Name = "Pure Springs Co.", Email = "contact@puresprings.com", Location = "Manila", ContactNo = "09171000001", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "AquaFlow Distributors", Email = "info@aquaflow.com", Location = "Cebu City", ContactNo = "09171000002", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "Blue Ridge Water Supply", Email = "sales@blueridge.com", Location = "Davao City", ContactNo = "09171000003", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "Crystal Clear Waters", Email = "orders@crystalclear.com", Location = "Quezon City", ContactNo = "09171000004", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "Hydro Source Trading", Email = "hydrosource@mail.com", Location = "Iloilo City", ContactNo = "09171000005", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "Pacific Water Solutions", Email = "pacific@watersolutions.com", Location = "Makati", ContactNo = "09171000006", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "Aqua Prime Supply", Email = "aquaprime@supply.com", Location = "Cagayan de Oro", ContactNo = "09171000007", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "SafeWater Enterprises", Email = "safe@waterenterprises.com", Location = "Zamboanga", ContactNo = "09171000008", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "FreshWave Distribution", Email = "freshwave@distribution.com", Location = "Bacolod", ContactNo = "09171000009", IsActive = true, CreatedDate = DateTime.Now },
                new Supplier { Name = "AlkaLine Water Corp.", Email = "alkaline@watercorp.com", Location = "Pasig", ContactNo = "09171000010", IsActive = true, CreatedDate = DateTime.Now }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedProductsAsync(InventoryDbContext context)
        {
            if (await context.Products.AnyAsync()) return;

            var suppliers = await context.Suppliers.OrderBy(s => s.Id).ToListAsync();

            context.Products.AddRange(

                // Pure Springs Co.
                new Product { Name = "Purified Water 350ml", Price = 12.00m, Description = "Small purified drinking water bottle", SupplierId = suppliers[0].Id },
                new Product { Name = "Purified Water 500ml", Price = 18.00m, Description = "Standard purified water bottle", SupplierId = suppliers[0].Id },
                new Product { Name = "Purified Water 1L", Price = 30.00m, Description = "1 liter purified water bottle", SupplierId = suppliers[0].Id },
                new Product { Name = "Purified Water 5 Gallon", Price = 80.00m, Description = "5 gallon refill jug for water dispensers", SupplierId = suppliers[0].Id },
                new Product { Name = "Purified Water 10 Gallon", Price = 150.00m, Description = "10 gallon bulk refill container", SupplierId = suppliers[0].Id },

                // AquaFlow Distributors
                new Product { Name = "Mineral Water 350ml", Price = 15.00m, Description = "Mineral-enriched small water bottle", SupplierId = suppliers[1].Id },
                new Product { Name = "Mineral Water 500ml", Price = 22.00m, Description = "Mineral water with natural electrolytes", SupplierId = suppliers[1].Id },
                new Product { Name = "Mineral Water 1L", Price = 38.00m, Description = "1 liter mineral water bottle", SupplierId = suppliers[1].Id },
                new Product { Name = "Mineral Water 1.5L", Price = 50.00m, Description = "Large mineral water bottle", SupplierId = suppliers[1].Id },
                new Product { Name = "Mineral Water 5 Gallon", Price = 95.00m, Description = "5 gallon mineral water refill jug", SupplierId = suppliers[1].Id },

                // Blue Ridge Water Supply
                new Product { Name = "Distilled Water 500ml", Price = 20.00m, Description = "Distilled water, free of impurities", SupplierId = suppliers[2].Id },
                new Product { Name = "Distilled Water 1L", Price = 35.00m, Description = "1 liter distilled water bottle", SupplierId = suppliers[2].Id },
                new Product { Name = "Distilled Water 4L", Price = 75.00m, Description = "4 liter distilled water container", SupplierId = suppliers[2].Id },
                new Product { Name = "Distilled Water 5 Gallon", Price = 100.00m, Description = "5 gallon distilled water refill", SupplierId = suppliers[2].Id },
                new Product { Name = "Distilled Water 10 Gallon", Price = 180.00m, Description = "10 gallon distilled water bulk container", SupplierId = suppliers[2].Id },

                // Crystal Clear Waters
                new Product { Name = "Alkaline Water 350ml", Price = 25.00m, Description = "Alkaline water pH 8.5, small bottle", SupplierId = suppliers[3].Id },
                new Product { Name = "Alkaline Water 500ml", Price = 38.00m, Description = "Alkaline ionized water 500ml", SupplierId = suppliers[3].Id },
                new Product { Name = "Alkaline Water 1L", Price = 60.00m, Description = "1 liter alkaline water pH 9.0", SupplierId = suppliers[3].Id },
                new Product { Name = "Alkaline Water 1.5L", Price = 80.00m, Description = "Large alkaline water bottle pH 9.0", SupplierId = suppliers[3].Id },
                new Product { Name = "Alkaline Water 5 Gallon", Price = 150.00m, Description = "5 gallon alkaline water refill jug", SupplierId = suppliers[3].Id },

                // Hydro Source Trading
                new Product { Name = "Sparkling Water 350ml", Price = 30.00m, Description = "Carbonated sparkling water small", SupplierId = suppliers[4].Id },
                new Product { Name = "Sparkling Water 500ml", Price = 45.00m, Description = "Carbonated sparkling water 500ml", SupplierId = suppliers[4].Id },
                new Product { Name = "Sparkling Water 1L", Price = 70.00m, Description = "1 liter carbonated sparkling water", SupplierId = suppliers[4].Id },
                new Product { Name = "Sparkling Lemon Water 350ml", Price = 35.00m, Description = "Lemon-flavored sparkling water", SupplierId = suppliers[4].Id },
                new Product { Name = "Sparkling Berry Water 350ml", Price = 35.00m, Description = "Berry-flavored sparkling water", SupplierId = suppliers[4].Id },

                // Pacific Water Solutions
                new Product { Name = "Spring Water 500ml", Price = 20.00m, Description = "Natural spring water from the mountains", SupplierId = suppliers[5].Id },
                new Product { Name = "Spring Water 1L", Price = 35.00m, Description = "1 liter natural spring water", SupplierId = suppliers[5].Id },
                new Product { Name = "Spring Water 1.5L", Price = 48.00m, Description = "Large natural spring water bottle", SupplierId = suppliers[5].Id },
                new Product { Name = "Spring Water 5 Gallon", Price = 110.00m, Description = "5 gallon spring water refill jug", SupplierId = suppliers[5].Id },
                new Product { Name = "Spring Water 10 Gallon", Price = 200.00m, Description = "10 gallon spring water bulk refill", SupplierId = suppliers[5].Id },

                // Aqua Prime Supply
                new Product { Name = "Cucumber Infused Water 500ml", Price = 40.00m, Description = "Cucumber-infused purified water", SupplierId = suppliers[6].Id },
                new Product { Name = "Lemon Infused Water 500ml", Price = 40.00m, Description = "Lemon-infused purified water", SupplierId = suppliers[6].Id },
                new Product { Name = "Mint Infused Water 500ml", Price = 40.00m, Description = "Mint-infused purified water", SupplierId = suppliers[6].Id },
                new Product { Name = "Coconut Water 350ml", Price = 45.00m, Description = "Natural coconut water drink", SupplierId = suppliers[6].Id },
                new Product { Name = "Coconut Water 500ml", Price = 60.00m, Description = "Large natural coconut water", SupplierId = suppliers[6].Id },

                // SafeWater Enterprises
                new Product { Name = "pH Balanced Water 500ml", Price = 35.00m, Description = "pH 7.0 balanced drinking water", SupplierId = suppliers[7].Id },
                new Product { Name = "pH Balanced Water 1L", Price = 55.00m, Description = "1 liter pH 7.0 balanced water", SupplierId = suppliers[7].Id },
                new Product { Name = "pH Balanced Water 5 Gallon", Price = 130.00m, Description = "5 gallon pH balanced refill jug", SupplierId = suppliers[7].Id },
                new Product { Name = "Electrolyte Water 500ml", Price = 42.00m, Description = "Electrolyte-enhanced drinking water", SupplierId = suppliers[7].Id },
                new Product { Name = "Electrolyte Water 1L", Price = 65.00m, Description = "1 liter electrolyte water for hydration", SupplierId = suppliers[7].Id },

                // FreshWave Distribution
                new Product { Name = "Reverse Osmosis Water 1L", Price = 45.00m, Description = "RO filtered water, highly purified", SupplierId = suppliers[8].Id },
                new Product { Name = "Reverse Osmosis Water 5 Gallon", Price = 120.00m, Description = "5 gallon RO purified water refill", SupplierId = suppliers[8].Id },
                new Product { Name = "UV Treated Water 500ml", Price = 28.00m, Description = "UV light treated purified water", SupplierId = suppliers[8].Id },
                new Product { Name = "UV Treated Water 1L", Price = 45.00m, Description = "1 liter UV treated purified water", SupplierId = suppliers[8].Id },
                new Product { Name = "UV Treated Water 5 Gallon", Price = 105.00m, Description = "5 gallon UV treated water refill jug", SupplierId = suppliers[8].Id },

                // AlkaLine Water Corp.
                new Product { Name = "Ionized Water 500ml", Price = 50.00m, Description = "Ionized alkaline water for daily hydration", SupplierId = suppliers[9].Id },
                new Product { Name = "Ionized Water 1L", Price = 80.00m, Description = "1 liter ionized alkaline water", SupplierId = suppliers[9].Id },
                new Product { Name = "Ionized Water 5 Gallon", Price = 170.00m, Description = "5 gallon ionized alkaline water refill", SupplierId = suppliers[9].Id },
                new Product { Name = "Hydrogen Water 350ml", Price = 55.00m, Description = "Hydrogen-infused water for antioxidants", SupplierId = suppliers[9].Id },
                new Product { Name = "Hydrogen Water 500ml", Price = 75.00m, Description = "500ml hydrogen-rich antioxidant water", SupplierId = suppliers[9].Id }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedStockAsync(InventoryDbContext context)
        {
            if (await context.Stocks.AnyAsync()) return;

            var products = await context.Products.ToListAsync();

            foreach (var product in products)
            {
                context.Stocks.Add(new Stock
                {
                    ProductId = product.Id,
                    Quantity = 100,
                    CreatedDate = DateTime.Now
                });
            }

            await context.SaveChangesAsync();
        }
    }
}