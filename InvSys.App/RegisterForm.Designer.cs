namespace InvSys.App
{
    partial class RegisterForm
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
            gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            linkLabel1 = new LinkLabel();
            btnRegister = new Button();
            textBoxPassRegis = new TextBox();
            txtBoxEmailRegis = new TextBox();
            txtBoxUserRegis = new TextBox();
            label4 = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)gradientPanel1).BeginInit();
            gradientPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // gradientPanel1
            // 
            gradientPanel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Vertical, Color.DarkBlue, Color.FromArgb(26, 88, 166));
            gradientPanel1.Controls.Add(label1);
            gradientPanel1.Controls.Add(linkLabel1);
            gradientPanel1.Controls.Add(btnRegister);
            gradientPanel1.Controls.Add(textBoxPassRegis);
            gradientPanel1.Controls.Add(txtBoxEmailRegis);
            gradientPanel1.Controls.Add(txtBoxUserRegis);
            gradientPanel1.Controls.Add(label4);
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(394, 448);
            gradientPanel1.TabIndex = 0;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel1.DisabledLinkColor = Color.Transparent;
            linkLabel1.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.White;
            linkLabel1.Location = new Point(67, 355);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(235, 21);
            linkLabel1.TabIndex = 20;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Already have an account? Login.";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.MidnightBlue;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRegister.ForeColor = Color.AliceBlue;
            btnRegister.Image = Properties.Resources.login_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnRegister.ImageAlign = ContentAlignment.MiddleRight;
            btnRegister.Location = new Point(52, 293);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(277, 45);
            btnRegister.TabIndex = 18;
            btnRegister.Text = "Sign Up";
            btnRegister.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // textBoxPassRegis
            // 
            textBoxPassRegis.Font = new Font("Segoe UI", 14.25F);
            textBoxPassRegis.Location = new Point(52, 214);
            textBoxPassRegis.Name = "textBoxPassRegis";
            textBoxPassRegis.PlaceholderText = "Password";
            textBoxPassRegis.Size = new Size(277, 33);
            textBoxPassRegis.TabIndex = 17;
            // 
            // txtBoxEmailRegis
            // 
            txtBoxEmailRegis.Font = new Font("Segoe UI", 14.25F);
            txtBoxEmailRegis.Location = new Point(52, 153);
            txtBoxEmailRegis.Name = "txtBoxEmailRegis";
            txtBoxEmailRegis.PlaceholderText = "Email";
            txtBoxEmailRegis.Size = new Size(277, 33);
            txtBoxEmailRegis.TabIndex = 16;
            // 
            // txtBoxUserRegis
            // 
            txtBoxUserRegis.Font = new Font("Segoe UI", 14.25F);
            txtBoxUserRegis.Location = new Point(52, 94);
            txtBoxUserRegis.Name = "txtBoxUserRegis";
            txtBoxUserRegis.PlaceholderText = "Username";
            txtBoxUserRegis.Size = new Size(277, 33);
            txtBoxUserRegis.TabIndex = 15;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.FlatStyle = FlatStyle.Flat;
            label4.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            label4.ForeColor = Color.Snow;
            label4.Location = new Point(74, 27);
            label4.Name = "label4";
            label4.Size = new Size(228, 41);
            label4.TabIndex = 13;
            label4.Text = "Create Account";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 402);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(394, 450);
            Controls.Add(gradientPanel1);
            Name = "RegisterForm";
            Text = "RegisterForm";
            ((System.ComponentModel.ISupportInitialize)gradientPanel1).EndInit();
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Label label4;
        private TextBox textBoxPassRegis;
        private TextBox txtBoxEmailRegis;
        private TextBox txtBoxUserRegis;
        private Button btnRegister;
        private LinkLabel linkLabel1;
        private Label label1;
    }
}