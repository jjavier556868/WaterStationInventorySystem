using InvSys.App.CRUDForms;
using InvSys.App.Helpers;
using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;
using InvSys.Services.Services;
using Microsoft.EntityFrameworkCore;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace InvSys.App
{
    public partial class MainInventory : Form
    {
        private string _currentUsername;
        private UserRole _currentUserRole;
        private List<CartItem> _cart = new List<CartItem>();
        private AccountDisplayDTO _selectedAccount = null;
        private ReceiptData _lastReceiptData = null;

        // Cached so sync callbacks (QueryRowStyle, FormClosing) can use it
        private HashSet<int> _inactiveSupplierIds = new HashSet<int>();

        // ── Constructors ─────────────────────────────────────────────────
        public MainInventory()
        {
            InitializeComponent();
            SetupDataGridColumns();
            InitializeDataGrids();
        }

        public MainInventory(string username, UserRole userRole) : this()
        {
            _currentUsername = username;
            _currentUserRole = userRole;
            lblWelcome.Text = $"Welcome, {username}!";
            UpdateUIForRole();
            this.Load += async (s, e) => await RefreshAllTablesAsync();
        }

        public MainInventory(string username) : this(username, UserRole.User) { }

        // ── Shared helpers ───────────────────────────────────────────────
        private async Task<HashSet<int>> GetInactiveSupplierIdsAsync()
        {
            using var service = new SupplierService();
            var all = await service.GetAllSuppliersAsync();
            return all.Where(s => !s.IsActive).Select(s => s.Id).ToHashSet();
        }

        private async Task<HashSet<int>> GetActiveProductIdsAsync(HashSet<int> inactiveSupplierIds = null)
        {
            inactiveSupplierIds ??= await GetInactiveSupplierIdsAsync();
            using var service = new ProductService();
            var all = await service.GetAllProductsAsync();
            return all.Where(p => !inactiveSupplierIds.Contains(p.SupplierId)).Select(p => p.Id).ToHashSet();
        }

        private async Task<List<CartItem>> GetCartItemsWithInactiveSuppliersAsync()
        {
            if (_cart.Count == 0) return new List<CartItem>();
            try
            {
                using var productService = new ProductService();
                var inactiveSupplierIds = await GetInactiveSupplierIdsAsync();
                var products = (await productService.GetAllProductsAsync()).ToDictionary(p => p.Id);
                return _cart
                    .Where(c => products.TryGetValue(c.ProductId, out var p) &&
                                inactiveSupplierIds.Contains(p.SupplierId))
                    .ToList();
            }
            catch { return new List<CartItem>(); }
        }

        // ── Grid styling ─────────────────────────────────────────────────
        private void CustomizeDataGrid(SfDataGrid grid)
        {
            grid.RowHeight = 36;
            grid.HeaderRowHeight = 40;
            grid.Style.HeaderStyle.BackColor = Color.FromArgb(49, 52, 113);
            grid.Style.HeaderStyle.TextColor = Color.White;
            grid.Style.HeaderStyle.Font.Bold = true;
            grid.Style.HeaderStyle.Font.Size = 12f;
            grid.Style.SelectionStyle.BackColor = Color.FromArgb(108, 117, 219);
            grid.Style.SelectionStyle.TextColor = Color.White;
            grid.QueryRowStyle += (sender, e) =>
            {
                if (e.RowType == RowType.DefaultRow)
                {
                    e.Style.BackColor = e.RowIndex % 2 == 0 ? Color.FromArgb(220, 230, 255) : Color.White;
                    e.Style.TextColor = Color.FromArgb(30, 30, 30);
                    e.Style.Font.Size = 11f;
                }
            };
        }

        // Uses the cached field — no async needed here
        private void ApplyInactiveSupplierRowStyle(SfDataGrid grid)
        {
            grid.QueryRowStyle += (sender, e) =>
            {
                if (e.RowType != RowType.DefaultRow) return;
                int dataIndex = e.RowIndex - 1;
                if (dataIndex < 0) return;
                try
                {
                    if (grid.DataSource is System.Collections.IList source && dataIndex < source.Count)
                    {
                        var row = source[dataIndex];
                        if (row is ProductDTO dto && _inactiveSupplierIds.Contains(dto.SupplierId))
                        {
                            e.Style.BackColor = Color.FromArgb(210, 210, 210);
                            e.Style.TextColor = Color.FromArgb(140, 140, 140);
                            e.Style.Font.Italic = true;
                        }
                    }
                }
                catch { }
            };
        }

        // ── Column definitions ───────────────────────────────────────────
        private void SetupDataGridColumns()
        {
            ConfigureGrid(UserAccount,
                new GridTextColumn { MappingName = "Id", HeaderText = "ID" },
                new GridTextColumn { MappingName = "Name", HeaderText = "Supplier Name" },
                new GridTextColumn { MappingName = "Email", HeaderText = "Email" },
                new GridTextColumn { MappingName = "Location", HeaderText = "Location" },
                new GridTextColumn { MappingName = "ContactNo", HeaderText = "Contact" },
                new GridCheckBoxColumn { MappingName = "IsActive", HeaderText = "Active" },
                new GridTextColumn { MappingName = "CreatedDate", HeaderText = "Added On", Format = "MM/dd/yyyy hh:mm tt" });

            ConfigureGrid(ProductTable,
                new GridTextColumn { MappingName = "Id", HeaderText = "ID" },
                new GridTextColumn { MappingName = "Name", HeaderText = "Product Name" },
                new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" },
                new GridTextColumn { MappingName = "Description", HeaderText = "Description" },
                new GridTextColumn { MappingName = "SupplierName", HeaderText = "Supplier" });

            ConfigureGrid(accountsListTable,
                new GridTextColumn { MappingName = "Id", HeaderText = "ID" },
                new GridTextColumn { MappingName = "Username", HeaderText = "Username" },
                new GridTextColumn { MappingName = "Email", HeaderText = "Email" },
                new GridCheckBoxColumn { MappingName = "IsActive", HeaderText = "Active" },
                new GridTextColumn { MappingName = "Role", HeaderText = "Role" },
                new GridTextColumn { MappingName = "CreatedAt", HeaderText = "Date Added", Format = "MM/dd/yyyy hh:mm tt" });

            ConfigureGrid(ProductListToStockTable,
                new GridTextColumn { MappingName = "Name", HeaderText = "Product Name" },
                new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" },
                new GridTextColumn { MappingName = "Description", HeaderText = "Description" },
                new GridTextColumn { MappingName = "SupplierName", HeaderText = "Supplier" });

            ConfigureGrid(StockTable,
                new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" },
                new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty Restocked" },
                new GridTextColumn { MappingName = "CreatedDate", HeaderText = "Date Added", Format = "MM/dd/yyyy hh:mm tt" });

            ConfigureGrid(SalesTable,
                new GridTextColumn { MappingName = "PurchaseId", HeaderText = "Purchase #" },
                new GridTextColumn { MappingName = "PurchasedOn", HeaderText = "Date", Format = "MM/dd/yyyy hh:mm tt" },
                new GridTextColumn { MappingName = "ProductName", HeaderText = "Product" },
                new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty" },
                new GridTextColumn { MappingName = "UnitPrice", HeaderText = "Unit Price", Format = "C2" },
                new GridTextColumn { MappingName = "Subtotal", HeaderText = "Subtotal", Format = "C2" },
                new GridTextColumn { MappingName = "PurchaseTotal", HeaderText = "Total", Format = "C2" },
                new GridTextColumn { MappingName = "PaymentMethod", HeaderText = "Payment" },
                new GridTextColumn { MappingName = "CashierName", HeaderText = "Cashier" },
                new GridTextColumn { MappingName = "CashierRole", HeaderText = "Role" });

            ConfigureGrid(StockViewTable,
                new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" },
                new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" },
                new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty Available" });

            ConfigureGrid(ProductsToPurchaseTable,
                new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" },
                new GridTextColumn { MappingName = "Price", HeaderText = "Unit Price", Format = "C2" },
                new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty to Buy" },
                new GridTextColumn { MappingName = "Subtotal", HeaderText = "Subtotal", Format = "C2" });

            ConfigureGrid(PurchaseTable,
                new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" },
                new GridTextColumn { MappingName = "Price", HeaderText = "Unit Price", Format = "C2" },
                new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty to Buy" },
                new GridTextColumn { MappingName = "Subtotal", HeaderText = "Subtotal", Format = "C2" });

            ConfigureGrid(ProductTableLowStock,
                new GridTextColumn { MappingName = "ProductId", HeaderText = "ID" },
                new GridTextColumn { MappingName = "ProductName", HeaderText = "Product" },
                new GridTextColumn { MappingName = "AvailableQty", HeaderText = "Stock Left" },
                new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" },
                new GridTextColumn { MappingName = "SupplierName", HeaderText = "Supplier" });

            ConfigureGrid(MostSoldProductsTable,
                new GridTextColumn { MappingName = "Rank", HeaderText = "#" },
                new GridTextColumn { MappingName = "ProductName", HeaderText = "Product" },
                new GridTextColumn { MappingName = "TotalSold", HeaderText = "Qty Sold" },
                new GridTextColumn { MappingName = "Revenue", HeaderText = "Revenue", Format = "C2" });
        }

        private static void ConfigureGrid(SfDataGrid grid, params GridColumn[] columns)
        {
            grid.Columns.Clear();
            grid.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            foreach (var col in columns)
                grid.Columns.Add(col);
        }

        // ── Grid initialization ──────────────────────────────────────────
        private void InitializeDataGrids()
        {
            var allGrids = new[]
            {
                    UserAccount, ProductTable, ProductListToStockTable, StockTable,
                    SalesTable, StockViewTable, ProductsToPurchaseTable, PurchaseTable,
                    accountsListTable, MostSoldProductsTable, ProductTableLowStock
                };

            comboBoxSales.Items.AddRange(new object[]
            {
                "Today", "This Week", "This Month",
                "3 Months", "6 Months", "12 Months", "All Time"
            });
            comboBoxSales.SelectedIndex = 2; // defaults to This Month
            comboBoxSales.SelectedIndexChanged += async (s, e) =>
            {
                await RefreshSalesChartAsync(comboBoxSales.SelectedItem?.ToString() ?? "This Month");
            };

            foreach (var grid in allGrids)
            {
                grid.AutoGenerateColumns = false;
                grid.AllowEditing = false;
                grid.AllowGrouping = false;
                grid.AllowFiltering = true;
                grid.AllowSorting = true;
                CustomizeDataGrid(grid);
            }

            MostSoldProductsTable.CellDoubleClick += MostSoldProductsTable_CellDoubleClick;
            ProductTableLowStock.CellDoubleClick += ProductTableLowStock_CellDoubleClick;

            SalesTable.CellDoubleClick += SalesTable_CellDoubleClick;

            foreach (var grid in new[] { ProductListToStockTable, StockTable, SalesTable,
                                         accountsListTable, MostSoldProductsTable })
                grid.SelectionMode = GridSelectionMode.Single;

            accountsListTable.SelectionChanged += (s, e) =>
            {
                _selectedAccount = accountsListTable.SelectedItem as AccountDisplayDTO;
            };

            UserAccount.SelectionMode = GridSelectionMode.Extended;
            ProductTable.SelectionMode = GridSelectionMode.Extended;

            UserAccount.CellDoubleClick += SupplierTable_CellDoubleClick;
            ProductTable.CellDoubleClick += ProductTable_CellDoubleClick;

            ProductsToPurchaseTable.SelectionChanged += ProductsToPurchaseTable_SelectionChanged;

            ApplyInactiveSupplierRowStyle(ProductTable);

            MostSoldProductsTable.RowHeight = 28;
            MostSoldProductsTable.HeaderRowHeight = 32;
            MostSoldProductsTable.Style.HeaderStyle.Font.Size = 10f;
            MostSoldProductsTable.QueryRowStyle += (sender, e) =>
            {
                if (e.RowType == RowType.DefaultRow)
                    e.Style.Font.Size = 9f;
            };
        }

        private void SalesTable_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.DataRow.RowType != RowType.DefaultRow) return;
            if (e.DataRow.RowData is not SalesLineItemDto dto) return;

            string refLine = dto.PaymentMethod == "Cash"
                ? ""
                : $"\nReference No.: {dto.ReferenceNumber ?? "N/A"}";

            MessageBox.Show(
                $"Purchase #:    {dto.PurchaseId}\n" +
                $"Date:          {dto.PurchasedOn:MM/dd/yyyy hh:mm tt}\n" +
                $"Product:       {dto.ProductName}\n" +
                $"Qty:           {dto.Quantity}\n" +
                $"Unit Price:    ₱{dto.UnitPrice:N2}\n" +
                $"Subtotal:      ₱{dto.Subtotal:N2}\n" +
                $"Total:         ₱{dto.PurchaseTotal:N2}\n" +
                $"Payment:       {dto.PaymentMethod}" +
                refLine + "\n" +
                $"Cashier:       {dto.CashierName}\n" +
                $"Role:          {dto.CashierRole}",
                "Sale Details",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void MostSoldProductsTable_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.DataRow.RowType != RowType.DefaultRow) return;
            var row = e.DataRow.RowData;
            var type = row.GetType();
            var rank = type.GetProperty("Rank")?.GetValue(row);
            var productName = type.GetProperty("ProductName")?.GetValue(row);
            var totalSold = type.GetProperty("TotalSold")?.GetValue(row);
            var revenue = type.GetProperty("Revenue")?.GetValue(row);
            MessageBox.Show(
                $"Rank:        #{rank}\n" +
                $"Product:     {productName}\n" +
                $"Qty Sold:    {totalSold}\n" +
                $"Revenue:     ₱{revenue:N2}",
                "Top Product Details",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ProductTableLowStock_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.DataRow.RowType != RowType.DefaultRow) return;
            var row = e.DataRow.RowData;
            var type = row.GetType();
            var productId = type.GetProperty("ProductId")?.GetValue(row);
            var productName = type.GetProperty("ProductName")?.GetValue(row);
            var availableQty = type.GetProperty("AvailableQty")?.GetValue(row);
            var price = type.GetProperty("Price")?.GetValue(row);
            var supplierName = type.GetProperty("SupplierName")?.GetValue(row);
            MessageBox.Show(
                $"Product ID:  {productId}\n" +
                $"Product:     {productName}\n" +
                $"Stock Left:  {availableQty}\n" +
                $"Price:       ₱{price:N2}\n" +
                $"Supplier:    {supplierName}",
                "Low Stock Details",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        // ── Role-based UI ────────────────────────────────────────────────
        private void UpdateUIForRole()
        {
            bool isAdmin = _currentUserRole == UserRole.Admin;
            btnAccounts.Enabled = isAdmin;
            btnAccounts.Visible = isAdmin;
            btnYourAccount.Visible = isAdmin;
        }

        private bool IsAdmin()
        {
            if (_currentUserRole == UserRole.Admin) return true;
            MessageBox.Show("Admin access required.", "Access Denied",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        // ── Navigation ───────────────────────────────────────────────────
        private void HighlightButton(Button active, params Button[] group)
        {
            Color off = Color.FromArgb(49, 52, 113);
            Color on = Color.FromArgb(108, 117, 219);
            foreach (var btn in group) btn.BackColor = off;
            active.BackColor = on;
        }

        private void btnDashboard_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 0; HighlightButton((Button)sender, btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase); }
        private void btnStock_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 1; HighlightButton((Button)sender, btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase); }
        private void btnSupplier_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 2; HighlightButton((Button)sender, btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase); }
        private void btnProducts_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 3; HighlightButton((Button)sender, btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase); }
        private void btnPurchase_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 4; HighlightButton((Button)sender, btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase); }
        private void btnSales_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 5; HighlightButton((Button)sender, btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase); }
        private void btnManagePurchase_Click(object sender, EventArgs e) { PurchaseControl.SelectedIndex = 0; HighlightButton((Button)sender, btnManagePurchase, btnPurchaseCheckout); }
        private void btnPurchaseCheckout_Click(object sender, EventArgs e) { PurchaseControl.SelectedIndex = 1; HighlightButton((Button)sender, btnManagePurchase, btnPurchaseCheckout); }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            PanelControl.SelectedIndex = 6;
            HighlightButton((Button)sender, btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            var login = new LoginForm();
            login.Closed += (s, args) => this.Close();
            login.Show();
        }

        // ── Refresh ──────────────────────────────────────────────────────
        private async Task RefreshAllTablesAsync()
        {
            _inactiveSupplierIds = await GetInactiveSupplierIdsAsync();
            await RefreshSupplierTableAsync();
            await RefreshProductTableAsync();
            await RefreshStockTableAsync();
            await RefreshSalesTableAsync();
            await RefreshStockViewTableAsync();
            await RefreshDashboardAsync();
            await RefreshAccountsTableAsync();
        }

        private async Task RefreshDashboardAsync()
        {
            await RefreshTotalProductsCountAsync();
            await RefreshMonthlySalesAsync();
            await RefreshLowStockTableAsync();
            await RefreshSalesChartAsync();
            await RefreshMostSoldProductsTableAsync();
            await RefreshSalesChartAsync(comboBoxSales.SelectedItem?.ToString() ?? "This Month");
        }

        public async Task RefreshAccountsTableAsync()
        {
            using var service = new AccountService();
            accountsListTable.DataSource = await service.GetAllAccountsAsync();
        }

        public async Task RefreshSupplierTableAsync()
        {
            using var service = new SupplierService();
            var all = await service.GetAllSuppliersAsync();
            UserAccount.DataSource = all.OrderBy(s => s.Id).ToList();
        }

        public async Task RefreshProductTableAsync()
        {
            using var productService = new ProductService();
            var products = await productService.GetAllProductsAsync();
            ProductTable.DataSource = products;
            ProductListToStockTable.DataSource = products
                .Where(p => !_inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();
        }

        public async Task RefreshSalesTableAsync()
        {
            using var service = new PurchaseService();
            SalesTable.DataSource = await service.GetAllSalesAsync();
        }

        public async Task RefreshStockTableAsync()
        {
            using var stockService = new StockService();
            var activeProductIds = await GetActiveProductIdsAsync(_inactiveSupplierIds);
            var allStock = (await stockService.GetAllStockAsync())
                .Where(s => activeProductIds.Contains(s.ProductId))
                .Select(s =>
                {
                    int cartQty = _cart.FirstOrDefault(c => c.ProductId == s.ProductId)?.Quantity ?? 0;
                    return new StockDTO
                    {
                        Id = s.Id,
                        ProductId = s.ProductId,
                        ProductName = s.ProductName,
                        Quantity = Math.Max(0, s.Quantity - cartQty),
                        CreatedDate = s.CreatedDate
                    };
                })
                .ToList();

            StockTable.DataSource = allStock;
        }

        public async Task RefreshStockViewTableAsync()
        {
            StockViewTable.DataSource = await BuildStockViewAsync(filterText: null);
        }

        private async Task<List<StockViewDTO>> BuildStockViewAsync(string filterText)
        {
            using var stockService = new StockService();
            using var productService = new ProductService();

            var products = (await productService.GetAllProductsAsync())
                .Where(p => !_inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();

            var availabilities = await Task.WhenAll(
                products.Select(p => stockService.GetAvailableStockAsync(p.Id)));

            var view = products.Select((p, i) =>
            {
                int cartQty = _cart.FirstOrDefault(c => c.ProductId == p.Id)?.Quantity ?? 0;
                return new StockViewDTO
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Price = p.Price,
                    Quantity = Math.Max(0, availabilities[i] - cartQty),
                    Description = p.Description,
                    SupplierName = p.SupplierName
                };
            })
            .Where(v => v.Quantity > 0 || _cart.Any(c => c.ProductId == v.ProductId))
            .OrderBy(v => v.ProductName)
            .ToList();

            if (string.IsNullOrWhiteSpace(filterText)) return view;

            return view.Where(v =>
                v.ProductName.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                v.ProductId.ToString().Contains(filterText) ||
                v.SupplierName.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                v.Price.ToString().Contains(filterText))
            .ToList();
        }

        // ── Dashboard widgets ────────────────────────────────────────────
        private async Task RefreshTotalProductsCountAsync()
        {
            using var service = new ProductService();
            var all = await service.GetAllProductsAsync();
            txtTotalProducts.Text = all.Count.ToString();
        }

        private async Task RefreshMonthlySalesAsync()
        {
            var now = DateTime.Now;
            using var context = new InvSys.Infrastructure.InventoryDbContext();
            var sales = await context.Sales
                .Where(s => s.CreatedDate.Month == now.Month && s.CreatedDate.Year == now.Year)
                .ToListAsync();
            txtMonthlySales.Text = $"₱{sales.Sum(s => s.Subtotal):N2}";
        }

        private async Task RefreshMostSoldProductsTableAsync()
        {
            var now = DateTime.Now;
            using var context = new InvSys.Infrastructure.InventoryDbContext();

            var salesThisMonth = await context.Sales
                .Where(s => s.CreatedDate.Month == now.Month && s.CreatedDate.Year == now.Year)
                .ToListAsync();

            var top10 = salesThisMonth
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(s => s.Quantity), Revenue = g.Sum(s => s.Subtotal) })
                .OrderByDescending(x => x.TotalSold)
                .Take(10)
                .ToList();

            var productIds = top10.Select(t => t.ProductId).ToList();
            var productNames = await context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Name);

            MostSoldProductsTable.DataSource = top10
                .Select((x, i) => new
                {
                    Rank = i + 1,
                    ProductName = productNames.TryGetValue(x.ProductId, out var name) ? name : "Unknown",
                    TotalSold = x.TotalSold,
                    Revenue = x.Revenue
                })
                .ToList();
        }

        private async Task RefreshLowStockTableAsync()
        {
            using var stockService = new StockService();
            using var productService = new ProductService();
            using var supplierService = new SupplierService();

            var activeSupplierIds = (await supplierService.GetAllSuppliersAsync())
                .Where(s => s.IsActive).Select(s => s.Id).ToHashSet();

            var products = await productService.GetAllProductsAsync();
            var allStock = await stockService.GetAllStockAsync();

            var availabilities = await Task.WhenAll(
                allStock.Select(s => stockService.GetAvailableStockAsync(s.ProductId)));

            var lowStock = allStock.Select((s, i) =>
            {
                var product = products.FirstOrDefault(p => p.Id == s.ProductId);
                return new
                {
                    s.ProductId,
                    s.ProductName,
                    AvailableQty = availabilities[i],
                    Price = product?.Price ?? 0m,
                    SupplierName = product?.SupplierName ?? "Unknown",
                    SupplierId = product?.SupplierId ?? 0
                };
            })
            .Where(x => x.AvailableQty < 10 && activeSupplierIds.Contains(x.SupplierId))
            .OrderBy(x => x.AvailableQty)
            .Select(x => new { x.ProductId, x.ProductName, x.AvailableQty, x.Price, x.SupplierName })
            .ToList();

            ProductTableLowStock.DataSource = lowStock;
        }

        private async Task RefreshSalesChartAsync(string filter = "This Month")
        {
            var now = DateTime.Now;
            using var context = new InvSys.Infrastructure.InventoryDbContext();

            var query = context.Sales.AsQueryable();

            query = filter switch
            {
                "Today" => query.Where(s => s.CreatedDate.Date == now.Date),
                "This Week" => query.Where(s => s.CreatedDate >= now.Date.AddDays(-(int)now.DayOfWeek) && s.CreatedDate <= now),
                "This Month" => query.Where(s => s.CreatedDate.Month == now.Month && s.CreatedDate.Year == now.Year),
                "3 Months" => query.Where(s => s.CreatedDate >= now.AddMonths(-3)),
                "6 Months" => query.Where(s => s.CreatedDate >= now.AddMonths(-6)),
                "12 Months" => query.Where(s => s.CreatedDate >= now.AddMonths(-12)),
                _ => query // All Time
            };

            var salesData = await query.ToListAsync();

            var soldProductIds = salesData.Select(s => s.ProductId).Distinct().ToList();
            var products = await context.Products
                .Where(p => soldProductIds.Contains(p.Id))
                .ToListAsync();

            chartMostSold.Series.Clear();
            chartMostSold.ChartAreas[0].AxisX.Title = "Day";
            chartMostSold.ChartAreas[0].AxisY.Title = "Qty Sold";
            chartMostSold.ChartAreas[0].BackColor = Color.White;
            chartMostSold.BackColor = Color.White;
            chartMostSold.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);
            chartMostSold.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);

            if (!salesData.Any())
            {
                chartMostSold.Series.Add(new Series { Name = "No Sales", ChartType = SeriesChartType.Line, Color = Color.LightGray });
                return;
            }

            // Determine X axis grouping based on filter
            DateTime minDate = salesData.Min(s => s.CreatedDate).Date;
            DateTime maxDate = salesData.Max(s => s.CreatedDate).Date;

            chartMostSold.ChartAreas[0].AxisX.Minimum = minDate.ToOADate();
            chartMostSold.ChartAreas[0].AxisX.Maximum = maxDate.ToOADate();
            chartMostSold.ChartAreas[0].AxisX.LabelStyle.Format = filter == "Today" ? "hh tt" : "MM/dd";
            chartMostSold.ChartAreas[0].AxisX.IntervalType = filter == "Today"
                ? DateTimeIntervalType.Hours : DateTimeIntervalType.Days;
            chartMostSold.ChartAreas[0].AxisX.Interval = filter switch
            {
                "Today" => 2,
                "This Week" => 1,
                "12 Months" => 30,
                "6 Months" => 15,
                "3 Months" => 7,
                _ => 1
            };

            var colors = new[]
            {
        Color.FromArgb(49,  52,  113), Color.FromArgb(108, 117, 219),
        Color.FromArgb(220, 80,  80),  Color.FromArgb(80,  180, 120),
        Color.FromArgb(240, 160, 40),  Color.FromArgb(80,  180, 220),
        Color.FromArgb(180, 80,  180), Color.FromArgb(40,  140, 180)
    };

            int colorIndex = 0;
            foreach (var product in products)
            {
                var series = new Series
                {
                    Name = product.Name,
                    ChartType = SeriesChartType.Line,
                    Color = colors[colorIndex++ % colors.Length],
                    BorderWidth = 2,
                    IsVisibleInLegend = true,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6,
                    XValueType = ChartValueType.DateTime
                };

                if (filter == "Today")
                {
                    for (int hour = 0; hour < 24; hour++)
                    {
                        int qty = salesData
                            .Where(s => s.ProductId == product.Id && s.CreatedDate.Hour == hour)
                            .Sum(s => s.Quantity);
                        series.Points.AddXY(now.Date.AddHours(hour).ToOADate(), qty);
                    }
                }
                else
                {
                    for (DateTime d = minDate; d <= maxDate; d = d.AddDays(1))
                    {
                        int qty = salesData
                            .Where(s => s.ProductId == product.Id && s.CreatedDate.Date == d)
                            .Sum(s => s.Quantity);
                        series.Points.AddXY(d.ToOADate(), qty);
                    }
                }

                chartMostSold.Series.Add(series);
            }

            chartMostSold.Legends[0].BackColor = Color.White;
            chartMostSold.Legends[0].Font = new Font("Segoe UI", 8.5f);
            chartMostSold.Legends[0].Docking = Docking.Bottom;
        }

        // ── Supplier CRUD ────────────────────────────────────────────────
        private async void btnAddSupplier_Click(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            var form = new AddSupplier(this);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _inactiveSupplierIds = await GetInactiveSupplierIdsAsync();
                await RefreshSupplierTableAsync();
            }
        }

        private async void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            if (UserAccount.SelectedItem is not SupplierDTO dto)
            {
                MessageBox.Show("Please select a supplier to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var form = new UpdateSupplier(this);
            form.LoadSelectedSupplier(dto);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _inactiveSupplierIds = await GetInactiveSupplierIdsAsync();
                await RefreshSupplierTableAsync();
            }
        }

        private async void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            var selected = UserAccount.SelectedItems?.Cast<SupplierDTO>().ToList();
            if (selected == null || selected.Count == 0)
            {
                MessageBox.Show("Please select at least one supplier to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string names = string.Join(", ", selected.Select(s => s.Name));
            if (MessageBox.Show($"Delete {selected.Count} supplier(s)?\n\n{names}", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            using var productService = new ProductService();
            var allProducts = await productService.GetAllProductsAsync();
            var blockedProducts = allProducts
                .Where(p => selected.Any(s => s.Id == p.SupplierId))
                .ToList();

            if (blockedProducts.Count > 0)
            {
                string productNames = string.Join("\n  • ", blockedProducts.Select(p => p.Name));
                MessageBox.Show(
                    $"Cannot delete the selected supplier(s) because the following products are still associated:\n\n  • {productNames}\n\nPlease delete those products first, or consider deactivating the supplier instead.",
                    "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new InvSys.Infrastructure.InventoryDbContext();

                foreach (var s in selected)
                {
                    var supplier = await context.Suppliers
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(sup => sup.Id == s.Id);
                    if (supplier != null)
                        context.Suppliers.Remove(supplier);
                }

                await context.SaveChangesAsync();

                MessageBox.Show($"{selected.Count} supplier(s) deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _inactiveSupplierIds = await GetInactiveSupplierIdsAsync();
                await RefreshSupplierTableAsync();
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.InnerException?.Message
                                ?? ex.InnerException?.Message
                                ?? ex.Message;
                MessageBox.Show($"Delete failed:\n\n{innerMessage}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void SupplierTable_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (!IsAdmin()) return;
            if (e.DataRow.RowType != RowType.DefaultRow || e.DataRow.RowData is not SupplierDTO dto) return;

            var form = new UpdateSupplier(this);
            form.LoadSelectedSupplier(dto);

            // FormClosing is sync — check cart against cached product list
            form.FormClosing += (fs, fe) =>
            {
                if (form.DialogResult != DialogResult.OK || !form.IsMarkingInactive) return;
                var supplierProductIds = new ProductService()
                    .GetAllProductsAsync().GetAwaiter().GetResult()
                    .Where(p => p.SupplierId == dto.Id)
                    .Select(p => p.Id)
                    .ToHashSet();
                if (_cart.Any(c => supplierProductIds.Contains(c.ProductId)))
                {
                    MessageBox.Show(
                        "Cannot deactivate this supplier — one or more of their products " +
                        "are currently in the purchase cart.\n\n" +
                        "Please reset or complete the current transaction first.",
                        "Cannot Deactivate Supplier",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    fe.Cancel = true;
                }
            };

            if (form.ShowDialog() == DialogResult.OK)
            {
                _inactiveSupplierIds = await GetInactiveSupplierIdsAsync();
                await RefreshAllTablesAsync();
            }
        }

        private async void txtBoxSupplierSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtBoxSupplierSearch.Text.Trim();
            using var service = new SupplierService();
            var all = await service.GetAllSuppliersAsync();
            UserAccount.DataSource = string.IsNullOrEmpty(search)
                ? all.OrderBy(s => s.Id).ToList()
                : all.Where(s =>
                    s.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Email.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Location.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.ContactNo.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                  .OrderBy(s => s.Name).ToList();
        }

        // ── Product CRUD ─────────────────────────────────────────────────
        private async void btnAddProduct_Click_1(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            var form = new AddProduct(this);
            if (form.ShowDialog() == DialogResult.OK)
            {
                await RefreshProductTableAsync();
                await RefreshDashboardAsync();
            }
        }

        private async void btnUpdateProduct_Click_1(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            if (ProductTable.SelectedItem is not ProductDTO dto)
            {
                MessageBox.Show("Please select a product to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var form = new UpdateProduct(this);
            form.LoadSelectedProduct(dto);
            if (form.ShowDialog() == DialogResult.OK)
            {
                await RefreshProductTableAsync();
                await RefreshDashboardAsync();
            }
        }

        private async void btnDeleteProduct_Click_1(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            var selected = ProductTable.SelectedItems?.Cast<ProductDTO>().ToList();
            if (selected == null || selected.Count == 0)
            {
                MessageBox.Show("Please select at least one product to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string names = string.Join(", ", selected.Select(p => p.Name));
            if (MessageBox.Show($"Delete {selected.Count} product(s)?\n\n{names}", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                using var service = new ProductService();
                foreach (var p in selected)
                    await service.DeleteProductAsync(p.Id);
                MessageBox.Show($"{selected.Count} product(s) deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RefreshProductTableAsync();
                await RefreshDashboardAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ProductTable_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (!IsAdmin()) return;
            if (e.DataRow.RowType != RowType.DefaultRow || ProductTable.SelectedItem is not ProductDTO dto) return;
            var form = new UpdateProduct(this);
            form.LoadSelectedProduct(dto);
            if (form.ShowDialog() == DialogResult.OK)
            {
                await RefreshProductTableAsync();
                await RefreshDashboardAsync();
            }
        }

        private async void txtBoxProductSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtBoxProductSearch.Text.Trim();
            using var service = new ProductService();
            var all = await service.GetAllProductsAsync();
            ProductTable.DataSource = string.IsNullOrEmpty(search)
                ? all
                : all.Where(p =>
                    p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.SupplierName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Price.ToString().Contains(search))
                  .OrderBy(p => p.Name).ToList();
        }

        // ── Stock info panel ─────────────────────────────────────────────
        private void ProductListToStockTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListToStockTable.SelectedItem is not ProductDTO p) return;
            txtSelectedProductID.Text = $"ID: {p.Id}";
            txtSelectedProductName.Text = $"Name: {p.Name}";
            txtSelectedProductPrice.Text = $"Price: {p.Price:C2}";
            txtSelectedProductDescription.Text = $"Description: {p.Description}";
            txtSelectedProductSupplier.Text = $"Supplier: {p.SupplierName}";
        }

        // ── Stock CRUD ───────────────────────────────────────────────────
        private bool TryParseQuantity(string input, out int quantity)
        {
            quantity = 0;
            if (string.IsNullOrWhiteSpace(input))
            { MessageBox.Show("Quantity cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (!int.TryParse(input.Trim(), out quantity))
            { MessageBox.Show("Quantity must be a whole number (e.g. 10).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (quantity <= 0)
            { MessageBox.Show("Quantity must be greater than zero.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        private async void btnAddStock_Click(object sender, EventArgs e)
        {
            if (ProductListToStockTable.SelectedItem is not ProductDTO product)
            { MessageBox.Show("Please select a product first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!TryParseQuantity(txtBoxQuantityAdd.Text, out int qty)) return;
            try
            {
                using var service = new StockService();
                await service.RestockAsync(product.Id, qty);
                MessageBox.Show($"Added {qty} unit(s) to '{product.Name}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxQuantityAdd.Clear();
                await RefreshStockTableAsync();
                await RefreshStockViewTableAsync();
                await RefreshDashboardAsync();
            }
            catch (Exception ex) { MessageBox.Show($"Failed to add stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private async void btnUpdateStock_Click(object sender, EventArgs e)
        {
            if (StockTable.SelectedItem is not StockDTO stock)
            {
                MessageBox.Show("Please select a stock entry to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var inputDialog = new CRUDForms.UpdateStockDialog(stock.ProductName, stock.Quantity);
            if (inputDialog.ShowDialog(this) != DialogResult.OK) return;
            int desiredAvailable = inputDialog.EnteredQuantity;

            try
            {
                // Find out how many have been sold for this product
                using var context = new InvSys.Infrastructure.InventoryDbContext();
                int sold = await context.Sales
                    .Where(s => s.ProductId == stock.ProductId)
                    .SumAsync(s => (int?)s.Quantity) ?? 0;

                // Desired available = raw stock - sold  →  raw stock = desired + sold
                int rawStockToSet = desiredAvailable + sold;

                using var service = new StockService();
                await service.UpdateStockAsync(stock.Id, rawStockToSet);

                MessageBox.Show("Stock updated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                await RefreshStockTableAsync();
                await RefreshStockViewTableAsync();
                await RefreshDashboardAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update stock: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDeleteStock_Click(object sender, EventArgs e)
        {
            if (StockTable.SelectedItem is not StockDTO stock)
            { MessageBox.Show("Please select a stock entry to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (MessageBox.Show("Delete this stock entry?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                using var service = new StockService();
                await service.DeleteStockAsync(stock.Id);
                MessageBox.Show("Stock entry deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RefreshStockTableAsync();
                await RefreshStockViewTableAsync();
                await RefreshDashboardAsync();
            }
            catch (Exception ex) { MessageBox.Show($"Failed to delete stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Purchase / Cart ──────────────────────────────────────────────
        private bool TryParsePurchaseQuantity(string input, int availableQty, out int quantity)
        {
            quantity = 0;
            if (string.IsNullOrWhiteSpace(input))
            { MessageBox.Show("Please enter a purchase quantity.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (!int.TryParse(input.Trim(), out quantity))
            { MessageBox.Show("Quantity must be a whole number (e.g. 3).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (quantity <= 0)
            { MessageBox.Show("Quantity must be greater than zero.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (quantity > availableQty)
            { MessageBox.Show($"Requested quantity ({quantity}) exceeds available stock ({availableQty}).", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            return true;
        }

        private void RefreshTotalAmount()
        {
            txtTotalAmount.Text = $"Total Amount: ₱{_cart.Sum(c => c.Subtotal):N2}";
        }

        private void RefreshCartTables()
        {
            var snapshot = _cart.ToList();
            ProductsToPurchaseTable.DataSource = null;
            ProductsToPurchaseTable.DataSource = snapshot;
            PurchaseTable.DataSource = null;
            PurchaseTable.DataSource = snapshot;
        }

        private async void btnAddPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                if (_lastReceiptData != null)
                { MessageBox.Show("A previous transaction has not been reset yet.\n\nPlease click 'Reset Transaction' before starting a new purchase.", "Reset Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (StockViewTable.SelectedItem is not StockViewDTO selected)
                { MessageBox.Show("Please select a product from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                int selectedProductId = selected.ProductId;
                if (!TryParsePurchaseQuantity(txtBoxPurchaseQuantity.Text, selected.Quantity, out int qty)) return;

                var existing = _cart.FirstOrDefault(c => c.ProductId == selected.ProductId);
                if (existing != null)
                {
                    if (qty > selected.Quantity)
                    { MessageBox.Show($"Cannot add {qty} more. Only {selected.Quantity} unit(s) still available.", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    existing.Quantity += qty;
                    existing.Subtotal = existing.Price * existing.Quantity;
                }
                else
                {
                    _cart.Add(new CartItem
                    {
                        ProductId = selected.ProductId,
                        ProductName = selected.ProductName,
                        Price = selected.Price,
                        Quantity = qty,
                        Subtotal = selected.Price * qty
                    });
                }

                txtBoxPurchaseQuantity.Clear();
                RefreshCartTables();
                await RefreshStockViewTableAsync();
                await RefreshStockTableAsync();
                RefreshTotalAmount();

                var updated = (StockViewTable.DataSource as List<StockViewDTO>)
                    ?.FirstOrDefault(v => v.ProductId == selectedProductId);
                if (updated != null)
                    StockViewTable.SelectedIndex = (StockViewTable.DataSource as List<StockViewDTO>).IndexOf(updated);

                SyncPurchaseInfoLabelsToSelection();
                MessageBox.Show("Cart item added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add item to cart:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnUpdatePurchase_Click(object sender, EventArgs e)
        {
            if (ProductsToPurchaseTable.SelectedItem is not CartItem cartItem)
            { MessageBox.Show("Please select an item in the cart to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            using var svc = new StockService();
            int rawAvailable = await svc.GetAvailableStockAsync(cartItem.ProductId);
            int cartAlready = _cart.FirstOrDefault(c => c.ProductId == cartItem.ProductId)?.Quantity ?? 0;
            int maxAllowed = rawAvailable + cartAlready; // add back what's already reserved

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Enter new quantity for '{cartItem.ProductName}'.\nAvailable stock: {maxAllowed}",
                "Update Purchase Quantity", cartItem.Quantity.ToString());

            if (string.IsNullOrWhiteSpace(input)) return;
            if (!int.TryParse(input.Trim(), out int newQty))
            { MessageBox.Show("Quantity must be a whole number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (newQty <= 0)
            { MessageBox.Show("Quantity must be greater than zero.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (newQty > maxAllowed)
            { MessageBox.Show($"Quantity ({newQty}) exceeds available stock ({maxAllowed}).", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var item = _cart.FirstOrDefault(c => c.ProductId == cartItem.ProductId);
            if (item == null) return;
            item.Quantity = newQty;
            item.Subtotal = item.Price * newQty;

            RefreshCartTables();
            await RefreshStockViewTableAsync();
            await RefreshStockTableAsync();
            SyncPurchaseInfoLabelsToSelection();
            RefreshTotalAmount();
            MessageBox.Show($"'{cartItem.ProductName}' updated to {newQty}.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnDeletePurchase_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProductsToPurchaseTable.SelectedItem is not CartItem cartItem)
                { MessageBox.Show("Please select an item in the cart to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show($"Remove '{cartItem.ProductName}' from cart?", "Confirm Remove",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                _cart.RemoveAll(c => c.ProductId == cartItem.ProductId);
                txtBoxPurchaseQuantity.Clear();
                RefreshCartTables();
                await RefreshStockViewTableAsync();
                await RefreshStockTableAsync();
                SyncPurchaseInfoLabelsToSelection();
                RefreshTotalAmount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove cart item:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnResetPurchase_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            { MessageBox.Show("The cart is already empty.", "Nothing to Reset", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (MessageBox.Show("Clear all items from the cart?", "Confirm Reset",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            _cart.Clear();
            txtBoxPurchaseQuantity.Clear();
            ClearPurchaseInfoLabels();
            RefreshCartTables();
            await RefreshStockViewTableAsync();
            await RefreshStockTableAsync();
            RefreshTotalAmount();
        }

        // ── Purchase info labels ─────────────────────────────────────────
        private void ClearPurchaseInfoLabels()
        {
            txtFromPurchaseProductID.Text = "Product ID:";
            txtFromPurchaseProductName.Text = "Product Name:";
            txtFromPurchaseProductQuantity.Text = "Quantity Available:";
            txtFromPurchaseProductPrice.Text = "Price:";
            txtFromPurchaseProductDescription.Text = "Description:";
            txtFromPurchaseProductSupplier.Text = "Supplier:";
        }

        private void UpdatePurchaseInfoLabels(StockViewDTO item)
        {
            txtFromPurchaseProductID.Text = $"Product ID: {item.ProductId}";
            txtFromPurchaseProductName.Text = $"Product Name: {item.ProductName}";
            txtFromPurchaseProductQuantity.Text = $"Quantity Available: {item.Quantity}";
            txtFromPurchaseProductPrice.Text = $"Price: ₱{item.Price:N2}";
            txtFromPurchaseProductDescription.Text = $"Description: {item.Description}";
            txtFromPurchaseProductSupplier.Text = $"Supplier: {item.SupplierName}";
        }

        private void SyncPurchaseInfoLabelsToSelection()
        {
            if (StockViewTable.SelectedItem is StockViewDTO item)
                UpdatePurchaseInfoLabels(item);
            else
                ClearPurchaseInfoLabels();
        }

        private void StockViewTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StockViewTable.SelectedItem is StockViewDTO item)
                UpdatePurchaseInfoLabels(item);
            else
                ClearPurchaseInfoLabels();
        }

        private async void ProductsToPurchaseTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsToPurchaseTable.SelectedItem is not CartItem cartItem)
            { ClearPurchaseInfoLabels(); return; }

            try
            {
                using var stockService = new StockService();
                using var productService = new ProductService();
                int available = await stockService.GetAvailableStockAsync(cartItem.ProductId);
                var product = await productService.GetProductByIdAsync(cartItem.ProductId);

                txtFromPurchaseProductID.Text = $"Product ID: {cartItem.ProductId}";
                txtFromPurchaseProductName.Text = $"Product Name: {cartItem.ProductName}";
                txtFromPurchaseProductQuantity.Text = $"Quantity Available: {available}";
                txtFromPurchaseProductPrice.Text = $"Price: ₱{cartItem.Price:N2}";
                txtFromPurchaseProductDescription.Text = $"Description: {product?.Description ?? "N/A"}";
                txtFromPurchaseProductSupplier.Text = $"Supplier: {product?.SupplierName ?? "N/A"}";
            }
            catch
            {
                txtFromPurchaseProductID.Text = $"Product ID: {cartItem.ProductId}";
                txtFromPurchaseProductName.Text = $"Product Name: {cartItem.ProductName}";
                txtFromPurchaseProductQuantity.Text = "Quantity Available: N/A";
                txtFromPurchaseProductPrice.Text = $"Price: ₱{cartItem.Price:N2}";
                txtFromPurchaseProductDescription.Text = "Description: N/A";
                txtFromPurchaseProductSupplier.Text = "Supplier: N/A";
            }
        }

        // ── Payment ──────────────────────────────────────────────────────
        private async void btnPayTotalAmount_Click(object sender, EventArgs e)
        {
            if (_lastReceiptData != null)
            { MessageBox.Show("A previous transaction is still active.\n\nPlease click 'Reset Transaction' to clear it before processing a new payment.", "Previous Transaction Pending", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (_cart.Count == 0)
            { MessageBox.Show("Your cart is empty. Please add items before paying.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var inactiveItems = await GetCartItemsWithInactiveSuppliersAsync();
            if (inactiveItems.Count > 0)
            {
                string names = string.Join("\n  • ", inactiveItems.Select(c => c.ProductName));
                MessageBox.Show(
                    "Cannot proceed with checkout.\n\n" +
                    "The following item(s) belong to a deactivated supplier:\n\n" +
                    $"  • {names}\n\n" +
                    "Please remove them from the cart before paying.",
                    "Inactive Supplier — Checkout Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal total = _cart.Sum(c => c.Subtotal);
            decimal vat = total - (total / 1.12m);
            decimal vatableBase = total - vat;

            using var dialog = new PaymentDialog(total);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            var paymentMethod = dialog.SelectedPaymentMethod switch
            {
                "Cash" => PaymentMethod.Cash,
                "GCash" => PaymentMethod.GCash,
                "Maya" => PaymentMethod.Maya,
                "Credit/Debit Card" => PaymentMethod.CreditDebitCard,
                _ => PaymentMethod.Cash
            };

            try
            {
                var saleItems = _cart.Select(c => new SaleItemRequest
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList();

                using var service = new PurchaseService();
                var purchase = await service.ProcessPurchaseAsync(
                    saleItems,
                    paymentMethod,
                    _currentUsername ?? "Staff",
                    _currentUserRole.ToString(),
                    dialog.ReferenceNumber);

                decimal change = dialog.AmountPaid - total;
                txtTotalAmount.Text = $"Total Amount: ₱{total:N2}";
                txtAmountPaid.Text = $"Amount Paid: ₱{dialog.AmountPaid:N2}";
                txtChange.Text = dialog.SelectedPaymentMethod == "Cash"
                    ? $"Change: ₱{change:N2}" : "Change: N/A";

                _lastReceiptData = new ReceiptData
                {
                    PurchaseId = purchase.Id,
                    PurchasedOn = DateTime.Now,
                    CashierName = _currentUsername ?? "Staff",
                    PaymentMethod = paymentMethod,
                    TotalAmount = total,
                    AmountPaid = dialog.AmountPaid,
                    Items = _cart.Select(c => new ReceiptLineItem
                    {
                        ProductName = c.ProductName,
                        Quantity = c.Quantity,
                        UnitPrice = c.Price,
                        Subtotal = c.Subtotal
                    }).ToList()
                };

                btnGenerateReceipt.Enabled = true;
                _cart.Clear();
                await RefreshAllTablesAsync();
                SyncPurchaseInfoLabelsToSelection();

                MessageBox.Show(
                    $"✔  Purchase #{purchase.Id} recorded successfully!\n\n" +
                    $"Vatable Amount : ₱{vatableBase:N2}\n" +
                    $"VAT (12%)      : ₱{vat:N2}\n" +
                    $"Total          : ₱{total:N2}\n" +
                    $"Payment        : {dialog.SelectedPaymentMethod}\n" +
                    $"Amount Paid    : ₱{dialog.AmountPaid:N2}\n" +
                    (dialog.SelectedPaymentMethod == "Cash" ? $"Change         : ₱{change:N2}" : ""),
                    "Payment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Payment failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Receipt ──────────────────────────────────────────────────────
        private void btnGenerateReceipt_Click(object sender, EventArgs e)
        {
            if (_lastReceiptData == null)
            { MessageBox.Show("No completed transaction found. Please complete a purchase first.", "No Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                string path = ReceiptPdfGenerator.Generate(_lastReceiptData);
                if (path == null) return;
                var result = MessageBox.Show($"Receipt saved!\n\n{path}\n\nOpen it now?",
                    "Receipt Generated", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            { MessageBox.Show($"Failed to generate receipt:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ── Reset transaction ────────────────────────────────────────────
        private async void btnResetTransaction_Click(object sender, EventArgs e)
        {
            bool alreadyClear = _cart.Count == 0 &&
                txtAmountPaid.Text is "Amount Paid: ₱0.00" or "Amount Paid:" &&
                txtChange.Text is "Change: ₱0.00" or "Change:";

            if (alreadyClear)
            { MessageBox.Show("Nothing to reset.", "Already Clear", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (MessageBox.Show("Reset the entire transaction? This will clear the cart and all amounts.",
                "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            _cart.Clear();
            _lastReceiptData = null;
            btnGenerateReceipt.Enabled = false;
            txtTotalAmount.Text = "Total Amount: ₱0.00";
            txtAmountPaid.Text = "Amount Paid: ₱0.00";
            txtChange.Text = "Change: ₱0.00";

            ClearPurchaseInfoLabels();
            RefreshCartTables();
            await RefreshStockViewTableAsync();
            txtBoxPurchaseQuantity.Clear();
        }

        // ── Search handlers ──────────────────────────────────────────────
        private async void txtManagePurchaseSearch_TextChanged(object sender, EventArgs e)
        {
            StockViewTable.DataSource = await BuildStockViewAsync(txtManagePurchaseSearch.Text.Trim());
        }

        private async void txtProductListSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtProductListSearch.Text.Trim();
            using var productService = new ProductService();
            var all = (await productService.GetAllProductsAsync())
                .Where(p => !_inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();

            ProductListToStockTable.DataSource = string.IsNullOrEmpty(search) ? all :
                all.Where(p =>
                    p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.SupplierName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Price.ToString().Contains(search))
                .OrderBy(p => p.Name).ToList();
        }

        private async void txtCurrentStockSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtCurrentStockSearch.Text.Trim();
            using var stockService = new StockService();
            var activeProductIds = await GetActiveProductIdsAsync(_inactiveSupplierIds);
            var all = (await stockService.GetAllStockAsync())
                .Where(s => activeProductIds.Contains(s.ProductId))
                .ToList();

            StockTable.DataSource = string.IsNullOrEmpty(search) ? all :
                all.Where(s =>
                    s.ProductName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.ProductId.ToString().Contains(search) ||
                    s.Quantity.ToString().Contains(search))
                .OrderBy(s => s.ProductName).ToList();
        }
        private async void btnUpdateAccount_Click_1(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;

            var selected = _selectedAccount;
            if (selected == null)
            {
                MessageBox.Show("Please select an account to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var service = new AccountService();
                var account = await service.GetAccountByIdAsync(selected.Id);
                if (account == null)
                {
                    MessageBox.Show("Account not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var dialog = new CRUDForms.UpdateAccountDialog(
                account.Id, account.Username, account.Email,
                account.PasswordHash, account.Role, account.IsActive);

                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                await service.UpdateAccountAsync(
                    account.Id,
                    dialog.NewUsername,
                    dialog.NewEmail,
                    dialog.NewPassword,
                    dialog.SelectedRole,
                    dialog.IsActive);

                MessageBox.Show("Account updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RefreshAccountsTableAsync();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Update failed:\n{msg}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void btnDeleteAccount_Click_1(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;

            var selected = _selectedAccount;
            if (selected == null)
            {
                MessageBox.Show("Please select an account to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selected.Username == _currentUsername)
            {
                MessageBox.Show(
                    "You cannot delete your own account from here.\n\nUse the 'Your Account' button instead.",
                    "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var service = new AccountService();

                if (await service.IsLastAdminAsync(selected.Id))
                {
                    MessageBox.Show(
                        "Cannot delete this account.\n\nAt least one Admin account must remain in the system.",
                        "Cannot Delete Last Admin",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var pwdDialog = new CRUDForms.ConfirmPasswordDialog();
                if (pwdDialog.ShowDialog(this) != DialogResult.OK) return;

                var currentAccount = await service.GetAccountByUsernameAsync(_currentUsername);
                if (currentAccount == null ||
                    !VerifyPassword(pwdDialog.EnteredPassword, currentAccount.PasswordHash))
                {
                    MessageBox.Show("Incorrect password. Account deletion cancelled.",
                        "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Delete account '{selected.Username}'?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                await service.DeleteAccountAsync(selected.Id);

                MessageBox.Show("Account deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RefreshAccountsTableAsync();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.InnerException?.Message
                        ?? ex.InnerException?.Message
                        ?? ex.Message;
                MessageBox.Show($"Delete failed:\n\n{msg}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnYourAccount_Click_1(object sender, EventArgs e)
        {
            try
            {
                using var service = new AccountService();
                var account = await service.GetAccountByUsernameAsync(_currentUsername);
                if (account == null)
                {
                    MessageBox.Show("Could not load your account.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var dialog = new CRUDForms.YourAccountDialog(
                    account.Username, account.Email, account.PasswordHash);

                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                if (dialog.WantsToDeleteAccount)
                {
                    if (await service.IsLastAdminAsync(account.Id))
                    {
                        MessageBox.Show(
                            "You are the last Admin. Your account cannot be deleted.\n\n" +
                            "Assign another Admin first before removing this account.",
                            "Cannot Delete Last Admin",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (MessageBox.Show(
                        "Are you sure you want to permanently delete your account?\n\nThis cannot be undone.",
                        "Confirm Account Deletion",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                    await service.DeleteAccountAsync(account.Id);
                    MessageBox.Show("Your account has been deleted. You will now be logged out.",
                        "Account Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    var login = new LoginForm();
                    login.Closed += (s, args) => this.Close();
                    login.Show();
                    return;
                }

                await service.UpdateAccountAsync(
                    account.Id,
                    dialog.NewUsername,
                    dialog.NewEmail,
                    dialog.NewPassword);

                _currentUsername = dialog.NewUsername;
                lblWelcome.Text = $"Welcome, {_currentUsername}!";
                MessageBox.Show("Your account has been updated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.InnerException?.Message
                        ?? ex.InnerException?.Message
                        ?? ex.Message;
                MessageBox.Show($"Operation failed:\n\n{msg}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool VerifyPassword(string plaintext, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(plaintext, hash);
        }


        private async void btnExportSales_Click_1(object sender, EventArgs e)
        {
            try
            {
                using var service = new PurchaseService();
                var all = await service.GetAllSalesAsync();

                // Apply same filter as the table
                var now = DateTime.Now;
                string filter = comboBoxSales.SelectedItem?.ToString() ?? "All Time";

                var data = filter switch
                {
                    "Today" => all.Where(s => s.PurchasedOn.Date == now.Date).ToList(),
                    "This Week" => all.Where(s => s.PurchasedOn >= now.Date.AddDays(-(int)now.DayOfWeek) && s.PurchasedOn <= now).ToList(),
                    "This Month" => all.Where(s => s.PurchasedOn.Month == now.Month && s.PurchasedOn.Year == now.Year).ToList(),
                    "3 Months" => all.Where(s => s.PurchasedOn >= now.AddMonths(-3)).ToList(),
                    "6 Months" => all.Where(s => s.PurchasedOn >= now.AddMonths(-6)).ToList(),
                    "12 Months" => all.Where(s => s.PurchasedOn >= now.AddMonths(-12)).ToList(),
                    _ => all
                };

                if (data.Count == 0)
                {
                    MessageBox.Show("No sales data to export for the selected period.", "Nothing to Export",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var saveDialog = new SaveFileDialog
                {
                    Title = "Export Sales",
                    Filter = "CSV File (*.csv)|*.csv",
                    FileName = $"Sales_{filter.Replace(" ", "")}_{now:yyyyMMdd}"
                };

                if (saveDialog.ShowDialog() != DialogResult.OK) return;

                var sb = new System.Text.StringBuilder();
                sb.AppendLine("Purchase #,Date,Product,Qty,Unit Price,Subtotal,Total,Payment,Reference No.,Cashier,Role");

                foreach (var row in data)
                {
                    string refNo = row.PaymentMethod == "Cash" ? "" : (row.ReferenceNumber ?? "");
                    sb.AppendLine(
                        $"{row.PurchaseId}," +
                        $"{row.PurchasedOn:MM/dd/yyyy hh:mm tt}," +
                        $"\"{row.ProductName}\"," +
                        $"{row.Quantity}," +
                        $"{row.UnitPrice:N2}," +
                        $"{row.Subtotal:N2}," +
                        $"{row.PurchaseTotal:N2}," +
                        $"{row.PaymentMethod}," +
                        $"{refNo}," +
                        $"\"{row.CashierName}\"," +
                        $"{row.CashierRole}");
                }

                await System.IO.File.WriteAllTextAsync(saveDialog.FileName, sb.ToString());

                var open = MessageBox.Show(
                    $"Exported {data.Count} record(s) to:\n\n{saveDialog.FileName}\n\nOpen it now?",
                    "Export Successful", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (open == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(saveDialog.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}