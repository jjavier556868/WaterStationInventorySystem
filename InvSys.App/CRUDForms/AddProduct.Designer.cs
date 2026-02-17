namespace InvSys.App.CRUDForms
{
    partial class AddProduct
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            btnCancel = new Button();
            label6 = new Label();
            btnAddProduct = new Button();
            txtBoxProductName = new TextBox();
            label1 = new Label();
            txtBoxPrice = new TextBox();
            label2 = new Label();
            txtBoxQuantity = new TextBox();
            label3 = new Label();
            label4 = new Label();
            supplierBindingSource = new BindingSource(components);
            comboBoxSupplier = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)supplierBindingSource).BeginInit();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(201, 304);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(158, 47);
            btnCancel.TabIndex = 25;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(32, 25);
            label6.Name = "label6";
            label6.Size = new Size(236, 21);
            label6.TabIndex = 23;
            label6.Text = "Please enter product info here:";
            // 
            // btnAddProduct
            // 
            btnAddProduct.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddProduct.Location = new Point(32, 304);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(163, 47);
            btnAddProduct.TabIndex = 22;
            btnAddProduct.Text = "Add";
            btnAddProduct.UseVisualStyleBackColor = true;
            btnAddProduct.Click += btnAddProduct_Click;
            // 
            // txtBoxProductName
            // 
            txtBoxProductName.Location = new Point(32, 88);
            txtBoxProductName.Name = "txtBoxProductName";
            txtBoxProductName.Size = new Size(327, 23);
            txtBoxProductName.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(32, 64);
            label1.Name = "label1";
            label1.Size = new Size(113, 21);
            label1.TabIndex = 14;
            label1.Text = "Product Name:";
            // 
            // txtBoxPrice
            // 
            txtBoxPrice.Location = new Point(32, 138);
            txtBoxPrice.Name = "txtBoxPrice";
            txtBoxPrice.Size = new Size(327, 23);
            txtBoxPrice.TabIndex = 27;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(32, 114);
            label2.Name = "label2";
            label2.Size = new Size(47, 21);
            label2.TabIndex = 26;
            label2.Text = "Price:";
            // 
            // txtBoxQuantity
            // 
            txtBoxQuantity.Location = new Point(33, 188);
            txtBoxQuantity.Name = "txtBoxQuantity";
            txtBoxQuantity.Size = new Size(327, 23);
            txtBoxQuantity.TabIndex = 29;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(33, 164);
            label3.Name = "label3";
            label3.Size = new Size(73, 21);
            label3.TabIndex = 28;
            label3.Text = "Quantity:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(33, 214);
            label4.Name = "label4";
            label4.Size = new Size(71, 21);
            label4.TabIndex = 30;
            label4.Text = "Supplier:";
            // 
            // supplierBindingSource
            // 
            supplierBindingSource.DataSource = typeof(Domain.Models.InventoryItems.Supplier);
            // 
            // comboBoxSupplier
            // 
            comboBoxSupplier.FormattingEnabled = true;
            comboBoxSupplier.Location = new Point(32, 238);
            comboBoxSupplier.Name = "comboBoxSupplier";
            comboBoxSupplier.Size = new Size(327, 23);
            comboBoxSupplier.TabIndex = 32;
            // 
            // AddProduct
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(390, 383);
            Controls.Add(comboBoxSupplier);
            Controls.Add(label4);
            Controls.Add(txtBoxQuantity);
            Controls.Add(label3);
            Controls.Add(txtBoxPrice);
            Controls.Add(label2);
            Controls.Add(btnCancel);
            Controls.Add(label6);
            Controls.Add(btnAddProduct);
            Controls.Add(txtBoxProductName);
            Controls.Add(label1);
            Name = "AddProduct";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AddProduct";
            ((System.ComponentModel.ISupportInitialize)supplierBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancel;
        private Label label6;
        private Button btnAddProduct;
        private TextBox txtBoxProductName;
        private Label label1;
        private TextBox txtBoxPrice;
        private Label label2;
        private TextBox txtBoxQuantity;
        private Label label3;
        private Label label4;
        private BindingSource supplierBindingSource;
        private ComboBox comboBoxSupplier;
    }
}