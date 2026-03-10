using InvSys.Domain.Models.Account;
using InvSys.Infrastructure;
using System;
using System.Linq;
using System.Windows.Forms;
using BCrypt.Net;

namespace InvSys.App
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();

            // Set password character masking
            textBoxPassRegis.UseSystemPasswordChar = true;

            // Set up enter key navigation
            this.AcceptButton = btnRegister;

            // Initialize database
            EnsureDatabaseCreated();
        }

        private void EnsureDatabaseCreated()
        {
            try
            {
                using var context = new AccountsDbContext();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database initialization error: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Get values from form
            string username = txtBoxUserRegis.Text.Trim();
            string email = txtBoxEmailRegis.Text.Trim();
            string password = textBoxPassRegis.Text;

            // Validate inputs
            if (!ValidateInputs(username, email, password))
                return;

            try
            {
                using (var context = new AccountsDbContext())
                {
                    // Check if username already exists (case-insensitive)
                    if (context.UserAccounts.Any(u => u.Username.ToLower() == username.ToLower()))
                    {
                        MessageBox.Show("Username already exists! Please choose another.",
                            "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtBoxUserRegis.Focus();
                        txtBoxUserRegis.SelectAll();
                        return;
                    }

                    // Check if email already exists (case-insensitive)
                    if (context.UserAccounts.Any(u => u.Email.ToLower() == email.ToLower()))
                    {
                        MessageBox.Show("Email already registered! Please use another email or login.",
                            "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtBoxEmailRegis.Focus();
                        txtBoxEmailRegis.SelectAll();
                        return;
                    }

                    // Create new user account
                    var newUser = new UserAccount
                    {
                        Username = username,
                        Email = email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                        Role = Domain.Models.Enums.UserRole.User, // Default role
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = username // Self-registration
                    };

                    context.UserAccounts.Add(newUser);
                    context.SaveChanges();

                    // Show success message
                    MessageBox.Show($"Registration successful!\n\nWelcome, {username}!\nYou can now login with your credentials.",
                        "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Ask if user wants to login now
                    var result = MessageBox.Show("Registration successful! Would you like to login now?",
                        "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Navigate to login form
                        this.Hide();
                        var loginForm = new LoginForm();
                        loginForm.FormClosed += (s, args) => this.Close();
                        loginForm.Show();
                    }
                    else
                    {
                        // Return to login form
                        this.Hide();
                        var loginForm = new LoginForm();
                        loginForm.FormClosed += (s, args) => this.Close();
                        loginForm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs(string username, string email, string password)
        {
            // Username validation
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxUserRegis.Focus();
                return false;
            }

            if (username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxUserRegis.Focus();
                txtBoxUserRegis.SelectAll();
                return false;
            }

            if (username.Length > 50)
            {
                MessageBox.Show("Username must not exceed 50 characters!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxUserRegis.Focus();
                txtBoxUserRegis.SelectAll();
                return false;
            }

            if (username.Contains(" "))
            {
                MessageBox.Show("Username cannot contain spaces!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxUserRegis.Focus();
                txtBoxUserRegis.SelectAll();
                return false;
            }

            // Email validation
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmailRegis.Focus();
                return false;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address!\n(e.g., name@example.com)",
                    "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxEmailRegis.Focus();
                txtBoxEmailRegis.SelectAll();
                return false;
            }

            // Password validation
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Password is required!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassRegis.Focus();
                return false;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassRegis.Focus();
                textBoxPassRegis.SelectAll();
                return false;
            }

            if (password.Length > 100)
            {
                MessageBox.Show("Password must not exceed 100 characters!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassRegis.Focus();
                textBoxPassRegis.SelectAll();
                return false;
            }

            // Optional: Add password complexity requirements
            if (!password.Any(char.IsDigit))
            {
                MessageBox.Show("Password must contain at least one number (0-9)!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassRegis.Focus();
                textBoxPassRegis.SelectAll();
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                MessageBox.Show("Password must contain at least one uppercase letter (A-Z)!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassRegis.Focus();
                textBoxPassRegis.SelectAll();
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                MessageBox.Show("Password must contain at least one lowercase letter (a-z)!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPassRegis.Focus();
                textBoxPassRegis.SelectAll();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Navigate to login form
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
        }

        // Optional: Handle Enter key navigation
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (txtBoxUserRegis.Focused)
                {
                    txtBoxEmailRegis.Focus();
                    return true;
                }
                if (txtBoxEmailRegis.Focused)
                {
                    textBoxPassRegis.Focus();
                    return true;
                }
                if (textBoxPassRegis.Focused)
                {
                    btnRegister.PerformClick();
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}