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
            chkBoxActive = new CheckBox();
            label6 = new Label();
            btnUpdate = new Button();
            txtBoxContact = new TextBox();
            label4 = new Label();
            txtBoxLocation = new TextBox();
            label3 = new Label();
            txtBoxEmail = new TextBox();
            label2 = new Label();
            txtBoxSupplier = new TextBox();
            label1 = new Label();
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
            btnCancel.Location = new Point(202, 381);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(158, 47);
            btnCancel.TabIndex = 39;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkBoxActive
            // 
            chkBoxActive.AutoSize = true;
            chkBoxActive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkBoxActive.Location = new Point(34, 331);
            chkBoxActive.Name = "chkBoxActive";
            chkBoxActive.Size = new Size(71, 25);
            chkBoxActive.TabIndex = 38;
            chkBoxActive.Text = "Active";
            chkBoxActive.UseVisualStyleBackColor = true;
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
            btnUpdate.Location = new Point(33, 381);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(163, 47);
            btnUpdate.TabIndex = 36;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            // 
            // txtBoxContact
            // 
            txtBoxContact.Location = new Point(33, 293);
            txtBoxContact.Name = "txtBoxContact";
            txtBoxContact.Size = new Size(327, 23);
            txtBoxContact.TabIndex = 35;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(33, 269);
            label4.Name = "label4";
            label4.Size = new Size(91, 21);
            label4.TabIndex = 34;
            label4.Text = "Contact No.";
            // 
            // txtBoxLocation
            // 
            txtBoxLocation.Location = new Point(33, 243);
            txtBoxLocation.Name = "txtBoxLocation";
            txtBoxLocation.Size = new Size(327, 23);
            txtBoxLocation.TabIndex = 33;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(33, 219);
            label3.Name = "label3";
            label3.Size = new Size(72, 21);
            label3.TabIndex = 32;
            label3.Text = "Location:";
            // 
            // txtBoxEmail
            // 
            txtBoxEmail.Location = new Point(33, 193);
            txtBoxEmail.Name = "txtBoxEmail";
            txtBoxEmail.Size = new Size(327, 23);
            txtBoxEmail.TabIndex = 31;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(33, 169);
            label2.Name = "label2";
            label2.Size = new Size(51, 21);
            label2.TabIndex = 30;
            label2.Text = "Email:";
            // 
            // txtBoxSupplier
            // 
            txtBoxSupplier.Location = new Point(33, 143);
            txtBoxSupplier.Name = "txtBoxSupplier";
            txtBoxSupplier.Size = new Size(327, 23);
            txtBoxSupplier.TabIndex = 29;
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
            // UpdateProduct
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(392, 461);
            Controls.Add(txtBoxID);
            Controls.Add(label5);
            Controls.Add(btnCancel);
            Controls.Add(chkBoxActive);
            Controls.Add(label6);
            Controls.Add(btnUpdate);
            Controls.Add(txtBoxContact);
            Controls.Add(label4);
            Controls.Add(txtBoxLocation);
            Controls.Add(label3);
            Controls.Add(txtBoxEmail);
            Controls.Add(label2);
            Controls.Add(txtBoxSupplier);
            Controls.Add(label1);
            Name = "UpdateProduct";
            Text = "UpdateProduct";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtBoxID;
        private Label label5;
        private Button btnCancel;
        private CheckBox chkBoxActive;
        private Label label6;
        private Button btnUpdate;
        private TextBox txtBoxContact;
        private Label label4;
        private TextBox txtBoxLocation;
        private Label label3;
        private TextBox txtBoxEmail;
        private Label label2;
        private TextBox txtBoxSupplier;
        private Label label1;
    }
}