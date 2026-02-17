using BCrypt.Net;  
using InvSys.Domain.Models.Account;
using InvSys.Infrastructure;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

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
            InitializeDatabases();
        }

        private void InitializeDatabases()
        {
            using var invContext = new InventoryDbContext();
            invContext.Database.EnsureCreated();

            using var accContext = new AccountsDbContext();
            accContext.Database.EnsureCreated(); 
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
            
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var userAccount = new UserAccount
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.User
            };

            using (var context = new AccountsDbContext())
            {
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

        private void AddAdminToDatabase(string username, string email, string password)
        {

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var userAccount = new UserAccount
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = UserRole.Admin
            };

            using (var context = new AccountsDbContext())
            {
                if (context.UserAccounts.Any(u => u.Username == username))
                {
                    MessageBox.Show("Username already exists!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                context.UserAccounts.Add(userAccount);
                context.SaveChanges();
                MessageBox.Show("Admin created successfully!", "Success");
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

            using (var context = new AccountsDbContext())
            {
                var user = context.UserAccounts
                    .FirstOrDefault(u => u.Username == loginInput || u.Email == loginInput);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    if (user.Role == UserRole.Admin)
                    {
                        MessageBox.Show($"Welcome back, Admin {user.Username}!", "Admin Login Successful");
                    }
                    else
                    {
                        MessageBox.Show($"Welcome back, {user.Username}!", "Login Successful");
                    }

                    this.Hide();
                    var mainInv = new MainInventory(user.Username, user.Role); 
                    mainInv.Closed += (s, args) => this.Close();
                    mainInv.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username/email or password.", "Login Failed");
                    txtBoxPassword.Clear();
                    txtBoxUserEmail.Focus();
                }
            }
        }


        private void txtBoxUserEmail_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                txtBoxPassword.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
