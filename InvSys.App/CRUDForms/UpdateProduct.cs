using InvSys.Domain.Models.InventoryItems;
using InvSys.Services.DTOs;
using InvSys.Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public partial class UpdateProduct : Form
    {
        private readonly MainInventory _parentForm;
        private int _productId;
        private ProductDTO _originalProduct;

        public UpdateProduct(MainInventory parentForm = null)
        {
            InitializeComponent();
            _parentForm = parentForm;
            txtBoxID.Enabled = false;
            this.AcceptButton = btnUpdate;
            this.CancelButton = btnCancel;
            this.Load += async (s, e) => await LoadSuppliersAsync();
        }

        private async Task LoadSuppliersAsync()
        {
            try
            {
                using var service = new SupplierService();
                var suppliers = await service.GetAllSuppliersAsync();

                // Show all suppliers but indicate inactive ones in the UI
                comboBoxSupplier.DataSource = suppliers;
                comboBoxSupplier.DisplayMember = "Name";
                comboBoxSupplier.ValueMember = "Id";

                // Optional: Add visual indicator for inactive suppliers
                // Could use DrawItem event to gray out inactive ones
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load suppliers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task LoadSelectedProductAsync(ProductDTO product)
        {
            if (product == null)
            {
                MessageBox.Show("No product selected.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            _productId = product.Id;
            _originalProduct = product;

            txtBoxID.Text = product.Id.ToString();
            txtBoxProductName.Text = product.Name ?? "";
            txtBoxDescription.Text = product.Description ?? "";
            txtBoxPrice.Text = product.Price.ToString("F2"); // Format consistently

            // Ensure supplier is loaded before setting selected value
            await LoadSuppliersAsync();
            comboBoxSupplier.SelectedValue = product.SupplierId;
        }

        // Keep sync version for backward compatibility with MainInventory
        public void LoadSelectedProduct(ProductDTO product)
        {
            LoadSelectedProductAsync(product).ConfigureAwait(false);
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validate product ID
            if (_productId <= 0)
            {
                MessageBox.Show("Invalid product ID. Please reload the product.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate product name
            if (string.IsNullOrWhiteSpace(txtBoxProductName.Text))
            {
                MessageBox.Show("Product name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxProductName.Focus();
                return;
            }

            // Validate price
            if (!decimal.TryParse(txtBoxPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than zero.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxPrice.Focus();
                return;
            }

            // Validate supplier selection
            if (comboBoxSupplier.SelectedValue == null ||
                !int.TryParse(comboBoxSupplier.SelectedValue.ToString(), out int supplierId) ||
                supplierId <= 0)
            {
                MessageBox.Show("Please select a valid supplier.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxSupplier.Focus();
                return;
            }

            // Check if anything actually changed
            if (_originalProduct != null &&
                _originalProduct.Name == txtBoxProductName.Text.Trim() &&
                _originalProduct.Description == txtBoxDescription.Text.Trim() &&
                _originalProduct.Price == price &&
                _originalProduct.SupplierId == supplierId)
            {
                MessageBox.Show("No changes were made.", "No Changes",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Prevent double-clicks
                btnUpdate.Enabled = false;
                Cursor = Cursors.WaitCursor;

                using var service = new ProductService();

                // Use async method
                await service.UpdateProductAsync(
                    _productId,
                    txtBoxProductName.Text.Trim(),
                    txtBoxDescription.Text.Trim(),
                    price,
                    supplierId
                );

                MessageBox.Show("Product updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh parent form using correct async method
                if (_parentForm != null)
                {
                    await _parentForm.RefreshProductTableAsync();
                    // Also refresh dashboard since product data changed
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update product: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnUpdate.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Remove the duplicate btnCancel_Click_1 method!
    }
}