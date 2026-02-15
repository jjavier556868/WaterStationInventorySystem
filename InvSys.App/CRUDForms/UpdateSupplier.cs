using System;
using System.Linq;
using System.Windows.Forms;
using InvSys.Infrastructure;
using InvSys.Domain.Models.InventoryItems;

namespace InvSys.App.CRUDForms
{
    public partial class UpdateSupplier : Form
    {
        private readonly MainInventory _parentForm;
        private Supplier _selectedSupplier;

        public UpdateSupplier(MainInventory parentForm = null)
        {
            InitializeComponent();
            _parentForm = parentForm;
            txtBoxID.Enabled = false;  // Disable ID textbox
            chkBoxActive.Checked = true;
            this.AcceptButton = btnUpdate;
            this.CancelButton = btnCancel;
        }

        public void LoadSelectedSupplier(DataGridViewRow selectedRow)
        {
            _selectedSupplier = selectedRow.DataBoundItem as Supplier;
            if (_selectedSupplier != null)
            {
                txtBoxID.Text = _selectedSupplier.Id.ToString();
                txtBoxSupplier.Text = _selectedSupplier.Name ?? "";
                txtBoxEmail.Text = _selectedSupplier.Email ?? "";
                txtBoxLocation.Text = _selectedSupplier.Location ?? "";
                txtBoxContact.Text = _selectedSupplier.ContactNo ?? "";  // ✅ ContactNo from model
                chkBoxActive.Checked = _selectedSupplier.isActive;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxSupplier.Text))
            {
                MessageBox.Show("Supplier Name is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxSupplier.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBoxEmail.Text))
            {
                MessageBox.Show("Email is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmail.Focus();
                return;
            }

            if (!txtBoxEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email!", "Invalid Email",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmail.Focus();
                return;
            }

            using var service = new InventoryService();
            if (service.GetAllSuppliers().Any(s => s.Email == txtBoxEmail.Text.Trim() && s.Id != _selectedSupplier.Id))
            {
                MessageBox.Show("Supplier with this email already exists!", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmail.Focus();
                return;
            }

            try
            {
                service.UpdateSupplier(
                    _selectedSupplier.Id,
                    txtBoxSupplier.Text.Trim(),
                    txtBoxEmail.Text.Trim(),
                    txtBoxLocation.Text.Trim(),
                    txtBoxContact.Text.Trim(), 
                    chkBoxActive.Checked
                );

                MessageBox.Show("Supplier updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _parentForm?.RefreshSupplierTable();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Update Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }
    }
}
