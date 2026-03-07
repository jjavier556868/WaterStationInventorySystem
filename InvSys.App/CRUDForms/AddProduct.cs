using InvSys.Services.Services;
using System;
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
            LoadSuppliers();
            this.AcceptButton = btnAddProduct;
            this.CancelButton = btnCancel;
        }

        private void LoadSuppliers()
        {
            using var service = new SupplierService();
            comboBoxSupplier.DataSource = service.GetAllSuppliers();
            comboBoxSupplier.DisplayMember = "Name";
            comboBoxSupplier.ValueMember = "Id";
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
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
                using var service = new ProductService();
                service.AddProduct(
                    txtBoxProductName.Text.Trim(),
                    txtBoxDescription.Text.Trim(),
                    price,
                    (int)comboBoxSupplier.SelectedValue
                );

                MessageBox.Show("Product added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _parentForm?.RefreshProductTable();

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

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}