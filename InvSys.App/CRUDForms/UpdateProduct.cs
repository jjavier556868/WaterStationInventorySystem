using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;
using InvSys.Services.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public partial class UpdateProduct : Form
    {
        private readonly MainInventory _parentForm;
        private int _productId;

        public UpdateProduct(MainInventory parentForm = null)
        {
            InitializeComponent();
            _parentForm = parentForm;
            txtBoxID.Enabled = false;
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            using var service = new SupplierService();
            comboBoxSupplier.DataSource = service.GetAllSuppliers();
            comboBoxSupplier.DisplayMember = "Name";
            comboBoxSupplier.ValueMember = "Id";
        }

        public void LoadSelectedProduct(ProductDTO product)
        {
            if (product == null) return;

            _productId = product.Id;

            txtBoxID.Text = product.Id.ToString();
            txtBoxProductName.Text = product.Name ?? "";
            txtBoxDescription.Text = product.Description ?? "";
            txtBoxPrice.Text = product.Price.ToString();

            comboBoxSupplier.SelectedValue = product.SupplierId;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBoxProductName.Text))
            {
                MessageBox.Show("Product name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxProductName.Focus();
                return;
            }

            if (!decimal.TryParse(txtBoxPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price (0 or more).", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxPrice.Focus();
                return;
            }

            if (comboBoxSupplier.SelectedValue == null ||
                !int.TryParse(comboBoxSupplier.SelectedValue.ToString(), out int supplierId) || supplierId <= 0)
            {
                MessageBox.Show("Please select a valid supplier.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxSupplier.Focus();
                return;
            }

            try
            {
                using var service = new ProductService();
                service.UpdateProduct(
                    _productId,
                    txtBoxProductName.Text.Trim(),
                    txtBoxDescription.Text.Trim(),
                    price,
                    supplierId
                );

                MessageBox.Show("Product updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _parentForm?.RefreshProductTable();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}