
using System;
using System.Drawing;
using System.Windows.Forms;

namespace InvSys.App
{
    public class PaymentDialog : Form
    {
        public string SelectedPaymentMethod => cboPayment.SelectedItem?.ToString();
        public decimal AmountPaid { get; private set; }

        private readonly decimal _totalAmount;

        private Label lblPaymentLabel;
        private ComboBox cboPayment;
        private Label lblAmountLabel;
        private TextBox txtAmount;
        private Label lblChangeLabel;
        private Label lblChangeValue;
        private Button btnOk;
        private Button btnCancel;

        public PaymentDialog(decimal totalAmount)
        {
            _totalAmount = totalAmount;
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Mode of Payment";
            this.Size = new Size(400, 280);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 242, 250);
            this.Font = new Font("Segoe UI", 10f);

            // Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = Color.FromArgb(49, 52, 113)
            };
            var lblTitle = new Label
            {
                Text = $"Total: ₱{_totalAmount:N2}",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHeader.Controls.Add(lblTitle);

            // Payment method
            lblPaymentLabel = new Label
            {
                Text = "Payment Method:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Location = new Point(20, 68),
                AutoSize = true
            };
            cboPayment = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10f),
                Location = new Point(160, 64),
                Width = 200
            };
            cboPayment.Items.AddRange(new object[]
            {
                "Cash",
                "GCash",
                "Maya",
                "Credit/Debit Card"
            });
            cboPayment.SelectedIndex = 0;
            cboPayment.SelectedIndexChanged += (s, e) =>
            {
                bool isCash = cboPayment.SelectedItem?.ToString() == "Cash";
                lblAmountLabel.Visible = isCash;
                txtAmount.Visible = isCash;
                lblChangeLabel.Visible = isCash;
                lblChangeValue.Visible = isCash;
                this.Height = isCash ? 280 : 200;
            };

            // Amount paid (cash only)
            lblAmountLabel = new Label
            {
                Text = "Amount Paid:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Location = new Point(20, 106),
                AutoSize = true
            };
            txtAmount = new TextBox
            {
                Font = new Font("Segoe UI", 10f),
                Location = new Point(160, 102),
                Width = 200
            };
            txtAmount.TextChanged += (s, e) =>
            {
                if (decimal.TryParse(txtAmount.Text.Trim(), out decimal paid))
                {
                    decimal change = paid - _totalAmount;
                    lblChangeValue.Text = change >= 0 ? $"₱{change:N2}" : "Insufficient";
                    lblChangeValue.ForeColor = change >= 0
                        ? Color.FromArgb(0, 140, 70)
                        : Color.Firebrick;
                }
                else
                {
                    lblChangeValue.Text = "₱0.00";
                    lblChangeValue.ForeColor = Color.FromArgb(0, 140, 70);
                }
            };

            // Change
            lblChangeLabel = new Label
            {
                Text = "Change:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Location = new Point(20, 140),
                AutoSize = true
            };
            lblChangeValue = new Label
            {
                Text = "₱0.00",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 140, 70),
                Location = new Point(160, 140),
                AutoSize = true
            };

            // Buttons
            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 36),
                Location = new Point(20, 200)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            btnOk = new Button
            {
                Text = "Confirm",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 120, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(110, 36),
                Location = new Point(250, 200)
            };
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.Click += BtnOk_Click;

            this.Controls.AddRange(new Control[]
            {
                pnlHeader,
                lblPaymentLabel, cboPayment,
                lblAmountLabel,  txtAmount,
                lblChangeLabel,  lblChangeValue,
                btnCancel,       btnOk
            });
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            bool isCash = cboPayment.SelectedItem?.ToString() == "Cash";

            if (isCash)
            {
                if (!decimal.TryParse(txtAmount.Text.Trim(), out decimal paid) || paid <= 0)
                {
                    MessageBox.Show("Please enter a valid amount paid.", "Invalid Input",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (paid < _totalAmount)
                {
                    MessageBox.Show(
                        $"Amount paid (₱{paid:N2}) is less than the total (₱{_totalAmount:N2}).",
                        "Insufficient Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                AmountPaid = paid;
            }
            else
            {
                // Non-cash: amount paid = total exactly
                AmountPaid = _totalAmount;
            }

            this.DialogResult = DialogResult.OK;
            
        }
    }
}