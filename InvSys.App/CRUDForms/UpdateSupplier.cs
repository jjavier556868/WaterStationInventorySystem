using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;
using InvSys.Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public partial class UpdateSupplier : Form
    {
        private readonly MainInventory _parentForm;
        private SupplierDTO _selectedSupplier;
        private bool _isSaved = false;

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
            if (supplier == null)
            {
                MessageBox.Show("No supplier selected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            _selectedSupplier = supplier;
            txtBoxID.Text = supplier.Id.ToString();
            txtBoxSupplier.Text = supplier.Name ?? "";
            txtBoxEmail.Text = supplier.Email ?? "";
            txtBoxLocation.Text = supplier.Location ?? "";
            txtBoxContact.Text = supplier.ContactNo ?? "";
            chkBoxActive.Checked = supplier.IsActive;
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedSupplier == null)
            {
                MessageBox.Show("No supplier selected!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtBoxSupplier.Text))
            {
                MessageBox.Show("Supplier Name is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxSupplier.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBoxEmail.Text) || !IsValidEmail(txtBoxEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBoxLocation.Text))
            {
                MessageBox.Show("Location is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxLocation.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBoxContact.Text))
            {
                MessageBox.Show("Contact number is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxContact.Focus();
                return;
            }

            try
            {
                // Prevent double-clicks
                btnUpdate.Enabled = false;
                Cursor = Cursors.WaitCursor;

                using var service = new SupplierService();

                // Check for duplicate email (async)
                var existingSuppliers = await service.GetAllSuppliersAsync();
                if (existingSuppliers.Any(s =>
                    s.Email.Equals(txtBoxEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    s.Id != _selectedSupplier.Id))
                {
                    MessageBox.Show("A supplier with this email already exists!", "Duplicate Entry",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBoxEmail.Focus();
                    btnUpdate.Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }

                // Check for duplicate name (async)
                if (existingSuppliers.Any(s =>
                    s.Name.Equals(txtBoxSupplier.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    s.Id != _selectedSupplier.Id))
                {
                    MessageBox.Show("A supplier with this name already exists!", "Duplicate Entry",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBoxSupplier.Focus();
                    btnUpdate.Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }

                // Check if anything actually changed
                if (_selectedSupplier.Name == txtBoxSupplier.Text.Trim() &&
                    _selectedSupplier.Email == txtBoxEmail.Text.Trim() &&
                    _selectedSupplier.Location == txtBoxLocation.Text.Trim() &&
                    _selectedSupplier.ContactNo == txtBoxContact.Text.Trim() &&
                    _selectedSupplier.IsActive == chkBoxActive.Checked)
                {
                    MessageBox.Show("No changes were made.", "No Changes",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnUpdate.Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }

                // ── Flag whether this save is deactivating a previously active supplier ──
                IsMarkingInactive = _selectedSupplier.IsActive && !chkBoxActive.Checked;

                // Perform the update (async)
                await service.UpdateSupplierAsync(
                    _selectedSupplier.Id,
                    txtBoxSupplier.Text.Trim(),
                    txtBoxEmail.Text.Trim(),
                    txtBoxLocation.Text.Trim(),
                    txtBoxContact.Text.Trim(),
                    chkBoxActive.Checked
                );

                _isSaved = true;
                MessageBox.Show("Supplier updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh parent form using correct async method
                if (_parentForm != null)
                {
                    await _parentForm.RefreshSupplierTableAsync();
                    // Also refresh product table since supplier status affects products
                    await _parentForm.RefreshProductTableAsync();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update supplier: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnUpdate.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Prevent accidental close with unsaved changes
            if (this.DialogResult == DialogResult.OK && !_isSaved)
            {
                var result = MessageBox.Show(
                    "Changes haven't been saved. Close anyway?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.No)
                {
                    this.DialogResult = DialogResult.Cancel;
                    e.Cancel = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email.Trim());
                return addr.Address == email.Trim();
            }
            catch
            {
                return false;
            }
        }
    }
}