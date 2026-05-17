using System;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public class ConfirmPasswordDialog : Form
    {
        private TextBox txtPassword;
        private Button btnConfirm;
        private Button btnCancel;
        private Label lblPrompt;

        public string EnteredPassword => txtPassword.Text.Trim();


        public ConfirmPasswordDialog()
        {
            this.Text = "Confirm Your Password";
            this.Size = new System.Drawing.Size(360, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //label
            lblPrompt = new Label
            {
                Text = "Enter your password to confirm this action:",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(310, 20)
            };

            txtPassword = new TextBox
            {
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(310, 25),
                PasswordChar = '●'
            };

            btnConfirm = new Button
            {
                Text = "Confirm",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(160, 95),
                Size = new System.Drawing.Size(80, 30)
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(250, 95),
                Size = new System.Drawing.Size(80, 30)
            };

            this.Controls.AddRange(new Control[] { lblPrompt, txtPassword, btnConfirm, btnCancel });
            this.AcceptButton = btnConfirm;
            this.CancelButton = btnCancel;
        }
    }
}