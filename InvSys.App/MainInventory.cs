using InvSys.App.CRUDForms;
using InvSys.App.Helpers;
using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;
using InvSys.Services.Services;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
            RefreshAllTables();
        }

        public MainInventory(string username) : this(username, UserRole.User) { }

        // ── Shared helpers: supplier / product active-state queries ──────
        private HashSet<int> GetInactiveSupplierIds()
        {
            using var service = new SupplierService();
            return service.GetAllSuppliers()
                .Where(s => !s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();
        }

        private HashSet<int> GetActiveProductIds(HashSet<int> inactiveSupplierIds = null)
        {
            inactiveSupplierIds ??= GetInactiveSupplierIds();
            using var service = new ProductService();
            return service.GetAllProducts()
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .Select(p => p.Id)
                .ToHashSet();
        }

        private bool IsSupplierInactive(int supplierId)
        {
            try
            {
                using var service = new SupplierService();
                var supplier = service.GetSupplierById(supplierId);
                return supplier != null && !supplier.IsActive;
            }
            catch { return false; }
        }

        private bool IsSupplierInactiveByProductId(int productId)
        {
            try
            {
                using var service = new ProductService();
                var product = service.GetProductById(productId);
                return product != null && IsSupplierInactive(product.SupplierId);
            }
            catch { return false; }
        }

        private bool CartContainsProductsFromSupplier(int supplierId)
        {
            if (_cart.Count == 0) return false;
            try
            {
                using var service = new ProductService();
                var supplierProductIds = service.GetAllProducts()
                    .Where(p => p.SupplierId == supplierId)
                    .Select(p => p.Id)
                    .ToHashSet();
                return _cart.Any(c => supplierProductIds.Contains(c.ProductId));
            }
            catch { return false; }
        }

        private List<CartItem> GetCartItemsWithInactiveSuppliers()
        {
            if (_cart.Count == 0) return new List<CartItem>();
            try
            {
                using var productService = new ProductService();
                var inactiveSupplierIds = GetInactiveSupplierIds();
                var products = productService.GetAllProducts().ToDictionary(p => p.Id);

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

        // Gray-out rows for products whose supplier is inactive.
        // Syncfusion's QueryRowStyleEventArgs exposes no DataRow property, so we
        // resolve the bound object by mapping RowIndex against the DataSource list.
        private void ApplyInactiveSupplierRowStyle(SfDataGrid grid)
        {
            grid.QueryRowStyle += (sender, e) =>
            {
                if (e.RowType != RowType.DefaultRow) return;

                int dataIndex = e.RowIndex - 1; // row 0 is the header
                if (dataIndex < 0) return;

                bool isInactive = false;
                try
                {
                    if (grid.DataSource is System.Collections.IList source && dataIndex < source.Count)
                    {
                        var row = source[dataIndex];
                        if (row is ProductDTO dto)
                            isInactive = IsSupplierInactive(dto.SupplierId);
                        else if (row is StockViewDTO sv)
                            isInactive = IsSupplierInactiveByProductId(sv.ProductId);
                    }
                }
                catch { /* never let a style callback crash the grid */ }

                if (isInactive)
                {
                    e.Style.BackColor = Color.FromArgb(210, 210, 210);
                    e.Style.TextColor = Color.FromArgb(140, 140, 140);
                    e.Style.Font.Italic = true;
                }
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

        // Single place to set AutoSizeColumnsMode and add columns to a grid.
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

            // Selection modes
            foreach (var grid in new[] { ProductListToStockTable, StockTable, SalesTable,
                                         accountsListTable, MostSoldProductsTable })
                grid.SelectionMode = GridSelectionMode.Single;

            SupplierTable.SelectionMode = GridSelectionMode.Extended;
            ProductTable.SelectionMode = GridSelectionMode.Extended;

            // Event wiring
            SupplierTable.CellDoubleClick += SupplierTable_CellDoubleClick;
            ProductTable.CellDoubleClick += ProductTable_CellDoubleClick;
            ProductsToPurchaseTable.SelectionChanged += ProductsToPurchaseTable_SelectionChanged;

            // Inactive-supplier row styling
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
            foreach (var btn in group)
                btn.BackColor = off;
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
        private void RefreshAllTables()
        {
            RefreshSupplierTable();
            RefreshProductTable();
            RefreshStockTable();
            RefreshSalesTable();
            RefreshStockViewTable();
            RefreshDashboard();
            RefreshAccountsTable();
        }

        private void RefreshDashboard()
        {
            RefreshTotalProductsCount();
            RefreshMonthlySales();
            RefreshLowStockTable();
            RefreshSalesChart();
            RefreshMostSoldProductsTable();
        }

        public void RefreshAccountsTable()
        {
            using var service = new AccountService();
            accountsListTable.DataSource = service.GetAllAccounts();
        }

        public void RefreshSupplierTable()
        {
            using var service = new SupplierService();
            SupplierTable.DataSource = service.GetAllSuppliers().OrderBy(s => s.Id).ToList();
        }

        public void RefreshProductTable()
        {
            using var productService = new ProductService();
            using var supplierService = new SupplierService();

            var products = productService.GetAllProducts();
            var inactiveSupplierIds = GetInactiveSupplierIds();

            ProductTable.DataSource = products;

            // Only active-supplier products appear in the stock-add panel
            ProductListToStockTable.DataSource = products
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();
        }

        public void RefreshSalesTable()
        {
            using var service = new PurchaseService();
            SalesTable.DataSource = service.GetAllSales();
        }

        public void RefreshStockTable()
        {
            using var stockService = new StockService();

            var activeProductIds = GetActiveProductIds();
            var allStock = stockService.GetAllStock()
                .Where(s => activeProductIds.Contains(s.ProductId))
                .ToList();

            // Subtract whatever is sitting in the cart (not yet persisted)
            foreach (var entry in allStock)
            {
                var inCart = _cart.FirstOrDefault(c => c.ProductId == entry.ProductId);
                if (inCart != null)
                    entry.Quantity = Math.Max(0, entry.Quantity - inCart.Quantity);
            }

            StockTable.DataSource = allStock;
        }

        public void RefreshStockViewTable()
        {
            StockViewTable.DataSource = BuildStockView(filterText: null);
        }

        // Builds the purchase-panel stock view, optionally filtered by a search string.
        private List<StockViewDTO> BuildStockView(string filterText)
        {
            using var stockService = new StockService();
            using var productService = new ProductService();

            var inactiveSupplierIds = GetInactiveSupplierIds();
            var products = productService.GetAllProducts()
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();

            var view = products
                .Select(p =>
                {
                    int available = stockService.GetAvailableStock(p.Id);
                    int cartQty = _cart.FirstOrDefault(c => c.ProductId == p.Id)?.Quantity ?? 0;
                    return new StockViewDTO
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Price = p.Price,
                        Quantity = Math.Max(0, available - cartQty),
                        Description = p.Description,
                        SupplierName = p.SupplierName
                    };
                })
                .Where(v => v.Quantity > 0 || _cart.Any(c => c.ProductId == v.ProductId))
                .OrderBy(v => v.ProductName)
                .ToList();

            if (string.IsNullOrWhiteSpace(filterText))
                return view;

            return view
                .Where(v =>
                    v.ProductName.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.ProductId.ToString().Contains(filterText) ||
                    v.SupplierName.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.Price.ToString().Contains(filterText))
                .ToList();
        }

        // ── Dashboard widgets ────────────────────────────────────────────
        private void RefreshTotalProductsCount()
        {
            using var service = new ProductService();
            txtTotalProducts.Text = service.GetAllProducts().Count.ToString();
        }

        private void RefreshMonthlySales()
        {
            var now = DateTime.Now;
            using var context = new InvSys.Infrastructure.InventoryDbContext();
            decimal total = context.Sales
                .Where(s => s.CreatedDate.Month == now.Month && s.CreatedDate.Year == now.Year)
                .ToList()
                .Sum(s => s.Subtotal);
            txtMonthlySales.Text = $"₱{total:N2}";
        }

        private void RefreshMostSoldProductsTable()
        {
            var now = DateTime.Now;
            using var context = new InvSys.Infrastructure.InventoryDbContext();

            var top10 = context.Sales
                .Where(s => s.CreatedDate.Month == now.Month && s.CreatedDate.Year == now.Year)
                .ToList()
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(s => s.Quantity), Revenue = g.Sum(s => s.Subtotal) })
                .OrderByDescending(x => x.TotalSold)
                .Take(10)
                .ToList();

            var productNames = context.Products
                .Where(p => top10.Select(t => t.ProductId).Contains(p.Id))
                .ToDictionary(p => p.Id, p => p.Name);

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

        private void RefreshLowStockTable()
        {
            using var stockService = new StockService();
            using var productService = new ProductService();
            using var supplierService = new SupplierService();

            var activeSupplierIds = supplierService.GetAllSuppliers()
                .Where(s => s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();

            var products = productService.GetAllProducts();

            var lowStock = stockService.GetAllStock()
                .Select(s =>
                {
                    int available = stockService.GetAvailableStock(s.ProductId);
                    var product = products.FirstOrDefault(p => p.Id == s.ProductId);
                    return new
                    {
                        s.ProductId,
                        s.ProductName,
                        AvailableQty = available,
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

        private void RefreshSalesChart()
        {
            var now = DateTime.Now;
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

            using var context = new InvSys.Infrastructure.InventoryDbContext();

            var salesThisMonth = context.Sales
                .Where(s => s.CreatedDate.Month == now.Month && s.CreatedDate.Year == now.Year)
                .ToList();

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

            var soldProductIds = salesThisMonth.Select(s => s.ProductId).Distinct().ToList();
            var products = context.Products.Where(p => soldProductIds.Contains(p.Id)).ToList();

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
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            var form = new AddSupplier(this);
            if (form.ShowDialog() == DialogResult.OK)
                RefreshSupplierTable();
        }

        private void btnUpdateSupplier_Click(object sender, EventArgs e)
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
                RefreshSupplierTable();
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
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

            try
            {
                using var service = new SupplierService();
                foreach (var s in selected)
                    service.DeleteSupplier(s.Id);

                MessageBox.Show($"{selected.Count} supplier(s) deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshSupplierTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SupplierTable_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (!IsAdmin()) return;
            if (e.DataRow.RowType != RowType.DefaultRow || e.DataRow.RowData is not SupplierDTO dto) return;

            var form = new UpdateSupplier(this);
            form.LoadSelectedSupplier(dto);

            form.FormClosing += (fs, fe) =>
            {
                if (form.DialogResult != DialogResult.OK) return;
                if (form.IsMarkingInactive && CartContainsProductsFromSupplier(dto.Id))
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
                RefreshAllTables();
        }

        private void txtBoxSupplierSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtBoxSupplierSearch.Text.Trim();
            using var service = new SupplierService();
            var all = service.GetAllSuppliers();

            SupplierTable.DataSource = string.IsNullOrEmpty(search) ? all :
                all.Where(s =>
                    s.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Email.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Location.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.ContactNo.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(s => s.Name)
                .ToList();
        }

        // ── Product CRUD ─────────────────────────────────────────────────
        private void btnAddProduct_Click_1(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            var form = new AddProduct(this);
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshProductTable();
                RefreshDashboard();
            }
        }

        private void btnUpdateProduct_Click_1(object sender, EventArgs e)
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
                RefreshProductTable();
                RefreshDashboard();
            }
        }

        private void btnDeleteProduct_Click_1(object sender, EventArgs e)
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
                    service.DeleteProduct(p.Id);

                MessageBox.Show($"{selected.Count} product(s) deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshProductTable();
                RefreshDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProductTable_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (!IsAdmin()) return;
            if (e.DataRow.RowType != RowType.DefaultRow || ProductTable.SelectedItem is not ProductDTO dto) return;

            var form = new UpdateProduct(this);
            form.LoadSelectedProduct(dto);
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshProductTable();
                RefreshDashboard();
            }
        }

        private void txtBoxProductSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtBoxProductSearch.Text.Trim();
            using var service = new ProductService();
            var all = service.GetAllProducts();

            ProductTable.DataSource = string.IsNullOrEmpty(search) ? all :
                all.Where(p =>
                    p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.SupplierName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Price.ToString().Contains(search))
                .OrderBy(p => p.Name)
                .ToList();
        }

        // ── Stock info panel (left side of stock tab) ────────────────────
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
            {
                MessageBox.Show("Quantity cannot be empty.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(input.Trim(), out quantity))
            {
                MessageBox.Show("Quantity must be a whole number (e.g. 10).", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            if (ProductListToStockTable.SelectedItem is not ProductDTO product)
            {
                MessageBox.Show("Please select a product first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!TryParseQuantity(txtBoxQuantityAdd.Text, out int qty)) return;

            try
            {
                using var service = new StockService();
                service.Restock(product.Id, qty);
                MessageBox.Show($"Added {qty} unit(s) to '{product.Name}'.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxQuantityAdd.Clear();
                RefreshStockTable();
                RefreshStockViewTable();
                RefreshDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add stock: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            if (StockTable.SelectedItem is not StockDTO stock)
            {
                MessageBox.Show("Please select a stock entry to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!TryParseQuantity(txtBoxQuantityAdd.Text, out int qty)) return;

            try
            {
                using var service = new StockService();
                service.UpdateStock(stock.Id, qty);
                MessageBox.Show("Stock updated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxQuantityAdd.Clear();
                RefreshStockTable();
                RefreshStockViewTable();
                RefreshDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update stock: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteStock_Click(object sender, EventArgs e)
        {
            if (StockTable.SelectedItem is not StockDTO stock)
            {
                MessageBox.Show("Please select a stock entry to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Delete this stock entry?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                using var service = new StockService();
                service.DeleteStock(stock.Id);
                MessageBox.Show("Stock entry deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshStockTable();
                RefreshStockViewTable();
                RefreshDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete stock: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Purchase / Cart ──────────────────────────────────────────────
        private bool TryParsePurchaseQuantity(string input, int availableQty, out int quantity)
        {
            quantity = 0;
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter a purchase quantity.", "Input Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(input.Trim(), out quantity))
            {
                MessageBox.Show("Quantity must be a whole number (e.g. 3).", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (quantity > availableQty)
            {
                MessageBox.Show(
                    $"Requested quantity ({quantity}) exceeds available stock ({availableQty}).",
                    "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
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

        private void btnAddPurchase_Click(object sender, EventArgs e)
        {
            if (_lastReceiptData != null)
            {
                MessageBox.Show(
                    "A previous transaction has not been reset yet.\n\n" +
                    "Please click 'Reset Transaction' before starting a new purchase.",
                    "Reset Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (StockViewTable.SelectedItem is not StockViewDTO selected)
            {
                MessageBox.Show("Please select a product from the list first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedProductId = selected.ProductId;
            if (!TryParsePurchaseQuantity(txtBoxPurchaseQuantity.Text, selected.Quantity, out int qty)) return;

            var existing = _cart.FirstOrDefault(c => c.ProductId == selected.ProductId);
            if (existing != null)
            {
                if (qty > selected.Quantity)
                {
                    MessageBox.Show(
                        $"Cannot add {qty} more. Only {selected.Quantity} unit(s) still available.",
                        "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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
            RefreshStockViewTable();
            RefreshStockTable();
            RefreshTotalAmount();

            // Re-select the same row so info labels stay in sync
            var updated = (StockViewTable.DataSource as List<StockViewDTO>)
                ?.FirstOrDefault(v => v.ProductId == selectedProductId);
            if (updated != null)
                StockViewTable.SelectedIndex = (StockViewTable.DataSource as List<StockViewDTO>).IndexOf(updated);

            SyncPurchaseInfoLabelsToSelection();
            MessageBox.Show("Cart item added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdatePurchase_Click(object sender, EventArgs e)
        {
            if (ProductsToPurchaseTable.SelectedItem is not CartItem cartItem)
            {
                MessageBox.Show("Please select an item in the cart to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maxAllowed;
            using (var svc = new StockService())
                maxAllowed = svc.GetAvailableStock(cartItem.ProductId);

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Enter new quantity for '{cartItem.ProductName}'.\nAvailable stock: {maxAllowed}",
                "Update Purchase Quantity",
                cartItem.Quantity.ToString());

            if (string.IsNullOrWhiteSpace(input)) return;

            if (!int.TryParse(input.Trim(), out int newQty))
            {
                MessageBox.Show("Quantity must be a whole number.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (newQty <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (newQty > maxAllowed)
            {
                MessageBox.Show($"Quantity ({newQty}) exceeds available stock ({maxAllowed}).",
                    "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = _cart.FirstOrDefault(c => c.ProductId == cartItem.ProductId);
            if (item == null) return;

            item.Quantity = newQty;
            item.Subtotal = item.Price * newQty;

            RefreshCartTables();
            RefreshStockViewTable();
            RefreshStockTable();
            SyncPurchaseInfoLabelsToSelection();
            RefreshTotalAmount();

            MessageBox.Show($"'{cartItem.ProductName}' updated to {newQty}.",
                "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDeletePurchase_Click(object sender, EventArgs e)
        {
            if (ProductsToPurchaseTable.SelectedItem is not CartItem cartItem)
            {
                MessageBox.Show("Please select an item in the cart to remove.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Remove '{cartItem.ProductName}' from cart?", "Confirm Remove",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            _cart.RemoveAll(c => c.ProductId == cartItem.ProductId);
            txtBoxPurchaseQuantity.Clear();
            RefreshCartTables();
            RefreshStockViewTable();
            RefreshStockTable();
            SyncPurchaseInfoLabelsToSelection();
            RefreshTotalAmount();
        }

        private void btnResetPurchase_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("The cart is already empty.", "Nothing to Reset",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Clear all items from the cart?", "Confirm Reset",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            _cart.Clear();
            txtBoxPurchaseQuantity.Clear();
            ClearPurchaseInfoLabels();
            RefreshCartTables();
            RefreshStockViewTable();
            RefreshStockTable();
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

        private void ProductsToPurchaseTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsToPurchaseTable.SelectedItem is not CartItem cartItem)
            {
                ClearPurchaseInfoLabels();
                return;
            }

            try
            {
                using var stockService = new StockService();
                using var productService = new ProductService();

                int available = stockService.GetAvailableStock(cartItem.ProductId);
                var product = productService.GetProductById(cartItem.ProductId);

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
        private void btnPayTotalAmount_Click(object sender, EventArgs e)
        {
            if (_lastReceiptData != null)
            {
                MessageBox.Show(
                    "A previous transaction is still active.\n\n" +
                    "Please click 'Reset Transaction' to clear it before processing a new payment.",
                    "Previous Transaction Pending",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_cart.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add items before paying.",
                    "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var inactiveItems = GetCartItemsWithInactiveSuppliers();
            if (inactiveItems.Count > 0)
            {
                string names = string.Join("\n  • ", inactiveItems.Select(c => c.ProductName));
                MessageBox.Show(
                    "Cannot proceed with checkout.\n\n" +
                    "The following item(s) belong to a deactivated supplier:\n\n" +
                    $"  • {names}\n\n" +
                    "Please remove them from the cart before paying.",
                    "Inactive Supplier — Checkout Blocked",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                var purchase = service.ProcessPurchase(saleItems, paymentMethod);

                decimal change = dialog.AmountPaid - total;

                txtTotalAmount.Text = $"Total Amount: ₱{total:N2}";
                txtAmountPaid.Text = $"Amount Paid: ₱{dialog.AmountPaid:N2}";
                txtChange.Text = dialog.SelectedPaymentMethod == "Cash"
                    ? $"Change: ₱{change:N2}"
                    : "Change: N/A";

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
                RefreshAllTables();
                SyncPurchaseInfoLabelsToSelection();

                MessageBox.Show(
                    $"✔  Purchase #{purchase.Id} recorded successfully!\n\n" +
                    $"Vatable Amount : ₱{vatableBase:N2}\n" +
                    $"VAT (12%)      : ₱{vat:N2}\n" +
                    $"Total          : ₱{total:N2}\n" +
                    $"Payment        : {dialog.SelectedPaymentMethod}\n" +
                    $"Amount Paid    : ₱{dialog.AmountPaid:N2}\n" +
                    (dialog.SelectedPaymentMethod == "Cash" ? $"Change         : ₱{change:N2}" : ""),
                    "Payment Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Payment failed:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Receipt ──────────────────────────────────────────────────────
        private void btnGenerateReceipt_Click(object sender, EventArgs e)
        {
            if (_lastReceiptData == null)
            {
                MessageBox.Show("No completed transaction found. Please complete a purchase first.",
                    "No Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string path = ReceiptPdfGenerator.Generate(_lastReceiptData);

                if (path == null) return; // user cancelled the save dialog

                var result = MessageBox.Show($"Receipt saved!\n\n{path}\n\nOpen it now?",
                    "Receipt Generated", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate receipt:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Reset transaction ────────────────────────────────────────────
        private void btnResetTransaction_Click(object sender, EventArgs e)
        {
            bool alreadyClear = _cart.Count == 0 &&
                txtAmountPaid.Text is "Amount Paid: ₱0.00" or "Amount Paid:" &&
                txtChange.Text is "Change: ₱0.00" or "Change:";

            if (alreadyClear)
            {
                MessageBox.Show("Nothing to reset.", "Already Clear",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
            RefreshStockViewTable();
            txtBoxPurchaseQuantity.Clear();
        }

        // ── Search handlers ──────────────────────────────────────────────
        private void txtManagePurchaseSearch_TextChanged(object sender, EventArgs e)
        {
            StockViewTable.DataSource = BuildStockView(txtManagePurchaseSearch.Text.Trim());
        }

        private void txtProductListSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtProductListSearch.Text.Trim();
            using var productService = new ProductService();
            var inactiveSupplierIds = GetInactiveSupplierIds();

            var all = productService.GetAllProducts()
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();

            ProductListToStockTable.DataSource = string.IsNullOrEmpty(search) ? all :
                all.Where(p =>
                    p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.SupplierName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Price.ToString().Contains(search))
                .OrderBy(p => p.Name)
                .ToList();
        }

        private void txtCurrentStockSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtCurrentStockSearch.Text.Trim();
            using var stockService = new StockService();
            var activeProductIds = GetActiveProductIds();

            var all = stockService.GetAllStock()
                .Where(s => activeProductIds.Contains(s.ProductId))
                .ToList();

            StockTable.DataSource = string.IsNullOrEmpty(search) ? all :
                all.Where(s =>
                    s.ProductName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.ProductId.ToString().Contains(search) ||
                    s.Quantity.ToString().Contains(search))
                .OrderBy(s => s.ProductName)
                .ToList();
        }
    }
}