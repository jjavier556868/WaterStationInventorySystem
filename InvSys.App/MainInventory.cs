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

namespace InvSys.App
{
    public partial class MainInventory : Form
    {
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



        private string _currentUsername;

        public MainInventory(string username) : this()
        {
            _currentUsername = username;
            lblWelcome.Text = $"Welcome, {username}!";
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
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 1;
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 3;
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 2;
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 5;
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            PanelControl.SelectedIndex = 4;
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

        // Product CRUD Methods (same pattern as Supplier)
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            var addForm = new AddProduct();
            addForm.ShowDialog();
            if (addForm.DialogResult == DialogResult.OK)
            {
                RefreshProductTable();
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            /*
            if (ProductTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = ProductTable.SelectedRows[0];
            if (selectedRow.DataBoundItem is Product product)
            {
                var updateForm = new UpdateProduct(this);
                updateForm.LoadSelectedProduct(selectedRow);
                updateForm.ShowDialog();

                if (updateForm.DialogResult == DialogResult.OK)
                {
                    RefreshProductTable();
                }
            }
            */
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
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
                    // Get ID from the first cell (Id column) - works with anonymous objects
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
            string searchText = txtBoxSupplierSearch.Text.Trim().ToLower();

            foreach (DataGridViewRow row in SupplierTable.Rows)
            {
                bool found = false;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                    {
                        found = true;
                        break;
                    }
                }

                row.Visible = string.IsNullOrEmpty(searchText) || found;
            }
        }
    }
}
