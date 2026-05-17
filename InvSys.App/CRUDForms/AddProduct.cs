using InvSys.Services.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public partial class AddProduct : Form
    {
        private readonly MainInventory _parentForm;

        public AddProduct(MainInventory parentForm = null)
        {
            InitializeComponent();
            _parentForm = parentForm;
            this.AcceptButton = btnAddProduct;
            this.CancelButton = btnCancel;
            this.Load += async (s, e) => await LoadSuppliersAsync();
        }

        private async Task LoadSuppliersAsync()
        {
            try
            {
                using var service = new SupplierService();
                var suppliers = await service.GetAllSuppliersAsync();
                comboBoxSupplier.DataSource = suppliers;
                comboBoxSupplier.DisplayMember = "Name";
                comboBoxSupplier.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load suppliers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAddProduct_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(txtBoxProductName.Text))
            {
                MessageBox.Show("Product Name is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxProductName.Focus();
                return;
            }

            if (!decimal.TryParse(txtBoxPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Valid price required (e.g. 29.99)!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxPrice.Focus();
                return;
            }

            if (comboBoxSupplier.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBoxSupplier.Focus();
                return;
            }

            try
            {
                // Disable button to prevent double-clicks
                btnAddProduct.Enabled = false;

                using var service = new ProductService();
                await service.AddProductAsync(
                    txtBoxProductName.Text.Trim(),
                    txtBoxDescription.Text.Trim(),
                    price,
                    (int)comboBoxSupplier.SelectedValue
                );

                MessageBox.Show("Product added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh parent form tables
                if (_parentForm != null)
                {
                    await _parentForm.RefreshProductTableAsync();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Save Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAddProduct.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}