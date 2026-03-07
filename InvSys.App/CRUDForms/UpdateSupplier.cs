using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;
using InvSys.Services.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public partial class UpdateSupplier : Form
    {
        private readonly MainInventory _parentForm;
        private SupplierDTO _selectedSupplier;

        public UpdateSupplier(MainInventory parentForm = null)
        {
            InitializeComponent();
            _parentForm = parentForm;
            txtBoxID.Enabled = false;
            this.AcceptButton = btnUpdate;
            this.CancelButton = btnCancel;
        }

        public void LoadSelectedSupplier(SupplierDTO supplier)
        {
            _selectedSupplier = supplier;
            if (_selectedSupplier != null)
            {
                txtBoxID.Text = _selectedSupplier.Id.ToString();
                txtBoxSupplier.Text = _selectedSupplier.Name ?? "";
                txtBoxEmail.Text = _selectedSupplier.Email ?? "";
                txtBoxLocation.Text = _selectedSupplier.Location ?? "";
                txtBoxContact.Text = _selectedSupplier.ContactNo ?? "";
                chkBoxActive.Checked = _selectedSupplier.IsActive;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedSupplier == null)
            {
                MessageBox.Show("No supplier selected!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBoxSupplier.Text))
            {
                MessageBox.Show("Supplier Name is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxSupplier.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBoxEmail.Text) || !txtBoxEmail.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmail.Focus();
                return;
            }

            using var service = new SupplierService();

            // Optional: check for duplicate email
            if (service.GetAllSuppliers().Any(s =>
                s.Email == txtBoxEmail.Text.Trim() && s.Id != _selectedSupplier.Id))
            {
                MessageBox.Show("A supplier with this email already exists!", "Duplicate",
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}