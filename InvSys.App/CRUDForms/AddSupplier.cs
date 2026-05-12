using InvSys.Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public partial class AddSupplier : Form
    {
        private readonly MainInventory _parentForm;

        public AddSupplier(MainInventory parentForm = null)
        {
            InitializeComponent();
            _parentForm = parentForm;
            chkBoxActive.Checked = true;
            this.AcceptButton = btnAdd;
            this.CancelButton = btnCancel;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            // Validation
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
                btnAdd.Enabled = false;
                Cursor = Cursors.WaitCursor;

                using var service = new SupplierService();

                // Check for duplicate email using async method
                var existingSuppliers = await service.GetAllSuppliersAsync();
                if (existingSuppliers.Any(s =>
                    s.Email.Equals(txtBoxEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("A supplier with this email already exists!", "Duplicate Entry",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBoxEmail.Focus();
                    btnAdd.Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }

                // Check for duplicate name
                if (existingSuppliers.Any(s =>
                    s.Name.Equals(txtBoxSupplier.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("A supplier with this name already exists!", "Duplicate Entry",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBoxSupplier.Focus();
                    btnAdd.Enabled = true;
                    Cursor = Cursors.Default;
                    return;
                }

                // Use async method
                await service.AddSupplierAsync(
                    txtBoxSupplier.Text.Trim(),
                    txtBoxEmail.Text.Trim(),
                    txtBoxLocation.Text.Trim(),
                    txtBoxContact.Text.Trim(),
                    chkBoxActive.Checked
                );

                MessageBox.Show("Supplier added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh parent form using correct async method
                if (_parentForm != null)
                {
                    await _parentForm.RefreshSupplierTableAsync();
                    // Also refresh product table since new supplier affects products
                    await _parentForm.RefreshProductTableAsync();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add supplier: {ex.Message}", "Save Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAdd.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Better email validation
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