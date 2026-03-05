using InvSys.App.CRUDForms;
using InvSys.Domain.Models.Enums;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace InvSys.App
{
    public partial class MainInventory : Form
    {
        private string _currentUsername;
        private UserRole _currentUserRole;

        public MainInventory()
        {
            InitializeComponent();
            SetupSfDataGridColumns();
            InitializeSfDataGrids();
            RefreshSupplierTable();
            RefreshProductTable();
        }

        private void SetupSfDataGridColumns()
        {
            SupplierTable.Columns.Clear();
            SupplierTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            ProductTable.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

            SupplierTable.Columns.Add(new GridTextColumn
            {
                MappingName = "Id",
                HeaderText = "ID"
            });
            SupplierTable.Columns.Add(new GridTextColumn
            {
                MappingName = "Name",
                HeaderText = "Supplier Name"
            });
            SupplierTable.Columns.Add(new GridTextColumn
            {
                MappingName = "Email",
                HeaderText = "Email"
            });
            SupplierTable.Columns.Add(new GridTextColumn
            {
                MappingName = "Location",
                HeaderText = "Location"
            });
            SupplierTable.Columns.Add(new GridTextColumn
            {
                MappingName = "ContactNo",
                HeaderText = "Contact"
            });
            SupplierTable.Columns.Add(new GridCheckBoxColumn
            {
                MappingName = "IsActive",
                HeaderText = "Active"
            });

            ProductTable.Columns.Clear();
            ProductTable.Columns.Add(new GridTextColumn
            {
                MappingName = "Id",
                HeaderText = "ID"
            });
            ProductTable.Columns.Add(new GridTextColumn
            {
                MappingName = "Name",
                HeaderText = "Product Name"
            });
            ProductTable.Columns.Add(new GridTextColumn
            {
                MappingName = "Price",
                HeaderText = "Price",
                Format = "C2"
            });
            ProductTable.Columns.Add(new GridTextColumn
            {
                MappingName = "QuantityInStock",
                HeaderText = "Stock"
            });
            ProductTable.Columns.Add(new GridTextColumn
            {
                MappingName = "SupplierName",
                HeaderText = "Supplier"
            });
        }

        private void InitializeSfDataGrids()
        {
            SupplierTable.SelectionMode = GridSelectionMode.Single;
            SupplierTable.AutoGenerateColumns = false;
            SupplierTable.AllowEditing = false;
            SupplierTable.AllowGrouping = false;
            SupplierTable.AllowFiltering = true;
            SupplierTable.AllowSorting = true;

            ProductTable.SelectionMode = GridSelectionMode.Single;
            ProductTable.AutoGenerateColumns = false;
            ProductTable.AllowEditing = false;
            ProductTable.AllowGrouping = false;
            ProductTable.AllowFiltering = true;
            ProductTable.AllowSorting = true;
        }

        public MainInventory(string username, UserRole userRole) : this()
        {
            _currentUsername = username;
            _currentUserRole = userRole;
            lblWelcome.Text = $"Welcome, {username}!";
            UpdateUIForRole();
        }

        public MainInventory(string username) : this()
        {
            _currentUsername = username;
            _currentUserRole = UserRole.User;
            lblWelcome.Text = $"Welcome, {username}!";
            UpdateUIForRole();
        }

        private void HighlightActiveButton(Button activeButton)
        {
            Color inactiveColor = Color.FromArgb(49, 52, 113);
            Color activeColor = Color.FromArgb(108, 117, 219);
            btnDashboard.BackColor = inactiveColor;
            btnStock.BackColor = inactiveColor;
            btnSales.BackColor = inactiveColor;
            btnAccounts.BackColor = inactiveColor;
            btnProducts.BackColor = inactiveColor;
            btnSupplier.BackColor = inactiveColor;

            activeButton.BackColor = activeColor;
        }

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
            {
                lblWelcome.Text += " (Read-Only Mode)";
            }
        }

        public void RefreshSupplierTable()
        {
            using var service = new InventoryService();
            var suppliers = service.GetAllSuppliers().OrderBy(s => s.Id).ToList();
            SupplierTable.DataSource = suppliers;
        }

        public void RefreshProductTable()
        {
            using var service = new InventoryService();
            var products = service.GetAllProducts();
            ProductTable.DataSource = products;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.Closed += (s, args) => this.Close();
            loginForm.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 0;
            HighlightActiveButton((Button)sender);
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 1;
            HighlightActiveButton((Button)sender);
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 3;
            HighlightActiveButton((Button)sender);
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 2;
            HighlightActiveButton((Button)sender);
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PanelControl.SelectedIndex = 5;
            HighlightActiveButton((Button)sender);
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 4;
            HighlightActiveButton((Button)sender);
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var addForm = new AddSupplier(this);
            addForm.ShowDialog();
            if (addForm.DialogResult == DialogResult.OK)
            {
                RefreshSupplierTable();
            }
        }

        private void SupplierTable_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required to edit.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e.DataRow.RowType == RowType.DefaultRow && e.DataRow.RowData is Supplier supplier)
            {
                var updateForm = new UpdateSupplier(this);
                updateForm.LoadSelectedSupplier(supplier);
                updateForm.ShowDialog();

                if (updateForm.DialogResult == DialogResult.OK)
                {
                    RefreshSupplierTable();
                }
            }
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItems = SupplierTable.SelectedItems;
            if (selectedItems == null || selectedItems.Count == 0)
            {
                MessageBox.Show("Please select a supplier to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowCount = selectedItems.Count;
            string message = rowCount == 1
                ? "Are you sure you want to delete the selected supplier?"
                : $"Are you sure you want to delete {rowCount} selected suppliers?";

            var result = MessageBox.Show(message, "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using var service = new InventoryService();
                int deletedCount = 0;
                var suppliersToDelete = selectedItems.Cast<Supplier>().ToList();

                foreach (var supplier in suppliersToDelete)
                {
                    service.DeleteSupplier(supplier.Id);
                    deletedCount++;
                }

                MessageBox.Show($"{deletedCount} supplier(s) deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshSupplierTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SupplierTable.SelectedItem == null)
            {
                MessageBox.Show("Please select a supplier to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (SupplierTable.SelectedItem is Supplier supplier)
            {
                var updateForm = new UpdateSupplier(this);
                updateForm.LoadSelectedSupplier(supplier);
                updateForm.ShowDialog();

                if (updateForm.DialogResult == DialogResult.OK)
                {
                    RefreshSupplierTable();
                }
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var addForm = new AddProduct();
            addForm.ShowDialog();
            if (addForm.DialogResult == DialogResult.OK)
            {
                RefreshProductTable();
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItems = ProductTable.SelectedItems;
            if (selectedItems == null || selectedItems.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowCount = selectedItems.Count;
            string message = rowCount == 1
                ? "Are you sure you want to delete the selected product?"
                : $"Are you sure you want to delete {rowCount} selected products?";

            var result = MessageBox.Show(message, "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using var service = new InventoryService();
                int deletedCount = 0;
                var productsToDelete = selectedItems.Cast<dynamic>().ToList();

                foreach (var product in productsToDelete)
                {
                    int productId = product.Id;
                    service.DeleteProduct(productId);
                    deletedCount++;
                }

                MessageBox.Show($"{deletedCount} product(s) deleted successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshProductTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtBoxSupplierSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtBoxSupplierSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                RefreshSupplierTable();
                return;
            }

            using var service = new InventoryService();
            var allSuppliers = service.GetAllSuppliers();
            var filteredSuppliers = allSuppliers
                .Where(s => s.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           s.Email.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           s.Location.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           s.ContactNo.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(s => s.Name)
                .ToList();

            SupplierTable.DataSource = filteredSuppliers;

            if (SupplierTable.View != null && SupplierTable.View.Records.Count > 0)
            {
                SupplierTable.SelectedIndex = 0;
            }
        }

        private void ProductTable_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required to edit.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e.DataRow.RowType == RowType.DefaultRow && e.DataRow.RowData != null)
            {
                var product = e.DataRow.RowData;
                var updateForm = new UpdateProduct(this);
                updateForm.LoadSelectedProduct(product);
                updateForm.ShowDialog();

                if (updateForm.DialogResult == DialogResult.OK)
                {
                    RefreshProductTable();
                }
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ProductTable.SelectedItem == null)
            {
                MessageBox.Show("Please select a product to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedProduct = ProductTable.SelectedItem;
            var updateForm = new UpdateProduct(this);
            updateForm.LoadSelectedProduct(selectedProduct);
            updateForm.ShowDialog();

            if (updateForm.DialogResult == DialogResult.OK)
            {
                RefreshProductTable();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                RefreshProductTable();
                return;
            }

            using var service = new InventoryService();
            var allProducts = service.GetAllProducts().Cast<dynamic>()
                .Where(p => p.Name.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           p.Price.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           p.SupplierName.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           p.QuantityInStock.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(p => p.Name)
                .ToList<object>();

            ProductTable.DataSource = allProducts;

            if (ProductTable.View != null && ProductTable.View.Records.Count > 0)
            {
                ProductTable.SelectedIndex = 0;
            }
        }
    }
}