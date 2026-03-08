namespace InvSys.App
{
    partial class LoginForm
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
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            panel1 = new Syncfusion.Windows.Forms.Tools.GradientPanelExt();
            label7 = new Label();
            linkLabel1 = new LinkLabel();
            panel3 = new Panel();
            panel2 = new Panel();
            txtBoxUserEmail = new TextBox();
            btnExit = new Button();
            btnLogin = new Button();
            checkBoxShowPassword = new CheckBox();
            label1 = new Label();
            txtBoxPassword = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label8 = new Label();
            ((System.ComponentModel.ISupportInitialize)gradientPanel1).BeginInit();
            gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panel1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // gradientPanel1
            // 
            gradientPanel1.BackgroundImage = Properties.Resources.background;
            gradientPanel1.BackgroundImageLayout = ImageLayout.Stretch;
            gradientPanel1.Border3DStyle = Border3DStyle.Flat;
            gradientPanel1.BorderSingle = ButtonBorderStyle.None;
            gradientPanel1.Controls.Add(label6);
            gradientPanel1.Controls.Add(label5);
            gradientPanel1.Controls.Add(label4);
            gradientPanel1.Controls.Add(panel1);
            gradientPanel1.Dock = DockStyle.Fill;
            gradientPanel1.ForeColor = Color.Lavender;
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(1045, 561);
            gradientPanel1.TabIndex = 0;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 24F, FontStyle.Italic);
            label6.ImageAlign = ContentAlignment.BottomRight;
            label6.Location = new Point(58, 292);
            label6.Name = "label6";
            label6.Size = new Size(292, 45);
            label6.TabIndex = 14;
            label6.Text = "Organize on the go.";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 24F, FontStyle.Italic);
            label5.ImageAlign = ContentAlignment.BottomRight;
            label5.Location = new Point(58, 247);
            label5.Name = "label5";
            label5.Size = new Size(340, 45);
            label5.TabIndex = 13;
            label5.Text = "Control your workflow, ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Snow;
            label4.Location = new Point(58, 165);
            label4.Name = "label4";
            label4.Size = new Size(305, 65);
            label4.TabIndex = 12;
            label4.Text = "H2Organizer";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Border = new Padding(10);
            panel1.BorderColor = Color.Transparent;
            panel1.BorderGap = 10;
            panel1.BorderStyle = BorderStyle.None;
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(linkLabel1);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(txtBoxUserEmail);
            panel1.Controls.Add(btnExit);
            panel1.Controls.Add(btnLogin);
            panel1.Controls.Add(checkBoxShowPassword);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtBoxPassword);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.CornerRadius = 30;
            panel1.ForeColor = Color.SlateBlue;
            panel1.Location = new Point(452, 28);
            panel1.Name = "panel1";
            panel1.Size = new Size(562, 501);
            panel1.TabIndex = 0;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI Light", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(7, 13, 33);
            label7.Location = new Point(109, 93);
            label7.Name = "label7";
            label7.Size = new Size(344, 28);
            label7.TabIndex = 18;
            label7.Text = "Welcome! Please enter your credentials.";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.Black;
            linkLabel1.Location = new Point(39, 337);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(238, 25);
            linkLabel1.TabIndex = 17;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "No account? Register here!";
            // 
            // panel3
            // 
            panel3.BackgroundImage = Properties.Resources.lock_24dp_434343_FILL0_wght400_GRAD0_opsz24;
            panel3.BackgroundImageLayout = ImageLayout.Center;
            panel3.Location = new Point(39, 237);
            panel3.Name = "panel3";
            panel3.Size = new Size(30, 28);
            panel3.TabIndex = 16;
            // 
            // panel2
            // 
            panel2.BackgroundImage = Properties.Resources.account_circle_24dp_434343_FILL0_wght400_GRAD0_opsz24;
            panel2.BackgroundImageLayout = ImageLayout.Center;
            panel2.Location = new Point(39, 154);
            panel2.Name = "panel2";
            panel2.Size = new Size(30, 28);
            panel2.TabIndex = 15;
            // 
            // txtBoxUserEmail
            // 
            txtBoxUserEmail.Font = new Font("Segoe UI", 14.25F);
            txtBoxUserEmail.Location = new Point(39, 187);
            txtBoxUserEmail.Name = "txtBoxUserEmail";
            txtBoxUserEmail.PlaceholderText = "Username/email";
            txtBoxUserEmail.Size = new Size(484, 33);
            txtBoxUserEmail.TabIndex = 14;
            txtBoxUserEmail.KeyDown += txtBoxUserEmail_KeyDown;
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
            btnExit.Location = new Point(292, 389);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(227, 48);
            btnExit.TabIndex = 9;
            btnExit.Text = "Exit";
            btnExit.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.MidnightBlue;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 15F);
            btnLogin.ForeColor = Color.AliceBlue;
            btnLogin.Image = Properties.Resources.login_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnLogin.ImageAlign = ContentAlignment.MiddleRight;
            btnLogin.Location = new Point(39, 389);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(238, 48);
            btnLogin.TabIndex = 8;
            btnLogin.Text = "Login";
            btnLogin.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // checkBoxShowPassword
            // 
            checkBoxShowPassword.AutoSize = true;
            checkBoxShowPassword.Font = new Font("Segoe UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            checkBoxShowPassword.ForeColor = Color.FromArgb(7, 13, 33);
            checkBoxShowPassword.Location = new Point(39, 305);
            checkBoxShowPassword.Name = "checkBoxShowPassword";
            checkBoxShowPassword.Size = new Size(157, 29);
            checkBoxShowPassword.TabIndex = 7;
            checkBoxShowPassword.Text = "Show password";
            checkBoxShowPassword.UseVisualStyleBackColor = true;
            checkBoxShowPassword.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 30F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(7, 13, 33);
            label1.Location = new Point(210, 39);
            label1.Name = "label1";
            label1.Size = new Size(143, 54);
            label1.TabIndex = 0;
            label1.Text = "LOGIN";
            // 
            // txtBoxPassword
            // 
            txtBoxPassword.Font = new Font("Segoe UI", 14.25F);
            txtBoxPassword.Location = new Point(39, 266);
            txtBoxPassword.Name = "txtBoxPassword";
            txtBoxPassword.PlaceholderText = "Password";
            txtBoxPassword.Size = new Size(484, 33);
            txtBoxPassword.TabIndex = 6;
            txtBoxPassword.KeyDown += txtBoxPassword_KeyDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 14F);
            label3.ForeColor = Color.FromArgb(7, 13, 33);
            label3.Location = new Point(69, 239);
            label3.Name = "label3";
            label3.Size = new Size(96, 25);
            label3.TabIndex = 5;
            label3.Text = "Password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 14F);
            label2.ForeColor = Color.FromArgb(7, 13, 33);
            label2.Location = new Point(69, 156);
            label2.Name = "label2";
            label2.Size = new Size(158, 25);
            label2.TabIndex = 1;
            label2.Text = "Username/Email:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI Light", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.FromArgb(7, 13, 33);
            label8.Location = new Point(479, 452);
            label8.Name = "label8";
            label8.Size = new Size(34, 20);
            label8.TabIndex = 19;
            label8.Text = "v.1.0";
            label8.Click += label8_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1045, 561);
            Controls.Add(gradientPanel1);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.Navy;
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ((System.ComponentModel.ISupportInitialize)gradientPanel1).EndInit();
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)panel1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private Syncfusion.Windows.Forms.Tools.GradientPanelExt panel1;
        private Label label1;
        private Label label2;
        private TextBox txtBoxPassword;
        private Label label3;
        private CheckBox checkBoxShowPassword;
        private Button btnLogin;
        private Button btnExit;
        private Label label4;
        private TextBox txtBoxUserEmail;
        private Label label6;
        private Label label5;
        private Panel panel2;
        private Label label7;
        private LinkLabel linkLabel1;
        private Panel panel3;
        private Label label8;
    }
}