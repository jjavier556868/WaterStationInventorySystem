using InvSys.App.CRUDForms;
using InvSys.Domain.Models.InventoryItems;
using InvSys.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InvSys.Domain.Models.Enums;

namespace InvSys.App
{
    public partial class MainInventory : Form
    {
        private string _currentUsername;
        private UserRole _currentUserRole;

        public MainInventory()
        {
            InitializeComponent();
            RefreshSupplierTable();
            RefreshProductTable();

            SupplierTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SupplierTable.AutoGenerateColumns = false;

            ProductTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ProductTable.AutoGenerateColumns = false;
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
            SupplierTable.DataSource = null;
            SupplierTable.DataSource = suppliers;
            SupplierTable.Refresh();
        }

        public void RefreshProductTable()
        {
            using var service = new InventoryService();
            var products = service.GetAllProducts();
            ProductTable.DataSource = null;
            ProductTable.DataSource = products;
            ProductTable.Refresh();
        }

        private void MainInventory_Load(object sender, EventArgs e)
        {
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
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

        private void sfDataGrid2_Click(object sender, EventArgs e)
        {
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }

        private void userNameLabel_Click(object sender, EventArgs e)
        {
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void SupplierTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required to edit.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var selectedRow = SupplierTable.Rows[e.RowIndex];
                if (selectedRow.DataBoundItem != null)
                {
                    var updateForm = new UpdateSupplier(this);
                    updateForm.LoadSelectedSupplier(selectedRow);
                    updateForm.ShowDialog();

                    if (updateForm.DialogResult == DialogResult.OK)
                    {
                        RefreshSupplierTable();
                    }
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
            if (SupplierTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowCount = SupplierTable.SelectedRows.Count;
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

                for (int i = SupplierTable.SelectedRows.Count - 1; i >= 0; i--)
                {
                    var row = SupplierTable.SelectedRows[i];
                    if (row.DataBoundItem is Supplier supplier)
                    {
                        service.DeleteSupplier(supplier.Id);
                        deletedCount++;
                    }
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
            if (SupplierTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = SupplierTable.SelectedRows[0];
            if (selectedRow.DataBoundItem is Supplier supplier)
            {
                var updateForm = new UpdateSupplier(this);
                updateForm.LoadSelectedSupplier(selectedRow);
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
            if (ProductTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rowCount = ProductTable.SelectedRows.Count;
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

                for (int i = ProductTable.SelectedRows.Count - 1; i >= 0; i--)
                {
                    var row = ProductTable.SelectedRows[i];

                    if (row.Cells[0].Value != null)
                    {
                        int productId = Convert.ToInt32(row.Cells[0].Value);
                        service.DeleteProduct(productId);
                        deletedCount++;
                    }
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

            SupplierTable.DataSource = null;
            SupplierTable.DataSource = filteredSuppliers;

            if (SupplierTable.Rows.Count > 0)
            {
                SupplierTable.ClearSelection();
                SupplierTable.Rows[0].Selected = true;
                SupplierTable.CurrentCell = SupplierTable.Rows[0].Cells[0];
                SupplierTable.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        private void ProductTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_currentUserRole != UserRole.Admin)
            {
                MessageBox.Show("Admin access required to edit.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var selectedRow = ProductTable.Rows[e.RowIndex];
                if (selectedRow.DataBoundItem != null)
                {
                    var updateForm = new UpdateProduct(this);
                    updateForm.LoadSelectedProduct(selectedRow);
                    updateForm.ShowDialog();

                    if (updateForm.DialogResult == DialogResult.OK)
                    {
                        RefreshProductTable();
                    }
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
            if (ProductTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selectedRow = ProductTable.SelectedRows[0];
            if (selectedRow.DataBoundItem != null)
            {
                var updateForm = new UpdateProduct(this);
                updateForm.LoadSelectedProduct(selectedRow);
                updateForm.ShowDialog();
                if (updateForm.DialogResult == DialogResult.OK)
                {
                    RefreshProductTable();
                }
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

            ProductTable.DataSource = null;
            ProductTable.DataSource = allProducts;

            if (ProductTable.Rows.Count > 0)
            {
                ProductTable.ClearSelection();
                ProductTable.Rows[0].Selected = true;
                ProductTable.CurrentCell = ProductTable.Rows[0].Cells[0];
                ProductTable.FirstDisplayedScrollingRowIndex = 0;
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
