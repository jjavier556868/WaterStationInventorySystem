namespace InvSys.App.CRUDForms
{
    partial class UpdateSupplier
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
            txtBoxID = new TextBox();
            label5 = new Label();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(203, 375);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(158, 47);
            btnCancel.TabIndex = 25;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkBoxActive
            // 
            chkBoxActive.AutoSize = true;
            chkBoxActive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkBoxActive.Location = new Point(35, 325);
            chkBoxActive.Name = "chkBoxActive";
            chkBoxActive.Size = new Size(71, 25);
            chkBoxActive.TabIndex = 24;
            chkBoxActive.Text = "Active";
            chkBoxActive.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(33, 26);
            label6.Name = "label6";
            label6.Size = new Size(164, 21);
            label6.TabIndex = 23;
            label6.Text = "Update supplier info:";
            // 
            // btnUpdate
            // 
            btnUpdate.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUpdate.Location = new Point(34, 375);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(163, 47);
            btnUpdate.TabIndex = 22;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // txtBoxContact
            // 
            txtBoxContact.Location = new Point(34, 287);
            txtBoxContact.Name = "txtBoxContact";
            txtBoxContact.Size = new Size(327, 23);
            txtBoxContact.TabIndex = 21;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(34, 263);
            label4.Name = "label4";
            label4.Size = new Size(91, 21);
            label4.TabIndex = 20;
            label4.Text = "Contact No.";
            // 
            // txtBoxLocation
            // 
            txtBoxLocation.Location = new Point(34, 237);
            txtBoxLocation.Name = "txtBoxLocation";
            txtBoxLocation.Size = new Size(327, 23);
            txtBoxLocation.TabIndex = 19;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(34, 213);
            label3.Name = "label3";
            label3.Size = new Size(72, 21);
            label3.TabIndex = 18;
            label3.Text = "Location:";
            // 
            // txtBoxEmail
            // 
            txtBoxEmail.Location = new Point(34, 187);
            txtBoxEmail.Name = "txtBoxEmail";
            txtBoxEmail.Size = new Size(327, 23);
            txtBoxEmail.TabIndex = 17;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(34, 163);
            label2.Name = "label2";
            label2.Size = new Size(51, 21);
            label2.TabIndex = 16;
            label2.Text = "Email:";
            // 
            // txtBoxSupplier
            // 
            txtBoxSupplier.Location = new Point(34, 137);
            txtBoxSupplier.Name = "txtBoxSupplier";
            txtBoxSupplier.Size = new Size(327, 23);
            txtBoxSupplier.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(34, 113);
            label1.Name = "label1";
            label1.Size = new Size(117, 21);
            label1.TabIndex = 14;
            label1.Text = "Supplier Name:";
            // 
            // txtBoxID
            // 
            txtBoxID.Location = new Point(34, 91);
            txtBoxID.Name = "txtBoxID";
            txtBoxID.ReadOnly = true;
            txtBoxID.Size = new Size(327, 23);
            txtBoxID.TabIndex = 27;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(34, 67);
            label5.Name = "label5";
            label5.Size = new Size(28, 21);
            label5.TabIndex = 26;
            label5.Text = "ID:";
            // 
            // UpdateSupplier
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(394, 449);
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
            MaximizeBox = false;
            Name = "UpdateSupplier";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UpdateSupplier";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
        private TextBox txtBoxID;
        private Label label5;
    }
}