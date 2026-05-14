using System;
using System.Drawing;
using System.Windows.Forms;

namespace InvSys.App.CRUDForms
{
    public class UpdateStockDialog : Form
    {
        public int EnteredQuantity { get; private set; }

        private Label lblTitle, lblSub, lblAvail, lblInput;
        private TextBox txtQuantity;
        private Button btnConfirm, btnCancel;

        public UpdateStockDialog(string productName, int currentAvailable)
        {
            Text = "Update Available Stock";
            Size = new Size(420, 280);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10f);

            lblTitle = new Label
            {
                Text = "Update Stock Quantity",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.FromArgb(49, 52, 113),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            lblSub = new Label
            {
                Text = productName,
                Font = new Font("Segoe UI", 10f, FontStyle.Italic),
                ForeColor = Color.FromArgb(90, 90, 90),
                AutoSize = true,
                Location = new Point(22, 50)
            };

            var divider = new Panel
            {
                BackColor = Color.FromArgb(49, 52, 113),
                Size = new Size(376, 2),
                Location = new Point(20, 75)
            };

            lblAvail = new Label
            {
                Text = $"Currently Available:  {currentAvailable} unit(s)",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Location = new Point(22, 90)
            };

            lblInput = new Label
            {
                Text = "New Available Quantity:",
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(49, 52, 113),
                AutoSize = true,
                Location = new Point(22, 125)
            };

            txtQuantity = new TextBox
            {
                Text = currentAvailable.ToString(),
                Location = new Point(22, 148),
                Size = new Size(374, 30),
                Font = new Font("Segoe UI", 11f),
                BorderStyle = BorderStyle.FixedSingle,
            };

            btnConfirm = new Button
            {
                Text = "Confirm",
                Location = new Point(22, 196),
                Size = new Size(180, 38),
                BackColor = Color.FromArgb(49, 52, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.OK
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += (s, e) =>
            {
                if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty < 0)
                {
                    MessageBox.Show("Please enter a valid non-negative whole number.", "Invalid Input",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }
                EnteredQuantity = qty;
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(216, 196),
                Size = new Size(180, 38),
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
                lblTitle, lblSub, divider, lblAvail, lblInput, txtQuantity, btnConfirm, btnCancel
            });

            AcceptButton = btnConfirm;
            CancelButton = btnCancel;
        }
    }
}