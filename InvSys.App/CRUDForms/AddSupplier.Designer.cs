namespace InvSys.App.CRUDForms
{
    partial class AddSupplier
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
            label1 = new Label();
            txtBoxSupplier = new TextBox();
            txtBoxEmail = new TextBox();
            label2 = new Label();
            txtBoxLocation = new TextBox();
            label3 = new Label();
            txtBoxContact = new TextBox();
            label4 = new Label();
            btnAdd = new Button();
            label6 = new Label();
            chkBoxActive = new CheckBox();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(32, 65);
            label1.Name = "label1";
            label1.Size = new Size(117, 21);
            label1.TabIndex = 0;
            label1.Text = "Supplier Name:";
            // 
            // txtBoxSupplier
            // 
            txtBoxSupplier.Location = new Point(32, 89);
            txtBoxSupplier.Name = "txtBoxSupplier";
            txtBoxSupplier.Size = new Size(327, 23);
            txtBoxSupplier.TabIndex = 1;
            // 
            // txtBoxEmail
            // 
            txtBoxEmail.Location = new Point(32, 139);
            txtBoxEmail.Name = "txtBoxEmail";
            txtBoxEmail.Size = new Size(327, 23);
            txtBoxEmail.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(32, 115);
            label2.Name = "label2";
            label2.Size = new Size(51, 21);
            label2.TabIndex = 2;
            label2.Text = "Email:";
            // 
            // txtBoxLocation
            // 
            txtBoxLocation.Location = new Point(32, 189);
            txtBoxLocation.Name = "txtBoxLocation";
            txtBoxLocation.Size = new Size(327, 23);
            txtBoxLocation.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(32, 165);
            label3.Name = "label3";
            label3.Size = new Size(72, 21);
            label3.TabIndex = 4;
            label3.Text = "Location:";
            // 
            // txtBoxContact
            // 
            txtBoxContact.Location = new Point(32, 239);
            txtBoxContact.Name = "txtBoxContact";
            txtBoxContact.Size = new Size(327, 23);
            txtBoxContact.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(32, 215);
            label4.Name = "label4";
            label4.Size = new Size(91, 21);
            label4.TabIndex = 6;
            label4.Text = "Contact No.";
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAdd.Location = new Point(32, 327);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(163, 47);
            btnAdd.TabIndex = 10;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(32, 26);
            label6.Name = "label6";
            label6.Size = new Size(236, 21);
            label6.TabIndex = 11;
            label6.Text = "Please enter supplier info here:";
            // 
            // chkBoxActive
            // 
            chkBoxActive.AutoSize = true;
            chkBoxActive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkBoxActive.Location = new Point(33, 277);
            chkBoxActive.Name = "chkBoxActive";
            chkBoxActive.Size = new Size(71, 25);
            chkBoxActive.TabIndex = 12;
            chkBoxActive.Text = "Active";
            chkBoxActive.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(201, 327);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(158, 47);
            btnCancel.TabIndex = 13;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // AddSupplier
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(390, 399);
            Controls.Add(btnCancel);
            Controls.Add(chkBoxActive);
            Controls.Add(label6);
            Controls.Add(btnAdd);
            Controls.Add(txtBoxContact);
            Controls.Add(label4);
            Controls.Add(txtBoxLocation);
            Controls.Add(label3);
            Controls.Add(txtBoxEmail);
            Controls.Add(label2);
            Controls.Add(txtBoxSupplier);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "AddSupplier";
            Text = "AddSupplier";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtBoxSupplier;
        private TextBox txtBoxEmail;
        private Label label2;
        private TextBox txtBoxLocation;
        private Label label3;
        private TextBox txtBoxContact;
        private Label label4;
        private Button btnAdd;
        private Label label6;
        private CheckBox chkBoxActive;
        private Button btnCancel;
    }
}