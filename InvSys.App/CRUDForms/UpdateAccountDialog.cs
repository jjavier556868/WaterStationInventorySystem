using InvSys.Services.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public class UpdateAccountDialog : Form
    {
        private readonly int _accountId;
        private readonly string _currentPasswordHash;

        private Label lblTitle, lblSub, lblDivider2;
        private Panel divider;
        private TextBox txtUsername, txtEmail, txtNewPassword;
        private Label lblUsername, lblEmail, lblNewPassword, lblPasswordNote, lblRole, lblIsActive;
        private ComboBox cmbRole;
        private CheckBox chkIsActive;
        private Button btnConfirm, btnCancel;

        public string NewUsername => txtUsername.Text.Trim();
        public string NewEmail => txtEmail.Text.Trim();
        public string NewPassword { get; private set; }
        public string SelectedRole => cmbRole.SelectedItem?.ToString();
        public bool IsActive => chkIsActive.Checked;

        public UpdateAccountDialog(int accountId, string currentUsername, string currentEmail, string currentPasswordHash, string currentRole, bool isActive)
        {
            _accountId = accountId;
            _currentPasswordHash = currentPasswordHash;

            Text = "Update Account";
            Size = new Size(460, 560);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10f);

            lblTitle = new Label
            {
                Text = "Update Account",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(49, 52, 113),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            lblSub = new Label
            {
                Text = currentUsername,
                Font = new Font("Segoe UI", 10f, FontStyle.Italic),
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = true,
                Location = new Point(22, 50)
            };

            divider = new Panel
            {
                BackColor = Color.FromArgb(49, 52, 113),
                Size = new Size(416, 2),
                Location = new Point(20, 75)
            };

            // Username
            lblUsername = MakeLabel("Username", 90);
            txtUsername = MakeTextBox(currentUsername, 114);

            // Email
            lblEmail = MakeLabel("Email", 162);
            txtEmail = MakeTextBox(currentEmail, 186);

            // Role
            lblRole = MakeLabel("Role", 234);
            cmbRole = new ComboBox
            {
                Location = new Point(22, 258),
                Size = new Size(416, 28),
                Font = new Font("Segoe UI", 10.5f),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            cmbRole.Items.AddRange(new object[] { "User", "Admin" });
            cmbRole.SelectedItem = currentRole == "Admin" ? "Admin" : "User";

            // Active toggle
            lblIsActive = MakeLabel("Account Status", 300);
            chkIsActive = new CheckBox
            {
                Text = "Active",
                Location = new Point(22, 324),
                Size = new Size(416, 24),
                Font = new Font("Segoe UI", 10f),
                Checked = isActive,
                ForeColor = Color.FromArgb(49, 52, 113)
            };

            // Divider 2
            lblDivider2 = new Label
            {
                Text = "Change Password  (leave blank to keep current)",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(120, 120, 120),
                AutoSize = true,
                Location = new Point(22, 360)
            };

            // New Password
            lblNewPassword = MakeLabel("New Password", 384);
            txtNewPassword = MakeTextBox("", 408);
            txtNewPassword.UseSystemPasswordChar = true;

            lblPasswordNote = new Label
            {
                Text = "As admin, you can set a new password directly.",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(150, 100, 0),
                AutoSize = true,
                Location = new Point(22, 440)
            };

            btnConfirm = new Button
            {
                Text = "Save Changes",
                Location = new Point(20, 468),
                Size = new Size(195, 38),
                BackColor = Color.FromArgb(49, 52, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(225, 468),
                Size = new Size(195, 38),
                BackColor = Color.FromArgb(220, 225, 245),
                ForeColor = Color.FromArgb(49, 52, 113),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            Controls.AddRange(new Control[]
            {
                lblTitle, lblSub, divider,
                lblUsername, txtUsername,
                lblEmail, txtEmail,
                lblRole, cmbRole,
                lblIsActive, chkIsActive,
                lblDivider2,
                lblNewPassword, txtNewPassword,
                lblPasswordNote,
                btnConfirm, btnCancel
            });

            AcceptButton = btnConfirm;
            CancelButton = btnCancel;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Username cannot be empty.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Email cannot be empty.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Please enter a valid email address.", "Invalid Email",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewPassword = !string.IsNullOrWhiteSpace(txtNewPassword.Text)
                ? txtNewPassword.Text.Trim()
                : null;
        }

        private Label MakeLabel(string text, int y) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 10f),
            ForeColor = Color.FromArgb(49, 52, 113),
            AutoSize = true,
            Location = new Point(22, y)
        };

        private TextBox MakeTextBox(string text, int y) => new TextBox
        {
            Text = text,
            Location = new Point(22, y),
            Size = new Size(416, 28),
            Font = new Font("Segoe UI", 10.5f),
            BorderStyle = BorderStyle.FixedSingle
        };
    }
}