using InvSys.App.CRUDForms;
using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;
using InvSys.Services.Services;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;



namespace InvSys.App
{
    public partial class MainInventory : Form
    {
        private string _currentUsername;
        private UserRole _currentUserRole;

        public MainInventory()
        {
            InitializeComponent();
            SetupDataGridColumns();
            InitializeDataGrids();
        }

        private void CustomizeDataGrid(SfDataGrid grid)
        {
            // Row height
            grid.RowHeight = 36;
            grid.HeaderRowHeight = 40;

            // Header style
            grid.Style.HeaderStyle.BackColor = Color.FromArgb(49, 52, 113);
            grid.Style.HeaderStyle.TextColor = Color.White;
            grid.Style.HeaderStyle.Font.Bold = true;
            grid.Style.HeaderStyle.Font.Size = 12f;

            // Selected row
            grid.Style.SelectionStyle.BackColor = Color.FromArgb(108, 117, 219);
            grid.Style.SelectionStyle.TextColor = Color.White;

            // Alternating rows via QueryRowStyle event
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

        // ── Column setup ────────────────────────────────────────────────
        private void SetupDataGridColumns()
        {
            // Supplier
            SupplierTable.Columns.Clear();
            SupplierTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "Id", HeaderText = "ID" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "Name", HeaderText = "Supplier Name" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "Email", HeaderText = "Email" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "Location", HeaderText = "Location" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "ContactNo", HeaderText = "Contact" });
            SupplierTable.Columns.Add(new GridCheckBoxColumn { MappingName = "IsActive", HeaderText = "Active" });
            SupplierTable.Columns.Add(new GridTextColumn { MappingName = "CreatedDate", HeaderText = "Added On", Format = "MM/dd/yyyy hh:mm tt" });

