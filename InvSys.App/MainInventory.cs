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
            SupplierTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SupplierTable.AutoGenerateColumns = false;
        }

        public void RefreshSupplierTable()
        {
            using var service = new InventoryService();
            var suppliers = service.GetAllSuppliers().OrderBy(s => s.Id).ToList();  // Sort by ID first
            SupplierTable.DataSource = null;
            SupplierTable.DataSource = suppliers;
            SupplierTable.Refresh();
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
            new AddSupplier(this).ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SupplierTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Use the actual clicked ROW, not SelectedRows
                var selectedRow = SupplierTable.Rows[e.RowIndex];
                if (selectedRow.DataBoundItem != null)
                {
                    var updateForm = new UpdateSupplier(this);
                    updateForm.LoadSelectedSupplier(selectedRow);
                    updateForm.ShowDialog();
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
    }
}
