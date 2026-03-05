using System;
using System.Linq;
using System.Windows.Forms;
using InvSys.Infrastructure;
using InvSys.Domain.Models.InventoryItems;

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
            using var service = new InventoryService();
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

            
            if (!int.TryParse(txtBoxQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Valid quantity required (e.g. 50)!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxQuantity.Focus();
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
                using var service = new InventoryService();
                service.AddProduct(
                    txtBoxProductName.Text.Trim(),
                    price,
                    quantity,
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

        private void sfComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
