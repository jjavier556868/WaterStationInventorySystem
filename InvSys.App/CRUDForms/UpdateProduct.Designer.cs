namespace InvSys.App.CRUDForms
{
    partial class UpdateProduct
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
            txtBoxID = new TextBox();
            label5 = new Label();
            btnCancel = new Button();
            label6 = new Label();
            btnUpdate = new Button();
            txtBoxQuantity = new TextBox();
            label3 = new Label();
            txtBoxPrice = new TextBox();
            label2 = new Label();
            txtBoxProductName = new TextBox();
            label1 = new Label();
            label4 = new Label();
            comboBoxSupplier = new ComboBox();
            SuspendLayout();
            // 
            // txtBoxID
            // 
            txtBoxID.Location = new Point(33, 97);
            txtBoxID.Name = "txtBoxID";
            txtBoxID.ReadOnly = true;
            txtBoxID.Size = new Size(327, 23);
            txtBoxID.TabIndex = 41;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(33, 73);
            label5.Name = "label5";
            label5.Size = new Size(28, 21);
            label5.TabIndex = 40;
            label5.Text = "ID:";
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(202, 344);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(158, 47);
            btnCancel.TabIndex = 39;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(32, 32);
            label6.Name = "label6";
            label6.Size = new Size(164, 21);
            label6.TabIndex = 37;
            label6.Text = "Update product info:";
            // 
            // btnUpdate
            // 
            btnUpdate.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUpdate.Location = new Point(33, 344);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(163, 47);
            btnUpdate.TabIndex = 36;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // txtBoxQuantity
            // 
            txtBoxQuantity.Location = new Point(33, 243);
            txtBoxQuantity.Name = "txtBoxQuantity";
            txtBoxQuantity.Size = new Size(327, 23);
            txtBoxQuantity.TabIndex = 33;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(33, 219);
            label3.Name = "label3";
            label3.Size = new Size(73, 21);
            label3.TabIndex = 32;
            label3.Text = "Quantity:";
            // 
            // txtBoxPrice
            // 
            txtBoxPrice.Location = new Point(33, 193);
            txtBoxPrice.Name = "txtBoxPrice";
            txtBoxPrice.Size = new Size(327, 23);
            txtBoxPrice.TabIndex = 31;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(33, 169);
            label2.Name = "label2";
            label2.Size = new Size(47, 21);
            label2.TabIndex = 30;
            label2.Text = "Price:";
            // 
            // txtBoxProductName
            // 
            txtBoxProductName.Location = new Point(33, 143);
            txtBoxProductName.Name = "txtBoxProductName";
            txtBoxProductName.Size = new Size(327, 23);
            txtBoxProductName.TabIndex = 29;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(33, 119);
            label1.Name = "label1";
            label1.Size = new Size(113, 21);
            label1.TabIndex = 28;
            label1.Text = "Product Name:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(33, 269);
            label4.Name = "label4";
            label4.Size = new Size(71, 21);
            label4.TabIndex = 34;
            label4.Text = "Supplier:";
            // 
            // comboBoxSupplier
            // 
            comboBoxSupplier.FormattingEnabled = true;
            comboBoxSupplier.Location = new Point(33, 293);
            comboBoxSupplier.Name = "comboBoxSupplier";
            comboBoxSupplier.Size = new Size(327, 23);
            comboBoxSupplier.TabIndex = 42;
            // 
            // UpdateProduct
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(392, 426);
            Controls.Add(comboBoxSupplier);
            Controls.Add(txtBoxID);
            Controls.Add(label5);
            Controls.Add(btnCancel);
            Controls.Add(label6);
            Controls.Add(btnUpdate);
            Controls.Add(label4);
            Controls.Add(txtBoxQuantity);
            Controls.Add(label3);
            Controls.Add(txtBoxPrice);
            Controls.Add(label2);
            Controls.Add(txtBoxProductName);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "UpdateProduct";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UpdateProduct";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBoxID;
        private Label label5;
        private Button btnCancel;
        private Label label6;
        private Button btnUpdate;
        private TextBox txtBoxQuantity;
        private Label label3;
        private TextBox txtBoxPrice;
        private Label label2;
        private TextBox txtBoxProductName;
        private Label label1;
        private Label label4;
        private ComboBox comboBoxSupplier;
    }
}