using BCrypt.Net;
using InvSys.Domain.Models.Account;
using InvSys.Domain.Models.Enums;
using InvSys.Infrastructure;
using System.Linq;
using System.Windows.Forms;

namespace InvSys.App
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
            txtBoxPassword.UseSystemPasswordChar = true;
            textBoxRetypePassword.UseSystemPasswordChar = true;

            btnRegister.Click += btnRegister_Click;
            btnExit.Click += btnExit_Click;
            linkLabelGoToLogin.LinkClicked += linkLabelGoToLogin_LinkClicked;
        }

        private void checkBoxShowPassword_CheckedChanged_1(object sender, EventArgs e)
        {
            txtBoxPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
        }

        private void checkBoxShowRetypedPass_CheckedChanged_1(object sender, EventArgs e)
        {
            textBoxRetypePassword.UseSystemPasswordChar = !checkBoxShowRetypedPass.Checked;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtBoxUser.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string password = txtBoxPassword.Text;
            string retypePassword = textBoxRetypePassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(retypePassword))
            {
                MessageBox.Show("All fields are required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != retypePassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxPassword.Clear();
                textBoxRetypePassword.Clear();
                txtBoxPassword.Focus();
                return;
            }

            using var context = new AccountsDbContext();

            if (context.UserAccounts.Any(u => u.Username == username))
            {
                MessageBox.Show("That username is already taken.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (context.UserAccounts.Any(u => u.Email == email))
            {
                MessageBox.Show("That email is already registered.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new UserAccount
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.User
            };

            context.UserAccounts.Add(newUser);
            context.SaveChanges();

            MessageBox.Show("Account created successfully! You can now log in.", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
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

        private void linkLabelGoToLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabelGoToLogin_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}