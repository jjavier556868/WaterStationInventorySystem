using System;
using System.Linq;
using System.Windows.Forms;
using InvSys.Infrastructure;        // InventoryService + DbContext
using InvSys.Domain.Models.InventoryItems;  // Supplier model

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

        private void btnAdd_Click(object sender, EventArgs e)
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

            //check duplicate email
            using var service = new InventoryService();
            if (service.GetAllSuppliers().Any(s => s.Email == txtBoxEmail.Text.Trim()))
            {
                MessageBox.Show("Supplier with this email already exists!", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmail.Focus();
                return;
            }

            try
            {
                service.AddSupplier(
                    txtBoxSupplier.Text.Trim(),
                    txtBoxEmail.Text.Trim(),
                    txtBoxLocation.Text.Trim(),
                    txtBoxContact.Text.Trim(),
                    chkBoxActive.Checked
                );

                MessageBox.Show("Supplier added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _parentForm?.RefreshSupplierTable();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Save Failed",
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
