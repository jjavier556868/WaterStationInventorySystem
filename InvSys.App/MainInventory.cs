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
            ConfigureGrid(SupplierTable,
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
                new GridTextColumn { MappingName = "PaymentMethod", HeaderText = "Payment" });

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
                SupplierTable, ProductTable, ProductListToStockTable, StockTable,
                SalesTable, StockViewTable, ProductsToPurchaseTable, PurchaseTable,
                accountsListTable, MostSoldProductsTable
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

            foreach (var grid in new[] { ProductListToStockTable, StockTable, SalesTable,
                                         accountsListTable, MostSoldProductsTable })
                grid.SelectionMode = GridSelectionMode.Single;

            SupplierTable.SelectionMode = GridSelectionMode.Extended;
            ProductTable.SelectionMode = GridSelectionMode.Extended;

            SupplierTable.CellDoubleClick += SupplierTable_CellDoubleClick;
            ProductTable.CellDoubleClick += ProductTable_CellDoubleClick;
            ProductsToPurchaseTable.SelectionChanged += ProductsToPurchaseTable_SelectionChanged;

            ApplyInactiveSupplierRowStyle(ProductTable);
        }

        // ── Role-based UI ────────────────────────────────────────────────
        private void UpdateUIForRole()
        {
            bool isAdmin = _currentUserRole == UserRole.Admin;
            btnAddSupplier.Enabled = isAdmin;
            btnUpdateSupplier.Enabled = isAdmin;
            btnDeleteSupplier.Enabled = isAdmin;
            btnAddProduct.Enabled = isAdmin;
            btnUpdateProduct.Enabled = isAdmin;
            btnDeleteProduct.Enabled = isAdmin;
            btnAccounts.Enabled = isAdmin;
            if (!isAdmin)
                lblWelcome.Text += " (Read-Only Mode)";
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
            SupplierTable.DataSource = all.OrderBy(s => s.Id).ToList();
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
                .ToList();

            foreach (var entry in allStock)
            {
                var inCart = _cart.FirstOrDefault(c => c.ProductId == entry.ProductId);
                if (inCart != null)
                    entry.Quantity = Math.Max(0, entry.Quantity - inCart.Quantity);
            }

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

        private async Task RefreshSalesChartAsync()
        {
            var now = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            using var context = new InvSys.Infrastructure.InventoryDbContext();

            var salesThisMonth = await context.Sales
                .Where(s => s.CreatedDate.Month == now.Month && s.CreatedDate.Year == now.Year)
                .ToListAsync();

            var soldProductIds = salesThisMonth.Select(s => s.ProductId).Distinct().ToList();
            var products = await context.Products
                .Where(p => soldProductIds.Contains(p.Id))
                .ToListAsync();

            chartMostSold.Series.Clear();
            chartMostSold.ChartAreas[0].AxisX.Title = "Day of Month";
            chartMostSold.ChartAreas[0].AxisY.Title = "Qty Sold";
            chartMostSold.ChartAreas[0].AxisX.Minimum = 1;
            chartMostSold.ChartAreas[0].AxisX.Maximum = daysInMonth;
            chartMostSold.ChartAreas[0].AxisX.Interval = 1;
            chartMostSold.ChartAreas[0].BackColor = Color.White;
            chartMostSold.BackColor = Color.White;
            chartMostSold.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);
            chartMostSold.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);

            if (!salesThisMonth.Any())
            {
                chartMostSold.Series.Add(new Series { Name = "No Sales", ChartType = SeriesChartType.Line, Color = Color.LightGray });
                return;
            }

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
                    MarkerSize = 6
                };
                for (int day = 1; day <= daysInMonth; day++)
                {
                    int qty = salesThisMonth
                        .Where(s => s.ProductId == product.Id && s.CreatedDate.Day == day)
                        .Sum(s => s.Quantity);
                    series.Points.AddXY(day, qty);
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
            if (SupplierTable.SelectedItem is not SupplierDTO dto)
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
            var selected = SupplierTable.SelectedItems?.Cast<SupplierDTO>().ToList();
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
            var blockedSuppliers = selected
                .Where(s => allProducts.Any(p => p.SupplierId == s.Id))
                .ToList();

            if (blockedSuppliers.Count > 0)
            {
                string blockedNames = string.Join(", ", blockedSuppliers.Select(s => s.Name));
                MessageBox.Show(
                    $"The following supplier(s) cannot be deleted because they still have associated products:\n\n{blockedNames}\n\nPlease delete or reassign their products first, or consider deactivating the supplier instead.",
                    "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var context = new InvSys.Infrastructure.InventoryDbContext();

                foreach (var s in selected)
                {
                    var productIds = await context.Products
                        .IgnoreQueryFilters()
                        .Where(p => p.SupplierId == s.Id)
                        .Select(p => p.Id)
                        .ToListAsync();

                    if (productIds.Any())
                    {
                        var sales = await context.Sales
                            .IgnoreQueryFilters()
                            .Where(sale => productIds.Contains(sale.ProductId))
                            .ToListAsync();
                        context.Sales.RemoveRange(sales);

                        var stocks = await context.Stocks
                            .IgnoreQueryFilters()
                            .Where(st => productIds.Contains(st.ProductId))
                            .ToListAsync();
                        context.Stocks.RemoveRange(stocks);

                        var products = await context.Products
                            .IgnoreQueryFilters()
                            .Where(p => p.SupplierId == s.Id)
                            .ToListAsync();
                        context.Products.RemoveRange(products);
                    }

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
            SupplierTable.DataSource = string.IsNullOrEmpty(search)
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
            { MessageBox.Show("Please select a stock entry to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!TryParseQuantity(txtBoxQuantityAdd.Text, out int qty)) return;
            try
            {
                using var service = new StockService();
                await service.UpdateStockAsync(stock.Id, qty);
                MessageBox.Show("Stock updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxQuantityAdd.Clear();
                await RefreshStockTableAsync();
                await RefreshStockViewTableAsync();
                await RefreshDashboardAsync();
            }
            catch (Exception ex) { MessageBox.Show($"Failed to update stock: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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

        private async void btnUpdatePurchase_Click(object sender, EventArgs e)
        {
            if (ProductsToPurchaseTable.SelectedItem is not CartItem cartItem)
            { MessageBox.Show("Please select an item in the cart to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            using var svc = new StockService();
            int maxAllowed = await svc.GetAvailableStockAsync(cartItem.ProductId);

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
                var purchase = await service.ProcessPurchaseAsync(saleItems, paymentMethod);

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
    }
}