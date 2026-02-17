using System;
using System.Linq;
using System.Windows.Forms;
using InvSys.Infrastructure;

namespace InvSys.App.CRUDForms
{
    public partial class UpdateProduct : Form
    {
        private readonly MainInventory _parentForm;
        private int _productId;
        private readonly InventoryService _service;

        public UpdateProduct(MainInventory parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _service = new InventoryService();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            var suppliers = _service.GetAllSuppliers();
            comboBoxSupplier.DataSource = suppliers;
            comboBoxSupplier.DisplayMember = "Name";
            comboBoxSupplier.ValueMember = "Id";
        }

        public void LoadSelectedProduct(DataGridViewRow selectedRow)
        {
            if (selectedRow.DataBoundItem != null)
            {
                dynamic product = selectedRow.DataBoundItem;
                _productId = (int)product.Id;

                txtBoxID.Text = product.Id.ToString();
                txtBoxProductName.Text = product.Name ?? "";
                txtBoxPrice.Text = product.Price?.ToString() ?? "0";
                txtBoxQuantity.Text = product.QuantityInStock?.ToString() ?? "0";
                comboBoxSupplier.Text = product.SupplierName ?? "No Supplier";
            }
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

            if (!int.TryParse(txtBoxQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity (0 or more).", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxQuantity.Focus();
                return;
            }

            if (comboBoxSupplier.SelectedValue == null ||
                !int.TryParse(comboBoxSupplier.SelectedValue.ToString(), out int supplierId) ||
                supplierId <= 0)
            {
                MessageBox.Show("Please select a valid supplier from the dropdown.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxSupplier.DroppedDown = true;
                comboBoxSupplier.Focus();
                return;
            }

            try
            {
                _service.UpdateProduct(_productId, txtBoxProductName.Text.Trim(), price, quantity, supplierId);

                MessageBox.Show("Product updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                _parentForm.RefreshProductTable();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
