using InvSys.Domain.Models.Account;
using InvSys.Infrastructure;
using BCrypt.Net;  // Add this for BCrypt
using System;
using System.Linq;
using System.Windows.Forms;

namespace InvSys.App
{
    public partial class LoginForm : Form
    {
        private void SyncfusionLicensing()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdlWX1fc3RdRWJcV0J3WkVWYEs=");
        }

        public LoginForm()
        {
            SyncfusionLicensing();
            InitializeComponent();
            TxtBoxShowPasswordChar(false);
        }

        private void TxtBoxShowPasswordChar(bool _bool)
        {
            txtBoxPassword.UseSystemPasswordChar = !_bool;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TxtBoxShowPasswordChar(checkBoxShowPassword.Checked);
        }

        private void AddUserToDatabase(string username, string email, string password)
        {
            // Generate BCrypt hash (work factor 12 is secure default)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var userAccount = new UserAccount
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            using (var context = new AppDbContext())
            {
                // Check if user already exists
                if (context.UserAccounts.Any(u => u.Username == username))
                {
                    MessageBox.Show("Username already exists!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                context.UserAccounts.Add(userAccount);
                context.SaveChanges();
                MessageBox.Show("User created successfully!", "Success");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string loginInput = txtBoxUserEmail.Text.Trim();
            string password = txtBoxPassword.Text;

            if (string.IsNullOrEmpty(loginInput) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username/email and password.", "Validation Error");
                return;
            }

            using (var context = new AppDbContext())
            {
                // Query by Username or Email
                var user = context.UserAccounts
                    .FirstOrDefault(u => u.Username == loginInput || u.Email == loginInput);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    MessageBox.Show("Login successful!", "Welcome");
                    if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    {
                        string greeting = $"Welcome back, {user.Username}!";
                        MessageBox.Show(greeting, "Login Successful");
                        this.Hide();
                        MainInventory mainForm = new MainInventory();
                        mainForm.Show();
                    }

                }
                else
                {
                    MessageBox.Show("Invalid username/email or password.", "Login Failed");
                    txtBoxPassword.Clear();
                    txtBoxUserEmail.Focus();
                }
            }
        }
    }
}
