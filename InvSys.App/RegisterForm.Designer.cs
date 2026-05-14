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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            panel1 = new Syncfusion.Windows.Forms.Tools.GradientPanelExt();
            panel6 = new Panel();
            textBoxEmail = new TextBox();
            label5 = new Label();
            checkBoxShowRetypedPass = new CheckBox();
            panel4 = new Panel();
            label7 = new Label();
            textBoxRetypePassword = new TextBox();
            linkLabelGoToLogin = new LinkLabel();
            label4 = new Label();
            panel3 = new Panel();
            panel2 = new Panel();
            txtBoxUser = new TextBox();
            btnExit = new Button();
            btnRegister = new Button();
            checkBoxShowPassword = new CheckBox();
            label1 = new Label();
            txtBoxPassword = new TextBox();
            label3 = new Label();
            label2 = new Label();
            panel5 = new Panel();
            ((System.ComponentModel.ISupportInitialize)panel1).BeginInit();
            panel1.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Border = new Padding(10);
            panel1.BorderColor = Color.Transparent;
            panel1.BorderGap = 10;
            panel1.BorderStyle = BorderStyle.None;
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(textBoxEmail);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(checkBoxShowRetypedPass);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(textBoxRetypePassword);
            panel1.Controls.Add(linkLabelGoToLogin);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(txtBoxUser);
            panel1.Controls.Add(btnExit);
            panel1.Controls.Add(btnRegister);
            panel1.Controls.Add(checkBoxShowPassword);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtBoxPassword);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.CornerRadius = 30;
            panel1.ForeColor = Color.SlateBlue;
            panel1.Location = new Point(34, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(562, 520);
            panel1.TabIndex = 1;
            // 
            // panel6
            // 
            panel6.BackgroundImage = (Image)resources.GetObject("panel6.BackgroundImage");
            panel6.BackgroundImageLayout = ImageLayout.Center;
            panel6.Location = new Point(39, 190);
            panel6.Name = "panel6";
            panel6.Size = new Size(30, 28);
            panel6.TabIndex = 36;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Font = new Font("Segoe UI", 14.25F);
            textBoxEmail.Location = new Point(39, 223);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.PlaceholderText = "Email";
            textBoxEmail.Size = new Size(484, 33);
            textBoxEmail.TabIndex = 35;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 14F);
            label5.ForeColor = Color.FromArgb(7, 13, 33);
            label5.Location = new Point(69, 192);
            label5.Name = "label5";
            label5.Size = new Size(64, 25);
            label5.TabIndex = 34;
            label5.Text = "Email:";
            // 
            // checkBoxShowRetypedPass
            // 
            checkBoxShowRetypedPass.AutoSize = true;
            checkBoxShowRetypedPass.Font = new Font("Segoe UI", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            checkBoxShowRetypedPass.ForeColor = Color.FromArgb(7, 13, 33);
            checkBoxShowRetypedPass.Location = new Point(346, 326);
            checkBoxShowRetypedPass.Name = "checkBoxShowRetypedPass";
            checkBoxShowRetypedPass.Size = new Size(177, 24);
            checkBoxShowRetypedPass.TabIndex = 33;
            checkBoxShowRetypedPass.Text = "Show retyped password";
            checkBoxShowRetypedPass.UseVisualStyleBackColor = true;
            checkBoxShowRetypedPass.CheckedChanged += checkBoxShowRetypedPass_CheckedChanged_1;
            // 
            // panel4
            // 
            panel4.BackgroundImage = Properties.Resources.lock_24dp_434343_FILL0_wght400_GRAD0_opsz24;
            panel4.BackgroundImageLayout = ImageLayout.Center;
            panel4.Location = new Point(38, 324);
            panel4.Name = "panel4";
            panel4.Size = new Size(30, 28);
            panel4.TabIndex = 32;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI Light", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(7, 13, 33);
            label7.Location = new Point(109, 88);
            label7.Name = "label7";
            label7.Size = new Size(344, 28);
            label7.TabIndex = 31;
            label7.Text = "Welcome! Please enter your credentials.";
            // 
            // textBoxRetypePassword
            // 
            textBoxRetypePassword.Font = new Font("Segoe UI", 14.25F);
            textBoxRetypePassword.Location = new Point(38, 353);
            textBoxRetypePassword.Name = "textBoxRetypePassword";
            textBoxRetypePassword.PlaceholderText = "Retype password";
            textBoxRetypePassword.Size = new Size(484, 33);
            textBoxRetypePassword.TabIndex = 31;
            // 
            // linkLabelGoToLogin
            // 
            linkLabelGoToLogin.AutoSize = true;
            linkLabelGoToLogin.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabelGoToLogin.LinkColor = Color.Black;
            linkLabelGoToLogin.Location = new Point(39, 401);
            linkLabelGoToLogin.Name = "linkLabelGoToLogin";
            linkLabelGoToLogin.Size = new Size(328, 25);
            linkLabelGoToLogin.TabIndex = 30;
            linkLabelGoToLogin.TabStop = true;
            linkLabelGoToLogin.Text = "Already have an account? Login here!";
            linkLabelGoToLogin.LinkClicked += linkLabelGoToLogin_LinkClicked_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 14F);
            label4.ForeColor = Color.FromArgb(7, 13, 33);
            label4.Location = new Point(68, 326);
            label4.Name = "label4";
            label4.Size = new Size(168, 25);
            label4.TabIndex = 30;
            label4.Text = "Re-type Password:";
            // 
            // panel3
            // 
            panel3.BackgroundImage = Properties.Resources.lock_24dp_434343_FILL0_wght400_GRAD0_opsz24;
            panel3.BackgroundImageLayout = ImageLayout.Center;
            panel3.Location = new Point(38, 261);
            panel3.Name = "panel3";
            panel3.Size = new Size(30, 28);
            panel3.TabIndex = 29;
            // 
            // panel2
            // 
            panel2.BackgroundImage = Properties.Resources.account_circle_24dp_434343_FILL0_wght400_GRAD0_opsz24;
            panel2.BackgroundImageLayout = ImageLayout.Center;
            panel2.Location = new Point(39, 123);
            panel2.Name = "panel2";
            panel2.Size = new Size(30, 28);
            panel2.TabIndex = 28;
            // 
            // txtBoxUser
            // 
            txtBoxUser.Font = new Font("Segoe UI", 14.25F);
            txtBoxUser.Location = new Point(39, 156);
            txtBoxUser.Name = "txtBoxUser";
            txtBoxUser.PlaceholderText = "Username";
            txtBoxUser.Size = new Size(484, 33);
            txtBoxUser.TabIndex = 27;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(242, 64, 64);
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 15F);
            btnExit.ForeColor = SystemColors.ButtonFace;
            btnExit.Image = Properties.Resources.exit_to_app_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnExit.ImageAlign = ContentAlignment.MiddleRight;
            btnExit.Location = new Point(296, 444);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(227, 48);
            btnExit.TabIndex = 26;
            btnExit.Text = "Exit";
            btnExit.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExit.UseVisualStyleBackColor = false;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.MidnightBlue;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 15F);
            btnRegister.ForeColor = Color.AliceBlue;
            btnRegister.Image = Properties.Resources.login_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnRegister.ImageAlign = ContentAlignment.MiddleRight;
            btnRegister.Location = new Point(43, 444);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(238, 48);
            btnRegister.TabIndex = 25;
            btnRegister.Text = "Register";
            btnRegister.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRegister.UseVisualStyleBackColor = false;
            // 
            // checkBoxShowPassword
            // 
            checkBoxShowPassword.AutoSize = true;
            checkBoxShowPassword.Font = new Font("Segoe UI", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            checkBoxShowPassword.ForeColor = Color.FromArgb(7, 13, 33);
            checkBoxShowPassword.Location = new Point(397, 266);
            checkBoxShowPassword.Name = "checkBoxShowPassword";
            checkBoxShowPassword.Size = new Size(126, 24);
            checkBoxShowPassword.TabIndex = 24;
            checkBoxShowPassword.Text = "Show password";
            checkBoxShowPassword.UseVisualStyleBackColor = true;
            checkBoxShowPassword.CheckedChanged += checkBoxShowPassword_CheckedChanged_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 30F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(7, 13, 33);
            label1.Location = new Point(182, 34);
            label1.Name = "label1";
            label1.Size = new Size(199, 54);
            label1.TabIndex = 20;
            label1.Text = "REGISTER";
            // 
            // txtBoxPassword
            // 
            txtBoxPassword.Font = new Font("Segoe UI", 14.25F);
            txtBoxPassword.Location = new Point(38, 290);
            txtBoxPassword.Name = "txtBoxPassword";
            txtBoxPassword.PlaceholderText = "Password";
            txtBoxPassword.Size = new Size(484, 33);
            txtBoxPassword.TabIndex = 23;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 14F);
            label3.ForeColor = Color.FromArgb(7, 13, 33);
            label3.Location = new Point(68, 263);
            label3.Name = "label3";
            label3.Size = new Size(96, 25);
            label3.TabIndex = 22;
            label3.Text = "Password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 14F);
            label2.ForeColor = Color.FromArgb(7, 13, 33);
            label2.Location = new Point(69, 125);
            label2.Name = "label2";
            label2.Size = new Size(103, 25);
            label2.TabIndex = 21;
            label2.Text = "Username:";
            // 
            // panel5
            // 
            panel5.BackgroundImage = Properties.Resources.background;
            panel5.BackgroundImageLayout = ImageLayout.None;
            panel5.Controls.Add(panel1);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(630, 537);
            panel5.TabIndex = 2;
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(630, 537);
            Controls.Add(panel5);
            MinimizeBox = false;
            Name = "RegisterForm";
            Text = "Register";
            ((System.ComponentModel.ISupportInitialize)panel1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanelExt panel1;
        private CheckBox checkBoxShowRetypedPass;
        private Panel panel4;
        private Label label7;
        private TextBox textBoxRetypePassword;
        private LinkLabel linkLabelGoToLogin;
        private Label label4;
        private Panel panel3;
        private Panel panel2;
        private TextBox txtBoxUser;
        private Button btnExit;
        private Button btnRegister;
        private CheckBox checkBoxShowPassword;
        private Label label1;
        private TextBox txtBoxPassword;
        private Label label3;
        private Label label2;
        private Panel panel5;
        private Panel panel6;
        private TextBox textBoxEmail;
        private Label label5;
    }
}