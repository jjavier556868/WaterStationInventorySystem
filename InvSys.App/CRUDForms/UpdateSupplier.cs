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

        // ── True when the user is saving with IsActive unchecked (was active before) ──
        public bool IsMarkingInactive { get; private set; }

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

            if (service.GetAllSuppliers().Any(s =>
                s.Email == txtBoxEmail.Text.Trim() && s.Id != _selectedSupplier.Id))
            {
                MessageBox.Show("A supplier with this email already exists!", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmail.Focus();
                return;
            }

            // ── Flag whether this save is deactivating a previously active supplier ──
            IsMarkingInactive = _selectedSupplier.IsActive && !chkBoxActive.Checked;

            // Set DialogResult before closing so the FormClosing guard in
            // MainInventory can read IsMarkingInactive and cancel if needed.
            this.DialogResult = DialogResult.OK;

            // Don't call Close() yet — let FormClosing in the parent decide.
            // If the parent doesn't cancel, the form will close naturally.
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Only commit the save if the dialog was accepted and not cancelled by the parent guard.
            if (this.DialogResult != DialogResult.OK || e.Cancel)
                return;

            try
            {
                using var service = new SupplierService();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Update Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}