            // Product
            ProductTable.Columns.Clear();
            ProductTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "Id", HeaderText = "ID" });
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "Name", HeaderText = "Product Name" });
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" });
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "Description", HeaderText = "Description" });
            ProductTable.Columns.Add(new GridTextColumn { MappingName = "SupplierName", HeaderText = "Supplier" });
            // ── Product Columns for ProductListToStockTable ──────────────────────
            ProductListToStockTable.Columns.Clear();
            ProductListToStockTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "Id", HeaderText = "ID" });
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "Name", HeaderText = "Product Name" });
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "Price", HeaderText = "Price", Format = "C2" });
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "Description", HeaderText = "Description" });
            ProductListToStockTable.Columns.Add(new GridTextColumn { MappingName = "SupplierName", HeaderText = "Supplier" });

            // Stock
            // Stock
            StockTable.Columns.Clear();
            StockTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            StockTable.Columns.Add(new GridTextColumn { MappingName = "Id", HeaderText = "ID" });
            StockTable.Columns.Add(new GridTextColumn { MappingName = "ProductId", HeaderText = "Product ID" });
            StockTable.Columns.Add(new GridTextColumn { MappingName = "ProductName", HeaderText = "Product Name" });
            StockTable.Columns.Add(new GridTextColumn { MappingName = "Quantity", HeaderText = "Quantity" });
            StockTable.Columns.Add(new GridTextColumn { MappingName = "CreatedDate", HeaderText = "Restocked On", Format = "MM/dd/yyyy hh:mm tt" });

            // Sales
            SalesTable.Columns.Clear();
            SalesTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "SaleId", HeaderText = "Sale #" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "PurchaseId", HeaderText = "Purchase #" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "PurchasedOn", HeaderText = "Date", Format = "MM/dd/yyyy hh:mm tt" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "ProductName", HeaderText = "Product" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "Quantity", HeaderText = "Qty" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "UnitPrice", HeaderText = "Unit Price", Format = "C2" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "Subtotal", HeaderText = "Subtotal", Format = "C2" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "PurchaseTotal", HeaderText = "Total", Format = "C2" });
            SalesTable.Columns.Add(new GridTextColumn { MappingName = "PaymentMethod", HeaderText = "Payment" });
        }

        private void InitializeDataGrids()
        {
            // Single selection for grids that don't need multi-delete
            foreach (var grid in new[] { ProductListToStockTable, StockTable, SalesTable })
            {
                grid.SelectionMode = GridSelectionMode.Single;
            }

            // Multi-select for Supplier and Product tables
            SupplierTable.SelectionMode = GridSelectionMode.Extended;
            ProductTable.SelectionMode = GridSelectionMode.Extended;

            foreach (var grid in new[] { SupplierTable, ProductTable, ProductListToStockTable, StockTable, SalesTable })
            {
                grid.AutoGenerateColumns = false;
                grid.AllowEditing = false;
                grid.AllowGrouping = false;
                grid.AllowFiltering = true;
                grid.AllowSorting = true;
            }

            SupplierTable.CellDoubleClick += SupplierTable_CellDoubleClick;
            ProductTable.CellDoubleClick += ProductTable_CellDoubleClick;

            foreach (var grid in new[] { SupplierTable, ProductTable, ProductListToStockTable, StockTable, SalesTable })
            {
                CustomizeDataGrid(grid);
            }
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

        // ── Refresh ───────────────────────────────────────────────────────
        private void RefreshAllTables()
        {
            RefreshSupplierTable();
            RefreshProductTable();
            RefreshStockTable();
            RefreshSalesTable();
        }

        public void RefreshSupplierTable()
        {
            using var service = new SupplierService();
            SupplierTable.DataSource = service.GetAllSuppliers().OrderBy(s => s.Id).ToList();
        }

        public void RefreshProductTable()
        {
            using var service = new ProductService();
            var products = service.GetAllProducts();

            ProductTable.DataSource = products;
            ProductListToStockTable.DataSource = products;
        }
        public void RefreshSalesTable()
        {
            using var service = new PurchaseService();
            SalesTable.DataSource = service.GetAllSales();
        }

        // ── Navigation ────────────────────────────────────────────────────
        private void HighlightActiveButton(Button active)
        {
            Color off = Color.FromArgb(49, 52, 113);
            Color on = Color.FromArgb(108, 117, 219);
            foreach (var btn in new[] { btnDashboard, btnStock, btnSales, btnAccounts, btnProducts, btnSupplier, btnPurchase })
                btn.BackColor = off;
            active.BackColor = on;
        }

        private void btnDashboard_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 0; HighlightActiveButton((Button)sender); }
        private void btnStock_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 1; HighlightActiveButton((Button)sender); }
        private void btnSupplier_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 2; HighlightActiveButton((Button)sender); }
        private void btnProducts_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 3; HighlightActiveButton((Button)sender); }

        private void btnPurchase_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 4; HighlightActiveButton((Button)sender); }
        private void btnSales_Click(object sender, EventArgs e) { PanelControl.SelectedIndex = 5; HighlightActiveButton((Button)sender); }

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

        // ── Supplier CRUD ─────────────────────────────────────────────────
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
                var form = new UpdateSupplier(this);
                form.LoadSelectedSupplier(supplierDto);
                form.ShowDialog();

                if (form.DialogResult == DialogResult.OK)
                    RefreshSupplierTable();
            }
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

        // ── Product CRUD ──────────────────────────────────────────────────
        private void AddProductPerform()
        {
            if (!IsAdmin()) return;
            var form = new AddProduct(this);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
                RefreshProductTable();
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
                RefreshProductTable();
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
                    RefreshProductTable();
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

        private void btnAddProduct_Click_1(object sender, EventArgs e)
        {
            AddProductPerform();
        }

        private void btnUpdateProduct_Click_1(object sender, EventArgs e)
        {
            UpdateProductPerform();
        }

        private void btnDeleteProduct_Click_1(object sender, EventArgs e)
        {
            DeleteProductPerform();
        }

        private void panel26_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel35_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

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

        // ── Stock CRUD ────────────────────────────────────────────────────


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
                service.Restock(product.Id, quantity);  // ← Restock(), not AddStock()
                MessageBox.Show($"Added {quantity} unit(s) to '{product.Name}' successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBoxQuantityAdd.Clear();
                RefreshStockTable();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete stock: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshStockTable()
        {
            using var service = new StockService();
            StockTable.DataSource = service.GetAllStock();
        }

    }
}