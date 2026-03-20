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

        // ── Stores the last completed transaction for receipt generation ──
        private ReceiptData _lastReceiptData = null;

        public MainInventory()
        {
            InitializeComponent();
            SetupDataGridColumns();
            InitializeDataGrids();

        }

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

        // ── Gray-out rows for products whose supplier is inactive ────────
        // Syncfusion's QueryRowStyleEventArgs does not expose a DataRow property.
        // Instead we resolve the bound object by mapping RowIndex against the
        // grid's current DataSource list (accounting for the header offset).
        private void ApplyInactiveSupplierRowStyle(SfDataGrid grid)
        {
            grid.QueryRowStyle += (sender, e) =>
            {
                if (e.RowType != RowType.DefaultRow) return;

                // Syncfusion data rows start at index 1 (index 0 is the header row),
                // so the zero-based data index is RowIndex - 1.
                int dataIndex = e.RowIndex - 1;
                if (dataIndex < 0) return;

                bool isInactive = false;

                try
                {
                    if (grid.DataSource is System.Collections.IList source && dataIndex < source.Count)
                    {
                        var rowData = source[dataIndex];
                        if (rowData is ProductDTO productDto)
                            isInactive = IsSupplierInactive(productDto.SupplierId);
                        else if (rowData is StockViewDTO stockView)
                            isInactive = IsSupplierInactiveByProductId(stockView.ProductId);
                    }
                }
                catch { /* swallow — never let a style callback crash the grid */ }

                if (isInactive)
                {
                    e.Style.BackColor = Color.FromArgb(210, 210, 210);
                    e.Style.TextColor = Color.FromArgb(140, 140, 140);
                    e.Style.Font.Italic = true;
                }
            };
        }

        // ── Helpers: check supplier active status ────────────────────────
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
                using var productService = new ProductService();
                var product = productService.GetProductById(productId);
                if (product == null) return false;
                return IsSupplierInactive(product.SupplierId);
            }
            catch { return false; }
        }

        private bool HasInactiveSupplierProducts()
        {
            if (_cart.Count == 0) return false;
            foreach (var item in _cart)
            {
                if (IsSupplierInactiveByProductId(item.ProductId))
                    return true;
            }
            return false;
        }

        public MainInventory(string username, UserRole userRole) : this()
        {
            _currentUsername = username;
            _currentUserRole = userRole;
            lblWelcome.Text = $"Welcome, {username}!";
            UpdateUIForRole();
            RefreshAllTables();
        }

        public MainInventory(string username) : this()
        {
            _currentUsername = username;
            _currentUserRole = UserRole.User;
            lblWelcome.Text = $"Welcome, {username}!";
            UpdateUIForRole();
            RefreshAllTables();
        }

        private void RefreshDashboard()
        {
            RefreshMostSoldProduct();
            RefreshTotalProductsCount();
            RefreshMonthlySales();
            RefreshLowStockTable();
            RefreshSalesChart();
        }

        private void RefreshTotalProductsCount()
        {
            using var service = new InvSys.Services.Services.ProductService();
            int count = service.GetAllProducts().Count;
            txtTotalProducts.Text = count.ToString();
        }

        private void RefreshMonthlySales()
        {
            var now = DateTime.Now;
            int thisMonth = now.Month;
            int thisYear = now.Year;

            using var context = new InvSys.Infrastructure.InventoryDbContext();

            decimal monthlyTotal = context.Sales
                .Where(s => s.CreatedDate.Month == thisMonth && s.CreatedDate.Year == thisYear)
                .ToList()
                .Sum(s => s.Subtotal);

            txtMonthlySales.Text = $"₱{monthlyTotal:N2}";
        }

        private void RefreshLowStockTable()
        {
            using var stockService = new InvSys.Services.Services.StockService();
            using var productService = new InvSys.Services.Services.ProductService();
            using var supplierService = new InvSys.Services.Services.SupplierService();

            var allStock = stockService.GetAllStock();
            var products = productService.GetAllProducts();
            var suppliers = supplierService.GetAllSuppliers();

            // Build a set of supplier IDs that are active
            var activeSupplierIds = suppliers
                .Where(s => s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();

            var lowStock = allStock
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
                .Select(x => new
                {
                    x.ProductId,
                    x.ProductName,
                    x.AvailableQty,
                    x.Price,
                    x.SupplierName
                })
                .ToList();

            ProductTableLowStock.DataSource = lowStock;
        }

        private void RefreshSalesChart()
        {
            var now = DateTime.Now;
            int thisMonth = now.Month;
            int thisYear = now.Year;
            int daysInMonth = DateTime.DaysInMonth(thisYear, thisMonth);

            using var context = new InvSys.Infrastructure.InventoryDbContext();

            var salesThisMonth = context.Sales
                .Where(s => s.CreatedDate.Month == thisMonth && s.CreatedDate.Year == thisYear)
                .ToList();

            var soldProductIds = salesThisMonth.Select(s => s.ProductId).Distinct().ToList();
            var products = context.Products.Where(p => soldProductIds.Contains(p.Id)).ToList();

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
                var emptySeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "No Sales",
                    ChartType = SeriesChartType.Line,
                    Color = Color.LightGray
                };
                chartMostSold.Series.Add(emptySeries);
                return;
            }

            var colors = new[]
            {
                Color.FromArgb(49,  52,  113),
                Color.FromArgb(108, 117, 219),
                Color.FromArgb(220, 80,  80),
                Color.FromArgb(80,  180, 120),
                Color.FromArgb(240, 160, 40),
                Color.FromArgb(80,  180, 220),
                Color.FromArgb(180, 80,  180),
                Color.FromArgb(40,  140, 180)
            };

            int colorIndex = 0;
            foreach (var product in products)
            {
                var series = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = product.Name,
                    ChartType = SeriesChartType.Line,
                    Color = colors[colorIndex % colors.Length],
                    BorderWidth = 2,
                    IsVisibleInLegend = true,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6
                };

                for (int day = 1; day <= daysInMonth; day++)
                {
                    int qtySold = salesThisMonth
                        .Where(s => s.ProductId == product.Id && s.CreatedDate.Day == day)
                        .Sum(s => s.Quantity);
                    series.Points.AddXY(day, qtySold);
                }

                chartMostSold.Series.Add(series);
                colorIndex++;
            }

            chartMostSold.Legends[0].BackColor = Color.White;
            chartMostSold.Legends[0].Font = new Font("Segoe UI", 8.5f);
            chartMostSold.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
        }

        private void RefreshMostSoldProduct()
        {
            var now = DateTime.Now;
            int thisMonth = now.Month;
            int thisYear = now.Year;

            using var context = new InvSys.Infrastructure.InventoryDbContext();

            var topProduct = context.Sales
                .Where(s => s.CreatedDate.Month == thisMonth && s.CreatedDate.Year == thisYear)
                .ToList()
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(s => s.Quantity) })
                .OrderByDescending(x => x.TotalSold)
                .FirstOrDefault();

            if (topProduct == null)
            {
                txtNameMostSoldProduct.Text = "No sales yet";
                txtMostSoldDescription.Text = $"0 sold as of {now:MMMM yyyy}";
                return;
            }

            var product = context.Products.FirstOrDefault(p => p.Id == topProduct.ProductId);
            txtNameMostSoldProduct.Text = product?.Name ?? "Unknown";
            txtMostSoldDescription.Text = $"{topProduct.TotalSold} sold as of {now:MMMM yyyy}";
        }

        // ── Column setup ─────────────────────────────────────────────────
        private void SetupDataGridColumns()
        {
            SupplierTable.Columns.Clear();
            SupplierTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "Id", HeaderText = "ID" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "Name", HeaderText = "Supplier Name" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "Email", HeaderText = "Email" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "Location", HeaderText = "Location" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "ContactNo", HeaderText = "Contact" });
            SupplierTable.Columns.Add(new GridCheckBoxColumn { MappingName = "IsActive", HeaderText = "Active" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "CreatedDate", HeaderText = "Added On", Format = "MM/dd/yyyy hh:mm tt" });

            ProductTable.Columns.Clear();
            ProductTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "Id", HeaderText = "ID" });
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "Name", HeaderText = "Product Name" });
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" });
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "Description", HeaderText = "Description" });
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "SupplierName", HeaderText = "Supplier" });

            ProductListToStockTable.Columns.Clear();
            ProductListToStockTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "Name", HeaderText = "Product Name" });
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" });
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "Description", HeaderText = "Description" });
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "SupplierName", HeaderText = "Supplier" });

            StockTable.Columns.Clear();
            StockTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            // Id and ProductId columns intentionally omitted
            StockTable.Columns.Add(new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" });
            StockTable.Columns.Add(new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty Restocked" });
            StockTable.Columns.Add(new GridTextColumn { MappingName = "CreatedDate", HeaderText = "Date Added", Format = "MM/dd/yyyy hh:mm tt" });

            SalesTable.Columns.Clear();
            SalesTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            // SaleId (Product #) intentionally omitted
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "PurchaseId", HeaderText = "Purchase #" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "PurchasedOn", HeaderText = "Date", Format = "MM/dd/yyyy hh:mm tt" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "ProductName", HeaderText = "Product" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "UnitPrice", HeaderText = "Unit Price", Format = "C2" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "Subtotal", HeaderText = "Subtotal", Format = "C2" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "PurchaseTotal", HeaderText = "Total", Format = "C2" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "PaymentMethod", HeaderText = "Payment" });

            StockViewTable.Columns.Clear();
            StockViewTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            // ProductId intentionally omitted
            StockViewTable.Columns.Add(new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" });
            StockViewTable.Columns.Add(new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" });
            StockViewTable.Columns.Add(new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty Available" });

            ProductsToPurchaseTable.Columns.Clear();
            ProductsToPurchaseTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            // ProductId intentionally omitted
            ProductsToPurchaseTable.Columns.Add(new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" });
            ProductsToPurchaseTable.Columns.Add(new GridTextColumn { MappingName = "Price", HeaderText = "Unit Price", Format = "C2" });
            ProductsToPurchaseTable.Columns.Add(new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty to Buy" });
            ProductsToPurchaseTable.Columns.Add(new GridTextColumn { MappingName = "Subtotal", HeaderText = "Subtotal", Format = "C2" });

            PurchaseTable.Columns.Clear();
            PurchaseTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            // ProductId intentionally omitted
            PurchaseTable.Columns.Add(new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" });
            PurchaseTable.Columns.Add(new GridTextColumn { MappingName = "Price", HeaderText = "Unit Price", Format = "C2" });
            PurchaseTable.Columns.Add(new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty to Buy" });
            PurchaseTable.Columns.Add(new GridTextColumn { MappingName = "Subtotal", HeaderText = "Subtotal", Format = "C2" });

            ProductTableLowStock.Columns.Clear();
            ProductTableLowStock.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            ProductTableLowStock.Columns.Add(new GridTextColumn { MappingName = "ProductId", HeaderText = "ID" });
            ProductTableLowStock.Columns.Add(new GridTextColumn { MappingName = "ProductName", HeaderText = "Product" });
            ProductTableLowStock.Columns.Add(new GridTextColumn { MappingName = "AvailableQty", HeaderText = "Stock Left" });
            ProductTableLowStock.Columns.Add(new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" });
            ProductTableLowStock.Columns.Add(new GridTextColumn { MappingName = "SupplierName", HeaderText = "Supplier" });
        }

        private void InitializeDataGrids()
        {
            foreach (var grid in new[] { ProductListToStockTable, StockTable, SalesTable })
                grid.SelectionMode = GridSelectionMode.Single;

            SupplierTable.SelectionMode = GridSelectionMode.Extended;
            ProductTable.SelectionMode = GridSelectionMode.Extended;

            foreach (var grid in new[] { SupplierTable, ProductTable, ProductListToStockTable, StockTable, SalesTable, StockViewTable, ProductsToPurchaseTable, PurchaseTable })
            {
                grid.AutoGenerateColumns = false;
                grid.AllowEditing = false;
                grid.AllowGrouping = false;
                grid.AllowFiltering = true;
                grid.AllowSorting = true;
            }

            SupplierTable.CellDoubleClick += SupplierTable_CellDoubleClick;
            ProductTable.CellDoubleClick += ProductTable_CellDoubleClick;

            // ── Wire up ProductsToPurchaseTable row click → info labels ──
            ProductsToPurchaseTable.SelectionChanged += ProductsToPurchaseTable_SelectionChanged;

            foreach (var grid in new[] { SupplierTable, ProductTable, ProductListToStockTable, StockTable, SalesTable, StockViewTable, ProductsToPurchaseTable, PurchaseTable })
                CustomizeDataGrid(grid);

            // ── Apply inactive-supplier gray-out to relevant grids ───────
            ApplyInactiveSupplierRowStyle(ProductTable);
        }

        // ── Role UI ──────────────────────────────────────────────────────
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

            if (!isAdmin && _currentUsername != null)
                lblWelcome.Text += " (Read-Only Mode)";
        }

        private bool IsAdmin()
        {
            if (_currentUserRole == UserRole.Admin) return true;
            MessageBox.Show("Admin access required.", "Access Denied",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
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
            var suppliers = supplierService.GetAllSuppliers();

            // Build inactive supplier ID set
            var inactiveSupplierIds = suppliers
                .Where(s => !s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();

            // Active-supplier products go to stock table; inactive ones still show
            // in ProductTable (grayed out) so admins can see them, but are excluded
            // from stock/purchase views.
            ProductTable.DataSource = products;

            // Only active-supplier products shown in the stock add panel
            var activeProducts = products
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();
            ProductListToStockTable.DataSource = activeProducts;
        }

        public void RefreshSalesTable()
        {
            using var service = new PurchaseService();
            SalesTable.DataSource = service.GetAllSales();
        }

        // ── Navigation ───────────────────────────────────────────────────
        private void HighlightActiveButton(Button active)
        {
            Color off = Color.FromArgb(49, 52, 113);
            Color on = Color.FromArgb(108, 117, 219);
            foreach (var btn in new[] { btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase })
                btn.BackColor = off;
            active.BackColor = on;
        }

        private void HighlightActiveButtonInPurchases(Button active)
        {
            Color off = Color.FromArgb(49, 52, 113);
            Color on = Color.FromArgb(108, 117, 219);
            foreach (var btn in new[] { btnManagePurchase, btnPurchaseCheckout })
                btn.BackColor = off;
            active.BackColor = on;
        }

        private void btnDashboard_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 0; HighlightActiveButton((Button)sender); }
        private void btnStock_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 1; HighlightActiveButton((Button)sender); }
        private void btnSupplier_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 2; HighlightActiveButton((Button)sender); }
        private void btnProducts_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 3; HighlightActiveButton((Button)sender); }
        private void btnPurchase_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 4; HighlightActiveButton((Button)sender); }
        private void btnSales_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 5; HighlightActiveButton((Button)sender); }

        private void btnManagePurchase_Click(object sender, EventArgs e) { PurchaseControl.SelectedIndex = 0; HighlightActiveButtonInPurchases((Button)sender); }
        private void btnPurchaseCheckout_Click(object sender, EventArgs e) { PurchaseControl.SelectedIndex = 1; HighlightActiveButtonInPurchases((Button)sender); }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            PanelControl.SelectedIndex = 6;
            HighlightActiveButton((Button)sender);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.Closed += (s, args) => this.Close();
            loginForm.Show();
        }

        // ── Supplier CRUD ────────────────────────────────────────────────
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;
            var form = new AddSupplier(this);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
                RefreshSupplierTable();
        }

        private void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            if (!IsAdmin()) return;

            if (SupplierTable.SelectedItem is not SupplierDTO supplierDto)
            {
                MessageBox.Show("Please select a supplier to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var form = new UpdateSupplier(this);
            form.LoadSelectedSupplier(supplierDto);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
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
                foreach (var supplier in selected)
                    service.DeleteSupplier(supplier.Id);

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

            if (e.DataRow.RowType == RowType.DefaultRow && e.DataRow.RowData is SupplierDTO supplierDto)
            {
                // ── Guard: block deactivation while affected products are in the cart ──
                if (!supplierDto.IsActive == false) // only check when about to deactivate (IsActive is still true here)
                {
                    // We check after the dialog; see UpdateSupplier_OnSave guard below.
                }

                var form = new UpdateSupplier(this);
                form.LoadSelectedSupplier(supplierDto);

                // Attach a guard that fires when the user tries to save the supplier as inactive
                form.FormClosing += (fs, fe) =>
                {
                    if (form.DialogResult != DialogResult.OK) return;
                    if (form.IsMarkingInactive && CartContainsProductsFromSupplier(supplierDto.Id))
                    {
                        MessageBox.Show(
                            "Cannot deactivate this supplier because one or more of their products " +
                            "are currently in the purchase cart.\n\n" +
                            "Please reset or complete the current transaction first, then try again.",
                            "Cannot Deactivate Supplier",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        fe.Cancel = true;
                    }
                };

                form.ShowDialog();

                if (form.DialogResult == DialogResult.OK)
                {
                    RefreshAllTables();
                }
            }
        }

        // ── Check if cart contains products belonging to a supplier ──────
        private bool CartContainsProductsFromSupplier(int supplierId)
        {
            if (_cart.Count == 0) return false;
            try
            {
                using var productService = new ProductService();
                var supplierProducts = productService.GetAllProducts()
                    .Where(p => p.SupplierId == supplierId)
                    .Select(p => p.Id)
                    .ToHashSet();

                return _cart.Any(c => supplierProducts.Contains(c.ProductId));
            }
            catch { return false; }
        }

        private void txtBoxSupplierSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtBoxSupplierSearch.Text.Trim();
            using var service = new SupplierService();
            var all = service.GetAllSuppliers();
            var filtered = string.IsNullOrEmpty(search) ? all : all
                .Where(s => s.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            s.Email.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            s.Location.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            s.ContactNo.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(s => s.Name).ToList();
            SupplierTable.DataSource = filtered;
        }

        // ── Product CRUD ─────────────────────────────────────────────────
        private void AddProductPerform()
        {
            if (!IsAdmin()) return;
            var form = new AddProduct(this);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                RefreshProductTable();
                RefreshDashboard();
            }
        }

        private void UpdateProductPerform()
        {
            if (!IsAdmin()) return;

            if (ProductTable.SelectedItem == null)
            {
                MessageBox.Show("Please select a product to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var form = new UpdateProduct(this);
            form.LoadSelectedProduct((ProductDTO)ProductTable.SelectedItem);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                RefreshProductTable();
                RefreshDashboard();
            }
        }

        private void DeleteProductPerform()
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
                foreach (var product in selected)
                    service.DeleteProduct(product.Id);

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
            if (e.DataRow.RowType == RowType.DefaultRow && e.DataRow.RowData != null)
            {
                var form = new UpdateProduct(this);
                form.LoadSelectedProduct((ProductDTO)ProductTable.SelectedItem);
                form.ShowDialog();
                if (form.DialogResult == DialogResult.OK)
                {
                    RefreshProductTable();
                    RefreshDashboard();
                }
            }
        }

        private void txtBoxProductSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtBoxProductSearch.Text.Trim();
            using var service = new ProductService();
            var all = service.GetAllProducts();
            var filtered = string.IsNullOrEmpty(search) ? all : all
                .Where(p => p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            p.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            p.SupplierName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            p.Price.ToString().IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(p => p.Name).ToList();
            ProductTable.DataSource = filtered;
        }

        private void btnAddProduct_Click_1(object sender, EventArgs e) => AddProductPerform();
        private void btnUpdateProduct_Click_1(object sender, EventArgs e) => UpdateProductPerform();
        private void btnDeleteProduct_Click_1(object sender, EventArgs e) => DeleteProductPerform();

        private void panel26_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel35_Paint(object sender, PaintEventArgs e) { }
        private void label19_Click(object sender, EventArgs e) { }

        private void ProductListToStockTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListToStockTable.SelectedItem is ProductDTO product)
            {
                txtSelectedProductID.Text = $"ID: {product.Id}";
                txtSelectedProductName.Text = $"Name: {product.Name}";
                txtSelectedProductPrice.Text = $"Price: {product.Price:C2}";
                txtSelectedProductDescription.Text = $"Description: {product.Description}";
                txtSelectedProductSupplier.Text = $"Supplier: {product.SupplierName}";
            }
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
                MessageBox.Show("Quantity must be a whole number (e.g. 10). No decimals or letters allowed.", "Invalid Input",
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

            if (!TryParseQuantity(txtBoxQuantityAdd.Text, out int quantity))
                return;

            try
            {
                using var service = new StockService();
                service.Restock(product.Id, quantity);
                MessageBox.Show($"Added {quantity} unit(s) to '{product.Name}' successfully.", "Success",
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

            if (!TryParseQuantity(txtBoxQuantityAdd.Text, out int quantity))
                return;

            try
            {
                using var service = new StockService();
                service.UpdateStock(stock.Id, quantity);
                MessageBox.Show("Stock updated successfully.", "Success",
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

            if (MessageBox.Show($"Are you sure you want to delete this stock entry?", "Confirm Delete",
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

        public void RefreshStockTable()
        {
            using var stockService = new StockService();
            using var productService = new ProductService();
            using var supplierService = new SupplierService();

            var inactiveSupplierIds = supplierService.GetAllSuppliers()
                .Where(s => !s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();

            var activeProductIds = productService.GetAllProducts()
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .Select(p => p.Id)
                .ToHashSet();

            // Get DB stock (already subtracts saved sales per our StockService fix)
            var allStock = stockService.GetAllStock()
                .Where(s => activeProductIds.Contains(s.ProductId))
                .ToList();

            // Also subtract whatever is currently sitting in the cart (not yet in DB)
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
            using var stockService = new StockService();
            using var productService = new ProductService();
            using var supplierService = new SupplierService();

            var inactiveSupplierIds = supplierService.GetAllSuppliers()
                .Where(s => !s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();

            var products = productService.GetAllProducts()
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId)) // exclude inactive-supplier products
                .ToList();

            var view = products
                .Select(product =>
                {
                    int available = stockService.GetAvailableStock(product.Id);
                    var inCart = _cart.FirstOrDefault(c => c.ProductId == product.Id);
                    int cartQty = inCart?.Quantity ?? 0;
                    int displayed = Math.Max(0, available - cartQty);

                    return new StockViewDTO
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = displayed,
                        Description = product.Description,
                        SupplierName = product.SupplierName
                    };
                })
                .Where(v => v.Quantity > 0 || _cart.Any(c => c.ProductId == v.ProductId))
                .OrderBy(v => v.ProductName)
                .ToList();

            StockViewTable.DataSource = view;
        }

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
                MessageBox.Show("Quantity must be a whole number (e.g. 3). No decimals or letters allowed.", "Invalid Input",
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
            decimal total = _cart.Sum(c => c.Subtotal);
            txtTotalAmount.Text = $"Total Amount: ₱{total:N2}";
        }

        private void btnAddPurchase_Click(object sender, EventArgs e)
        {
            if (StockViewTable.SelectedItem is not StockViewDTO selected)
            {
                MessageBox.Show("Please select a product from the list first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedProductId = selected.ProductId;

            if (!TryParsePurchaseQuantity(txtBoxPurchaseQuantity.Text, selected.Quantity, out int qty))
                return;

            var existing = _cart.FirstOrDefault(c => c.ProductId == selected.ProductId);
            if (existing != null)
            {
                int remainingAvailable = selected.Quantity;
                if (qty > remainingAvailable)
                {
                    MessageBox.Show(
                        $"Cannot add {qty} more. Only {remainingAvailable} unit(s) still available.",
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

            var updatedRow = (StockViewTable.DataSource as List<StockViewDTO>)
                               ?.FirstOrDefault(v => v.ProductId == selectedProductId);
            if (updatedRow != null)
            {
                int rowIndex = (StockViewTable.DataSource as List<StockViewDTO>).IndexOf(updatedRow);
                StockViewTable.SelectedIndex = rowIndex;
            }

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

            int dbAvailable;
            using (var svc = new StockService())
                dbAvailable = svc.GetAvailableStock(cartItem.ProductId);

            int maxAllowed = dbAvailable;

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Enter new quantity for '{cartItem.ProductName}'.\nAvailable stock: {maxAllowed}",
                "Update Purchase Quantity",
                cartItem.Quantity.ToString());

            if (string.IsNullOrWhiteSpace(input))
                return;

            if (!int.TryParse(input.Trim(), out int newQty))
            {
                MessageBox.Show("Quantity must be a whole number (e.g. 3). No decimals or letters allowed.",
                    "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            MessageBox.Show($"'{cartItem.ProductName}' quantity updated to {newQty}.",
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
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

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
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            _cart.Clear();
            txtBoxPurchaseQuantity.Clear();
            ClearPurchaseInfoLabels();
            RefreshCartTables();
            RefreshStockViewTable();
            RefreshStockTable();
            SyncPurchaseInfoLabelsToSelection();
            RefreshTotalAmount();
        }

        private void RefreshCartTables()
        {
            var snapshot = _cart.ToList();
            ProductsToPurchaseTable.DataSource = null;
            ProductsToPurchaseTable.DataSource = snapshot;
            PurchaseTable.DataSource = null;
            PurchaseTable.DataSource = snapshot;
        }

        private void ClearPurchaseInfoLabels()
        {
            txtFromPurchaseProductID.Text = "Product ID:";
            txtFromPurchaseProductName.Text = "Product Name:";
            txtFromPurchaseProductQuantity.Text = "Quantity Available:";
            txtFromPurchaseProductPrice.Text = "Price:";
            txtFromPurchaseProductDescription.Text = "Description:";
            txtFromPurchaseProductSupplier.Text = "Supplier:";
        }

        // ── ProductsToPurchaseTable row click → update info labels ───────
        private void ProductsToPurchaseTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsToPurchaseTable.SelectedItem is not CartItem cartItem)
            {
                ClearPurchaseInfoLabels();
                return;
            }

            // Look up the live stock view entry for this cart item so we show
            // the true remaining available quantity (not the quantity in cart)
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
                // Fallback to cart-only data if service calls fail
                txtFromPurchaseProductID.Text = $"Product ID: {cartItem.ProductId}";
                txtFromPurchaseProductName.Text = $"Product Name: {cartItem.ProductName}";
                txtFromPurchaseProductQuantity.Text = "Quantity Available: N/A";
                txtFromPurchaseProductPrice.Text = $"Price: ₱{cartItem.Price:N2}";
                txtFromPurchaseProductDescription.Text = "Description: N/A";
                txtFromPurchaseProductSupplier.Text = "Supplier: N/A";
            }
        }

        private void StockViewTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StockViewTable.SelectedItem is not StockViewDTO item)
            {
                ClearPurchaseInfoLabels();
                return;
            }
            UpdatePurchaseInfoLabels(item);
        }

        private void SyncPurchaseInfoLabelsToSelection()
        {
            if (StockViewTable.SelectedItem is StockViewDTO item)
                UpdatePurchaseInfoLabels(item);
            else
                ClearPurchaseInfoLabels();
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

        // ── Payment ──────────────────────────────────────────────────────
        private void btnPayTotalAmount_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Your cart is empty. Please add items before paying.",
                    "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ── Guard: block checkout if any cart product now has an inactive supplier ──
            var inactiveItems = GetCartItemsWithInactiveSuppliers();
            if (inactiveItems.Count > 0)
            {
                string itemNames = string.Join("\n  • ", inactiveItems.Select(c => c.ProductName));
                MessageBox.Show(
                    "Cannot proceed with checkout.\n\n" +
                    "The following cart item(s) belong to a supplier that has been deactivated:\n\n" +
                    $"  • {itemNames}\n\n" +
                    "Please remove these items from the cart before paying.",
                    "Inactive Supplier — Checkout Blocked",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalAmount = _cart.Sum(c => c.Subtotal);
            decimal vatAmount = totalAmount - (totalAmount / (1 + 0.12m));
            decimal vatableAmount = totalAmount - vatAmount;

            using var dialog = new PaymentDialog(totalAmount);
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            PaymentMethod paymentMethod = dialog.SelectedPaymentMethod switch
            {
                "Cash" => PaymentMethod.Cash,
                "GCash" => PaymentMethod.GCash,
                "Maya" => PaymentMethod.Maya,
                "Credit/Debit Card" => PaymentMethod.CreditDebitCard,
                _ => PaymentMethod.Cash
            };

            try
            {
                var saleItems = _cart.Select(c => new InvSys.Services.DTOs.SaleItemRequest
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList();

                using var service = new InvSys.Services.Services.PurchaseService();
                var purchase = service.ProcessPurchase(saleItems, paymentMethod);

                decimal change = dialog.AmountPaid - totalAmount;

                txtTotalAmount.Text = $"Total Amount: ₱{totalAmount:N2}";
                txtAmountPaid.Text = $"Amount Paid: ₱{dialog.AmountPaid:N2}";
                txtChange.Text = dialog.SelectedPaymentMethod == "Cash"
                    ? $"Change: ₱{change:N2}"
                    : "Change: N/A";

                // ── Save receipt data immediately after successful payment ──
                _lastReceiptData = new ReceiptData
                {
                    PurchaseId = purchase.Id,
                    PurchasedOn = DateTime.Now,
                    CashierName = _currentUsername ?? "Staff",
                    PaymentMethod = paymentMethod,
                    TotalAmount = totalAmount,
                    AmountPaid = dialog.AmountPaid,
                    Items = _cart.Select(c => new ReceiptLineItem
                    {
                        ProductName = c.ProductName,
                        Quantity = c.Quantity,
                        UnitPrice = c.Price,
                        Subtotal = c.Subtotal
                    }).ToList()
                };

                // Enable the receipt button now that we have a completed transaction
                btnGenerateReceipt.Enabled = true;
                _cart.Clear();

                RefreshSalesTable();
                RefreshStockTable();
                RefreshStockViewTable();
                SyncPurchaseInfoLabelsToSelection();
                RefreshAllTables();

                MessageBox.Show(
                    $"✔  Purchase #{purchase.Id} recorded successfully!\n\n" +
                    $"Vatable Amount : ₱{vatableAmount:N2}\n" +
                    $"VAT (12%)      : ₱{vatAmount:N2}\n" +
                    $"Total          : ₱{totalAmount:N2}\n" +
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

        // ── Returns cart items whose supplier is currently inactive ──────
        private List<CartItem> GetCartItemsWithInactiveSuppliers()
        {
            var result = new List<CartItem>();
            if (_cart.Count == 0) return result;

            try
            {
                using var productService = new ProductService();
                using var supplierService = new SupplierService();

                var inactiveSupplierIds = supplierService.GetAllSuppliers()
                    .Where(s => !s.IsActive)
                    .Select(s => s.Id)
                    .ToHashSet();

                var products = productService.GetAllProducts()
                    .ToDictionary(p => p.Id);

                foreach (var cartItem in _cart)
                {
                    if (products.TryGetValue(cartItem.ProductId, out var product) &&
                        inactiveSupplierIds.Contains(product.SupplierId))
                    {
                        result.Add(cartItem);
                    }
                }
            }
            catch { /* If the check itself fails, allow checkout and let the service layer handle it */ }

            return result;
        }

        // ── Generate Receipt ─────────────────────────────────────────────
        private void btnGenerateReceipt_Click(object sender, EventArgs e)
        {
            if (_lastReceiptData == null)
            {
                MessageBox.Show(
                    "No completed transaction found.\nPlease complete a purchase first.",
                    "No Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string pdfPath = ReceiptPdfGenerator.Generate(_lastReceiptData);

                var result = MessageBox.Show(
                    $"Receipt saved!\n\n{pdfPath}\n\nOpen it now?",
                    "Receipt Generated", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to generate receipt:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Reset Transaction ────────────────────────────────────────────
        private void btnResetTransaction_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0 &&
                txtAmountPaid.Text is "Amount Paid: ₱0.00" or "Amount Paid:" &&
                txtChange.Text is "Change: ₱0.00" or "Change:")
            {
                MessageBox.Show("Nothing to reset.", "Already Clear",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Reset the entire transaction? This will clear the cart and all amounts.",
                "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

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
            var search = txtManagePurchaseSearch.Text.Trim();

            using var stockService = new StockService();
            using var productService = new ProductService();
            using var supplierService = new SupplierService();

            var inactiveSupplierIds = supplierService.GetAllSuppliers()
                .Where(s => !s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();

            // Exclude inactive-supplier products from purchase search
            var products = productService.GetAllProducts()
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();

            var view = products
                .Select(product =>
                {
                    int available = stockService.GetAvailableStock(product.Id);
                    var inCart = _cart.FirstOrDefault(c => c.ProductId == product.Id);
                    int cartQty = inCart?.Quantity ?? 0;

                    return new StockViewDTO
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = Math.Max(0, available - cartQty),
                        Description = product.Description,
                        SupplierName = product.SupplierName
                    };
                })
                .Where(v => v.Quantity > 0 || _cart.Any(c => c.ProductId == v.ProductId))
                .OrderBy(v => v.ProductName)
                .ToList();

            var filtered = string.IsNullOrEmpty(search) ? view : view
                .Where(v =>
                    v.ProductName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.ProductId.ToString().IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.SupplierName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    v.Price.ToString().IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            StockViewTable.DataSource = filtered;
        }

        private void txtProductListSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtProductListSearch.Text.Trim();
            using var productService = new ProductService();
            using var supplierService = new SupplierService();

            var inactiveSupplierIds = supplierService.GetAllSuppliers()
                .Where(s => !s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();

            var all = productService.GetAllProducts()
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .ToList();

            var filtered = string.IsNullOrEmpty(search) ? all : all
                .Where(p =>
                    p.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.SupplierName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.Price.ToString().IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(p => p.Name)
                .ToList();
            ProductListToStockTable.DataSource = filtered;
        }

        private void txtCurrentStockSearch_TextChanged(object sender, EventArgs e)
        {
            var search = txtCurrentStockSearch.Text.Trim();
            using var stockService = new StockService();
            using var productService = new ProductService();
            using var supplierService = new SupplierService();

            var inactiveSupplierIds = supplierService.GetAllSuppliers()
                .Where(s => !s.IsActive)
                .Select(s => s.Id)
                .ToHashSet();

            var activeProductIds = productService.GetAllProducts()
                .Where(p => !inactiveSupplierIds.Contains(p.SupplierId))
                .Select(p => p.Id)
                .ToHashSet();

            var all = stockService.GetAllStock()
                .Where(s => activeProductIds.Contains(s.ProductId))
                .ToList();

            var filtered = string.IsNullOrEmpty(search) ? all : all
                .Where(s =>
                    s.ProductName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.ProductId.ToString().IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    s.Quantity.ToString().IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(s => s.ProductName)
                .ToList();
            StockTable.DataSource = filtered;
        }
    }
}