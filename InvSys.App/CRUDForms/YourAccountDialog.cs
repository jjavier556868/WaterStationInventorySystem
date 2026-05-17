using System;
using System.Drawing;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public class YourAccountDialog : Form
    {
        private readonly string _currentPasswordHash;

        public string NewUsername => txtUsername.Text.Trim();
        public string NewEmail => txtEmail.Text.Trim();
        public string NewPassword { get; private set; }
        public bool WantsToDeleteAccount { get; private set; }

        private TextBox txtUsername, txtEmail, txtOldPassword, txtNewPassword;
        private Button btnSave, btnCancel, btnDeleteAccount;
        private Panel divider;

        public YourAccountDialog(string currentUsername, string currentEmail, string currentPasswordHash)
        {
            _currentPasswordHash = currentPasswordHash;

            Text = "Your Account";
            Size = new Size(460, 580);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10f);

            var lblTitle = new Label
            {
                Text = "Your Account",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(49, 52, 113),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            var lblSub = new Label
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

            var lblUsername = MakeLabel("Username", 90);
            txtUsername = MakeTextBox(currentUsername, 114);

            var lblEmail = MakeLabel("Email", 162);
            txtEmail = MakeTextBox(currentEmail, 186);

            var lblPwSection = new Label
            {
                Text = "Change Password  (leave blank to keep current)",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(120, 120, 120),
                AutoSize = true,
                Location = new Point(22, 240)
            };

            var lblOldPassword = MakeLabel("Current Password", 264);
            txtOldPassword = MakeTextBox("", 288);
            txtOldPassword.UseSystemPasswordChar = true;

            var lblNewPassword = MakeLabel("New Password", 336);
            txtNewPassword = MakeTextBox("", 360);
            txtNewPassword.UseSystemPasswordChar = true;

            btnSave = new Button
            {
                Text = "Save Changes",
                Location = new Point(20, 430),
                Size = new Size(195, 38),
                BackColor = Color.FromArgb(49, 52, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(225, 430),
                Size = new Size(195, 38),
                BackColor = Color.FromArgb(220, 225, 245),
                ForeColor = Color.FromArgb(49, 52, 113),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnDeleteAccount = new Button
            {
                Text = "Delete My Account",
                Location = new Point(20, 480),
                Size = new Size(400, 36),
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f),
                Cursor = Cursors.Hand
            };
            btnDeleteAccount.FlatAppearance.BorderSize = 0;
            btnDeleteAccount.Click += BtnDeleteAccount_Click;

            Controls.AddRange(new Control[]
            {
                lblTitle, lblSub, divider,
                lblUsername, txtUsername,
                lblEmail, txtEmail,
                lblPwSection,
                lblOldPassword, txtOldPassword,
                lblNewPassword, txtNewPassword,
                btnSave, btnCancel, btnDeleteAccount
            });

            AcceptButton = btnSave;
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Username cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Please enter a valid email address.", "Invalid Email",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("Email cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool wantsNewPassword = !string.IsNullOrWhiteSpace(txtNewPassword.Text);
            bool providedOldPassword = !string.IsNullOrWhiteSpace(txtOldPassword.Text);

            if (wantsNewPassword || providedOldPassword)
            {
                if (!providedOldPassword)
                {
                    DialogResult = DialogResult.None;
                    MessageBox.Show("Enter your current password to set a new one.", "Current Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!wantsNewPassword)
                {
                    DialogResult = DialogResult.None;
                    MessageBox.Show("Please enter the new password.", "New Password Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!VerifyPassword(txtOldPassword.Text.Trim(), _currentPasswordHash))
                {
                    DialogResult = DialogResult.None;
                    MessageBox.Show("The current password you entered is incorrect.", "Wrong Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                NewPassword = txtNewPassword.Text.Trim();
            }
            else
            {
                NewPassword = null;
            }
        }

        private void BtnDeleteAccount_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to permanently delete your account?\n\nThis action cannot be undone.",
                "Delete Account",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            WantsToDeleteAccount = true;
            DialogResult = DialogResult.OK;
        }

        private static bool VerifyPassword(string plaintext, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(plaintext, hash);
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