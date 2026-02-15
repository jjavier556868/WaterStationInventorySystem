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
            label4 = new Label();
            panel1 = new Syncfusion.Windows.Forms.Tools.GradientPanelExt();
            txtBoxUserEmail = new TextBox();
            linkLabelRegister = new LinkLabel();
            panel4 = new Panel();
            panel3 = new Panel();
            panel2 = new Panel();
            btnExit = new Button();
            btnLogin = new Button();
            checkBoxShowPassword = new CheckBox();
            label1 = new Label();
            txtBoxPassword = new TextBox();
            label3 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)gradientPanel1).BeginInit();
            gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panel1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // gradientPanel1
            // 
            gradientPanel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Vertical, Color.DarkBlue, Color.FromArgb(26, 88, 166));
            gradientPanel1.BackgroundImageLayout = ImageLayout.Stretch;
            gradientPanel1.Border3DStyle = Border3DStyle.Flat;
            gradientPanel1.BorderSingle = ButtonBorderStyle.None;
            gradientPanel1.Controls.Add(label4);
            gradientPanel1.Controls.Add(panel1);
            gradientPanel1.Dock = DockStyle.Fill;
            gradientPanel1.ForeColor = Color.Lavender;
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(1045, 566);
            gradientPanel1.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 36F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Snow;
            label4.Location = new Point(90, 231);
            label4.Name = "label4";
            label4.Size = new Size(305, 65);
            label4.TabIndex = 12;
            label4.Text = "H2Organizer";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Border = new Padding(10);
            panel1.BorderColor = Color.Transparent;
            panel1.BorderGap = 10;
            panel1.BorderStyle = BorderStyle.None;
            panel1.Controls.Add(txtBoxUserEmail);
            panel1.Controls.Add(linkLabelRegister);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnExit);
            panel1.Controls.Add(btnLogin);
            panel1.Controls.Add(checkBoxShowPassword);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtBoxPassword);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.CornerRadius = 30;
            panel1.Dock = DockStyle.Right;
            panel1.ForeColor = Color.SlateBlue;
            panel1.Location = new Point(469, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(572, 562);
            panel1.TabIndex = 0;
            // 
            // txtBoxUserEmail
            // 
            txtBoxUserEmail.Font = new Font("Segoe UI", 14.25F);
            txtBoxUserEmail.Location = new Point(45, 181);
            txtBoxUserEmail.Name = "txtBoxUserEmail";
            txtBoxUserEmail.PlaceholderText = "Username/email";
            txtBoxUserEmail.Size = new Size(484, 33);
            txtBoxUserEmail.TabIndex = 14;
            txtBoxUserEmail.KeyDown += txtBoxUserEmail_KeyDown;
            // 
            // linkLabelRegister
            // 
            linkLabelRegister.ActiveLinkColor = Color.LightSkyBlue;
            linkLabelRegister.AutoSize = true;
            linkLabelRegister.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            linkLabelRegister.LinkColor = Color.White;
            linkLabelRegister.Location = new Point(46, 345);
            linkLabelRegister.Name = "linkLabelRegister";
            linkLabelRegister.Size = new Size(192, 21);
            linkLabelRegister.TabIndex = 12;
            linkLabelRegister.TabStop = true;
            linkLabelRegister.Text = "No account? Register here";
            linkLabelRegister.VisitedLinkColor = Color.FromArgb(64, 0, 64);
            // 
            // panel4
            // 
            panel4.BackgroundImage = Properties.Resources.log_in;
            panel4.BackgroundImageLayout = ImageLayout.Stretch;
            panel4.Location = new Point(168, 41);
            panel4.Name = "panel4";
            panel4.Size = new Size(65, 65);
            panel4.TabIndex = 11;
            // 
            // panel3
            // 
            panel3.BackgroundImage = Properties.Resources.padlock;
            panel3.BackgroundImageLayout = ImageLayout.Stretch;
            panel3.Location = new Point(45, 231);
            panel3.Name = "panel3";
            panel3.Size = new Size(24, 25);
            panel3.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.BackgroundImage = Properties.Resources.user;
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Location = new Point(45, 150);
            panel2.Name = "panel2";
            panel2.Size = new Size(24, 25);
            panel2.TabIndex = 10;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(242, 64, 64);
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExit.ForeColor = SystemColors.ButtonFace;
            btnExit.Location = new Point(46, 466);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(483, 55);
            btnExit.TabIndex = 9;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.MidnightBlue;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.AliceBlue;
            btnLogin.ImageAlign = ContentAlignment.MiddleRight;
            btnLogin.Location = new Point(46, 405);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(483, 55);
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
            checkBoxShowPassword.ForeColor = Color.WhiteSmoke;
            checkBoxShowPassword.Location = new Point(46, 313);
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
            label1.Font = new Font("Segoe UI Light", 30F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Snow;
            label1.Location = new Point(258, 52);
            label1.Name = "label1";
            label1.Size = new Size(138, 54);
            label1.TabIndex = 0;
            label1.Text = "LOGIN";
            // 
            // txtBoxPassword
            // 
            txtBoxPassword.Font = new Font("Segoe UI", 14.25F);
            txtBoxPassword.Location = new Point(45, 260);
            txtBoxPassword.Name = "txtBoxPassword";
            txtBoxPassword.PlaceholderText = "Password";
            txtBoxPassword.Size = new Size(484, 33);
            txtBoxPassword.TabIndex = 6;
            txtBoxPassword.KeyDown += txtBoxPassword_KeyDown;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.WhiteSmoke;
            label3.Location = new Point(75, 231);
            label3.Name = "label3";
            label3.Size = new Size(96, 25);
            label3.TabIndex = 5;
            label3.Text = "Password:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.WhiteSmoke;
            label2.Location = new Point(75, 150);
            label2.Name = "label2";
            label2.Size = new Size(158, 25);
            label2.TabIndex = 1;
            label2.Text = "Username/Email:";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1045, 566);
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
        private Panel panel4;
        private Panel panel3;
        private Panel panel2;
        private LinkLabel linkLabelRegister;
        private TextBox txtBoxUserEmail;
    }
}