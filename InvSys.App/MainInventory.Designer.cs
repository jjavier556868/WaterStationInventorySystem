namespace InvSys.App
{
    partial class MainInventory
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
            components = new System.ComponentModel.Container();
            Syncfusion.Windows.Forms.Tools.DigitalClockRenderer digitalClockRenderer3 = new Syncfusion.Windows.Forms.Tools.DigitalClockRenderer();
            Syncfusion.Windows.Forms.Tools.ClockRenderer clockRenderer3 = new Syncfusion.Windows.Forms.Tools.ClockRenderer();
            panel1 = new Panel();
            gradientPanel1 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            panel4 = new Panel();
            btnLogout = new Button();
            panel3 = new Panel();
            label7 = new Label();
            btnStock = new Button();
            btnDashboard = new Button();
            btnAccounts = new Button();
            btnSupplier = new Button();
            btnSales = new Button();
            btnProducts = new Button();
            lblWelcome = new Label();
            PanelControl = new TabControl();
            tabPage1 = new TabPage();
            panel2 = new Panel();
            tabPage2 = new TabPage();
            label2 = new Label();
            tabPage3 = new TabPage();
            SupplierTable = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            panel7 = new Panel();
            txtBoxSupplierSearch = new TextBox();
            label3 = new Label();
            btnDeleteSupplier = new Button();
            btnUpdateSupplier = new Button();
            btnAddSupplier = new Button();
            tabPage4 = new TabPage();
            ProductTable = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            panel9 = new Panel();
            panel10 = new Panel();
            textBox2 = new TextBox();
            label4 = new Label();
            btnDeleteProduct = new Button();
            btnUpdateProduct = new Button();
            btnAddProduct = new Button();
            tabPage5 = new TabPage();
            panel6 = new Panel();
            button2 = new Button();
            button1 = new Button();
            sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            tabPage6 = new TabPage();
            label6 = new Label();
            panel5 = new Panel();
            gradientPanel2 = new Syncfusion.Windows.Forms.Tools.GradientPanel();
            clock1 = new Syncfusion.Windows.Forms.Tools.Clock();
            gridLayout1 = new Syncfusion.Windows.Forms.Tools.GridLayout(components);
            panel8 = new Panel();
            panel11 = new Panel();
            panel12 = new Panel();
            panel13 = new Panel();
            panel14 = new Panel();
            panel15 = new Panel();
            panel16 = new Panel();
            panel17 = new Panel();
            label1 = new Label();
            label5 = new Label();
            label8 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gradientPanel1).BeginInit();
            gradientPanel1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            PanelControl.SuspendLayout();
            tabPage1.SuspendLayout();
            panel2.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SupplierTable).BeginInit();
            panel7.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ProductTable).BeginInit();
            panel9.SuspendLayout();
            panel10.SuspendLayout();
            tabPage5.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)sfDataGrid1).BeginInit();
            tabPage6.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gradientPanel2).BeginInit();
            gradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridLayout1).BeginInit();
            panel13.SuspendLayout();
            panel14.SuspendLayout();
            panel16.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(gradientPanel1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(223, 755);
            panel1.TabIndex = 1;
            // 
            // gradientPanel1
            // 
            gradientPanel1.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Vertical, Color.Navy, Color.DodgerBlue);
            gradientPanel1.BorderStyle = BorderStyle.None;
            gradientPanel1.Controls.Add(panel4);
            gradientPanel1.Dock = DockStyle.Fill;
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(223, 755);
            gradientPanel1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(21, 45, 123);
            panel4.Controls.Add(btnLogout);
            panel4.Controls.Add(panel3);
            panel4.Controls.Add(btnStock);
            panel4.Controls.Add(btnDashboard);
            panel4.Controls.Add(btnAccounts);
            panel4.Controls.Add(btnSupplier);
            panel4.Controls.Add(btnSales);
            panel4.Controls.Add(btnProducts);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(223, 755);
            panel4.TabIndex = 2;
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Bottom;
            btnLogout.BackColor = Color.FromArgb(249, 51, 56);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 14.25F);
            btnLogout.ForeColor = SystemColors.ButtonFace;
            btnLogout.Image = Properties.Resources.logout_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnLogout.Location = new Point(-2, 681);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(226, 51);
            btnLogout.TabIndex = 8;
            btnLogout.Text = "Log Out";
            btnLogout.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(panel8);
            panel3.Controls.Add(label7);
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(224, 91);
            panel3.TabIndex = 14;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = SystemColors.Control;
            label7.Location = new Point(37, 30);
            label7.Name = "label7";
            label7.Size = new Size(180, 37);
            label7.TabIndex = 0;
            label7.Text = "H2Organizer";
            // 
            // btnStock
            // 
            btnStock.BackColor = Color.FromArgb(49, 52, 113);
            btnStock.FlatAppearance.BorderColor = Color.White;
            btnStock.FlatAppearance.BorderSize = 0;
            btnStock.FlatStyle = FlatStyle.Flat;
            btnStock.Font = new Font("Yu Gothic UI Semibold", 15.75F, FontStyle.Bold);
            btnStock.ForeColor = SystemColors.ButtonFace;
            btnStock.Image = Properties.Resources.inventory_2_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnStock.Location = new Point(0, 155);
            btnStock.Name = "btnStock";
            btnStock.Size = new Size(226, 52);
            btnStock.TabIndex = 13;
            btnStock.Text = "Stock";
            btnStock.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnStock.UseVisualStyleBackColor = false;
            btnStock.Click += btnStock_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.FromArgb(108, 117, 219);
            btnDashboard.FlatAppearance.BorderColor = Color.White;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Yu Gothic UI Semibold", 15.75F, FontStyle.Bold);
            btnDashboard.ForeColor = SystemColors.ButtonFace;
            btnDashboard.Image = Properties.Resources.dashboard_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnDashboard.Location = new Point(-2, 97);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(226, 52);
            btnDashboard.TabIndex = 2;
            btnDashboard.Text = "Dashboard";
            btnDashboard.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnDashboard.UseVisualStyleBackColor = false;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // btnAccounts
            // 
            btnAccounts.BackColor = Color.FromArgb(49, 52, 113);
            btnAccounts.FlatAppearance.BorderColor = Color.White;
            btnAccounts.FlatAppearance.BorderSize = 0;
            btnAccounts.FlatStyle = FlatStyle.Flat;
            btnAccounts.Font = new Font("Yu Gothic UI Semibold", 15.75F, FontStyle.Bold);
            btnAccounts.ForeColor = SystemColors.ButtonFace;
            btnAccounts.Image = Properties.Resources.account_circle_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnAccounts.Location = new Point(-1, 387);
            btnAccounts.Name = "btnAccounts";
            btnAccounts.Size = new Size(226, 52);
            btnAccounts.TabIndex = 12;
            btnAccounts.Text = "Accounts";
            btnAccounts.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnAccounts.UseVisualStyleBackColor = false;
            btnAccounts.Click += btnAccounts_Click;
            // 
            // btnSupplier
            // 
            btnSupplier.BackColor = Color.FromArgb(49, 52, 113);
            btnSupplier.FlatAppearance.BorderColor = Color.White;
            btnSupplier.FlatAppearance.BorderSize = 0;
            btnSupplier.FlatStyle = FlatStyle.Flat;
            btnSupplier.Font = new Font("Yu Gothic UI Semibold", 15.75F, FontStyle.Bold);
            btnSupplier.ForeColor = SystemColors.ButtonFace;
            btnSupplier.Image = Properties.Resources.person_book_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnSupplier.Location = new Point(-3, 213);
            btnSupplier.Name = "btnSupplier";
            btnSupplier.Size = new Size(226, 52);
            btnSupplier.TabIndex = 9;
            btnSupplier.Text = "Supplier";
            btnSupplier.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnSupplier.UseVisualStyleBackColor = false;
            btnSupplier.Click += btnSupplier_Click;
            // 
            // btnSales
            // 
            btnSales.BackColor = Color.FromArgb(49, 52, 113);
            btnSales.FlatAppearance.BorderColor = Color.White;
            btnSales.FlatAppearance.BorderSize = 0;
            btnSales.FlatStyle = FlatStyle.Flat;
            btnSales.Font = new Font("Yu Gothic UI Semibold", 15.75F, FontStyle.Bold);
            btnSales.ForeColor = SystemColors.ButtonFace;
            btnSales.Image = Properties.Resources.grouped_bar_chart_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnSales.Location = new Point(0, 329);
            btnSales.Name = "btnSales";
            btnSales.Size = new Size(225, 52);
            btnSales.TabIndex = 11;
            btnSales.Text = "Sales";
            btnSales.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnSales.UseVisualStyleBackColor = false;
            btnSales.Click += btnSales_Click;
            // 
            // btnProducts
            // 
            btnProducts.BackColor = Color.FromArgb(49, 52, 113);
            btnProducts.FlatAppearance.BorderColor = Color.White;
            btnProducts.FlatAppearance.BorderSize = 0;
            btnProducts.FlatStyle = FlatStyle.Flat;
            btnProducts.Font = new Font("Yu Gothic UI Semibold", 15.75F, FontStyle.Bold);
            btnProducts.ForeColor = SystemColors.ButtonFace;
            btnProducts.Image = Properties.Resources.category_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnProducts.Location = new Point(0, 271);
            btnProducts.Name = "btnProducts";
            btnProducts.Size = new Size(226, 52);
            btnProducts.TabIndex = 10;
            btnProducts.Text = "Products";
            btnProducts.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnProducts.UseVisualStyleBackColor = false;
            btnProducts.Click += btnProducts_Click;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.BackColor = Color.Transparent;
            lblWelcome.Font = new Font("Segoe UI Light", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Location = new Point(51, 26);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(208, 40);
            lblWelcome.TabIndex = 15;
            lblWelcome.Text = "Welcome, User!";
            // 
            // PanelControl
            // 
            PanelControl.Alignment = TabAlignment.Right;
            PanelControl.CausesValidation = false;
            PanelControl.Controls.Add(tabPage1);
            PanelControl.Controls.Add(tabPage2);
            PanelControl.Controls.Add(tabPage3);
            PanelControl.Controls.Add(tabPage4);
            PanelControl.Controls.Add(tabPage5);
            PanelControl.Controls.Add(tabPage6);
            PanelControl.Dock = DockStyle.Fill;
            PanelControl.ItemSize = new Size(50, 50);
            PanelControl.Location = new Point(223, 93);
            PanelControl.Margin = new Padding(0);
            PanelControl.Multiline = true;
            PanelControl.Name = "PanelControl";
            PanelControl.SelectedIndex = 0;
            PanelControl.Size = new Size(985, 662);
            PanelControl.SizeMode = TabSizeMode.Fixed;
            PanelControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel2);
            tabPage1.Location = new Point(4, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(927, 654);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.GradientActiveCaption;
            panel2.Controls.Add(panel16);
            panel2.Controls.Add(panel17);
            panel2.Controls.Add(panel14);
            panel2.Controls.Add(panel15);
            panel2.Controls.Add(panel13);
            panel2.Controls.Add(panel12);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(921, 648);
            panel2.TabIndex = 2;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new Point(4, 4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(927, 654);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(356, 302);
            label2.Name = "label2";
            label2.Size = new Size(68, 25);
            label2.TabIndex = 2;
            label2.Text = "STOCK";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(SupplierTable);
            tabPage3.Controls.Add(panel7);
            tabPage3.Location = new Point(4, 4);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(927, 654);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // SupplierTable
            // 
            SupplierTable.AccessibleName = "Table";
            SupplierTable.AllowEditing = false;
            SupplierTable.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            SupplierTable.Dock = DockStyle.Fill;
            SupplierTable.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SupplierTable.Location = new Point(3, 66);
            SupplierTable.Name = "SupplierTable";
            SupplierTable.Size = new Size(921, 585);
            SupplierTable.Style.BorderColor = Color.FromArgb(100, 100, 100);
            SupplierTable.Style.CheckBoxStyle.CheckedBackColor = Color.FromArgb(0, 120, 215);
            SupplierTable.Style.CheckBoxStyle.CheckedBorderColor = Color.FromArgb(0, 120, 215);
            SupplierTable.Style.CheckBoxStyle.IndeterminateBorderColor = Color.FromArgb(0, 120, 215);
            SupplierTable.Style.DragPreviewRowStyle.Font = new Font("Segoe UI", 9F);
            SupplierTable.Style.DragPreviewRowStyle.RowCountIndicatorBackColor = Color.FromArgb(0, 120, 215);
            SupplierTable.Style.DragPreviewRowStyle.RowCountIndicatorTextColor = Color.FromArgb(255, 255, 255);
            SupplierTable.Style.HyperlinkStyle.DefaultLinkColor = Color.FromArgb(0, 120, 215);
            SupplierTable.TabIndex = 7;
            SupplierTable.Text = "sfDataGrid2";
            // 
            // panel7
            // 
            panel7.Controls.Add(txtBoxSupplierSearch);
            panel7.Controls.Add(label3);
            panel7.Controls.Add(btnDeleteSupplier);
            panel7.Controls.Add(btnUpdateSupplier);
            panel7.Controls.Add(btnAddSupplier);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(921, 63);
            panel7.TabIndex = 0;
            // 
            // txtBoxSupplierSearch
            // 
            txtBoxSupplierSearch.Anchor = AnchorStyles.Left;
            txtBoxSupplierSearch.Location = new Point(80, 22);
            txtBoxSupplierSearch.MaxLength = 0;
            txtBoxSupplierSearch.Name = "txtBoxSupplierSearch";
            txtBoxSupplierSearch.Size = new Size(287, 23);
            txtBoxSupplierSearch.TabIndex = 6;
            txtBoxSupplierSearch.TextChanged += txtBoxSupplierSearch_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Yu Gothic UI", 11.25F);
            label3.Location = new Point(18, 21);
            label3.Name = "label3";
            label3.Size = new Size(56, 20);
            label3.TabIndex = 5;
            label3.Text = "Search:";
            // 
            // btnDeleteSupplier
            // 
            btnDeleteSupplier.Anchor = AnchorStyles.Right;
            btnDeleteSupplier.BackColor = Color.Firebrick;
            btnDeleteSupplier.FlatStyle = FlatStyle.Flat;
            btnDeleteSupplier.Font = new Font("Yu Gothic UI", 11.25F);
            btnDeleteSupplier.ForeColor = SystemColors.ButtonHighlight;
            btnDeleteSupplier.Location = new Point(754, 7);
            btnDeleteSupplier.Name = "btnDeleteSupplier";
            btnDeleteSupplier.Size = new Size(156, 48);
            btnDeleteSupplier.TabIndex = 4;
            btnDeleteSupplier.Text = "Delete Supplier";
            btnDeleteSupplier.UseVisualStyleBackColor = false;
            btnDeleteSupplier.Click += btnDeleteSupplier_Click;
            // 
            // btnUpdateSupplier
            // 
            btnUpdateSupplier.Anchor = AnchorStyles.Right;
            btnUpdateSupplier.BackColor = Color.GreenYellow;
            btnUpdateSupplier.FlatStyle = FlatStyle.Flat;
            btnUpdateSupplier.Font = new Font("Yu Gothic UI", 11.25F);
            btnUpdateSupplier.Location = new Point(592, 7);
            btnUpdateSupplier.Name = "btnUpdateSupplier";
            btnUpdateSupplier.Size = new Size(156, 48);
            btnUpdateSupplier.TabIndex = 3;
            btnUpdateSupplier.Text = "Update Supplier";
            btnUpdateSupplier.UseVisualStyleBackColor = false;
            btnUpdateSupplier.Click += btnUpdateSupplier_Click;
            // 
            // btnAddSupplier
            // 
            btnAddSupplier.Anchor = AnchorStyles.Right;
            btnAddSupplier.BackColor = Color.RoyalBlue;
            btnAddSupplier.FlatStyle = FlatStyle.Flat;
            btnAddSupplier.Font = new Font("Yu Gothic UI", 11.25F);
            btnAddSupplier.ForeColor = SystemColors.ButtonHighlight;
            btnAddSupplier.Location = new Point(430, 7);
            btnAddSupplier.Name = "btnAddSupplier";
            btnAddSupplier.Size = new Size(156, 48);
            btnAddSupplier.TabIndex = 2;
            btnAddSupplier.Text = "Add Supplier";
            btnAddSupplier.UseVisualStyleBackColor = false;
            btnAddSupplier.Click += btnAddSupplier_Click;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(ProductTable);
            tabPage4.Controls.Add(panel9);
            tabPage4.Location = new Point(4, 4);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(927, 654);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // ProductTable
            // 
            ProductTable.AccessibleName = "Table";
            ProductTable.Dock = DockStyle.Fill;
            ProductTable.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ProductTable.Location = new Point(3, 66);
            ProductTable.Name = "ProductTable";
            ProductTable.Size = new Size(921, 585);
            ProductTable.Style.BorderColor = Color.FromArgb(100, 100, 100);
            ProductTable.Style.CheckBoxStyle.CheckedBackColor = Color.FromArgb(0, 120, 215);
            ProductTable.Style.CheckBoxStyle.CheckedBorderColor = Color.FromArgb(0, 120, 215);
            ProductTable.Style.CheckBoxStyle.IndeterminateBorderColor = Color.FromArgb(0, 120, 215);
            ProductTable.Style.DragPreviewRowStyle.Font = new Font("Segoe UI", 9F);
            ProductTable.Style.DragPreviewRowStyle.RowCountIndicatorBackColor = Color.FromArgb(0, 120, 215);
            ProductTable.Style.DragPreviewRowStyle.RowCountIndicatorTextColor = Color.FromArgb(255, 255, 255);
            ProductTable.Style.HyperlinkStyle.DefaultLinkColor = Color.FromArgb(0, 120, 215);
            ProductTable.TabIndex = 8;
            ProductTable.Text = "sfDataGrid2";
            // 
            // panel9
            // 
            panel9.Controls.Add(panel10);
            panel9.Controls.Add(btnDeleteProduct);
            panel9.Controls.Add(btnUpdateProduct);
            panel9.Controls.Add(btnAddProduct);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(3, 3);
            panel9.Name = "panel9";
            panel9.Size = new Size(921, 63);
            panel9.TabIndex = 1;
            // 
            // panel10
            // 
            panel10.Anchor = AnchorStyles.Left;
            panel10.Controls.Add(textBox2);
            panel10.Controls.Add(label4);
            panel10.Location = new Point(3, 3);
            panel10.Name = "panel10";
            panel10.Size = new Size(415, 57);
            panel10.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Left;
            textBox2.Location = new Point(78, 18);
            textBox2.MaxLength = 0;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(324, 23);
            textBox2.TabIndex = 6;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Yu Gothic UI", 11.25F);
            label4.Location = new Point(16, 16);
            label4.Name = "label4";
            label4.Size = new Size(56, 20);
            label4.TabIndex = 5;
            label4.Text = "Search:";
            // 
            // btnDeleteProduct
            // 
            btnDeleteProduct.BackColor = Color.Firebrick;
            btnDeleteProduct.FlatStyle = FlatStyle.Flat;
            btnDeleteProduct.Font = new Font("Yu Gothic UI", 11.25F);
            btnDeleteProduct.ForeColor = SystemColors.Control;
            btnDeleteProduct.Location = new Point(748, 7);
            btnDeleteProduct.Name = "btnDeleteProduct";
            btnDeleteProduct.Size = new Size(156, 48);
            btnDeleteProduct.TabIndex = 4;
            btnDeleteProduct.Text = "Delete Product";
            btnDeleteProduct.UseVisualStyleBackColor = false;
            btnDeleteProduct.Click += btnDeleteProduct_Click;
            // 
            // btnUpdateProduct
            // 
            btnUpdateProduct.BackColor = Color.GreenYellow;
            btnUpdateProduct.FlatStyle = FlatStyle.Flat;
            btnUpdateProduct.Font = new Font("Yu Gothic UI", 11.25F);
            btnUpdateProduct.Location = new Point(586, 7);
            btnUpdateProduct.Name = "btnUpdateProduct";
            btnUpdateProduct.Size = new Size(156, 48);
            btnUpdateProduct.TabIndex = 3;
            btnUpdateProduct.Text = "Update Product";
            btnUpdateProduct.UseVisualStyleBackColor = false;
            btnUpdateProduct.Click += btnUpdateProduct_Click;
            // 
            // btnAddProduct
            // 
            btnAddProduct.BackColor = Color.RoyalBlue;
            btnAddProduct.FlatStyle = FlatStyle.Flat;
            btnAddProduct.Font = new Font("Yu Gothic UI", 11.25F);
            btnAddProduct.ForeColor = SystemColors.Control;
            btnAddProduct.Location = new Point(424, 7);
            btnAddProduct.Name = "btnAddProduct";
            btnAddProduct.Size = new Size(156, 48);
            btnAddProduct.TabIndex = 2;
            btnAddProduct.Text = "Add Product";
            btnAddProduct.UseVisualStyleBackColor = false;
            btnAddProduct.Click += btnAddProduct_Click;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(panel6);
            tabPage5.Controls.Add(sfDataGrid1);
            tabPage5.Location = new Point(4, 4);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(927, 654);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "tabPage5";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            panel6.Controls.Add(button2);
            panel6.Controls.Add(button1);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(3, 3);
            panel6.Name = "panel6";
            panel6.Size = new Size(921, 71);
            panel6.TabIndex = 1;
            // 
            // button2
            // 
            button2.Location = new Point(313, 3);
            button2.Name = "button2";
            button2.Size = new Size(156, 65);
            button2.TabIndex = 1;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(151, 3);
            button1.Name = "button1";
            button1.Size = new Size(156, 65);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // sfDataGrid1
            // 
            sfDataGrid1.AccessibleName = "Table";
            sfDataGrid1.Dock = DockStyle.Fill;
            sfDataGrid1.Location = new Point(3, 3);
            sfDataGrid1.Name = "sfDataGrid1";
            sfDataGrid1.Size = new Size(921, 648);
            sfDataGrid1.Style.BorderColor = Color.FromArgb(100, 100, 100);
            sfDataGrid1.Style.CheckBoxStyle.CheckedBackColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.CheckBoxStyle.CheckedBorderColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.CheckBoxStyle.IndeterminateBorderColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.DragPreviewRowStyle.Font = new Font("Segoe UI", 9F);
            sfDataGrid1.Style.DragPreviewRowStyle.RowCountIndicatorBackColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.Style.DragPreviewRowStyle.RowCountIndicatorTextColor = Color.FromArgb(255, 255, 255);
            sfDataGrid1.Style.HyperlinkStyle.DefaultLinkColor = Color.FromArgb(0, 120, 215);
            sfDataGrid1.TabIndex = 0;
            sfDataGrid1.Text = "sfDataGrid1";
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(label6);
            tabPage6.Location = new Point(4, 4);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(927, 654);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "tabPage6";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(344, 292);
            label6.Name = "label6";
            label6.Size = new Size(107, 25);
            label6.TabIndex = 3;
            label6.Text = "ACCOUNTS";
            // 
            // panel5
            // 
            panel5.BackColor = SystemColors.ActiveCaption;
            panel5.Controls.Add(gradientPanel2);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(223, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(985, 93);
            panel5.TabIndex = 8;
            // 
            // gradientPanel2
            // 
            gradientPanel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Horizontal, Color.RoyalBlue, Color.SkyBlue);
            gradientPanel2.Controls.Add(panel11);
            gradientPanel2.Controls.Add(clock1);
            gradientPanel2.Controls.Add(lblWelcome);
            gradientPanel2.Dock = DockStyle.Fill;
            gradientPanel2.Location = new Point(0, 0);
            gradientPanel2.Name = "gradientPanel2";
            gradientPanel2.Size = new Size(985, 93);
            gradientPanel2.TabIndex = 16;
            // 
            // clock1
            // 
            clock1.Anchor = AnchorStyles.Right;
            clock1.BackgroundColor = Color.Transparent;
            clock1.BeforeTouchSize = new Size(162, 81);
            clock1.BorderColor = Color.Transparent;
            clock1.ClockFormat = "HH:mm:ss";
            clock1.ClockFrame = Syncfusion.Windows.Forms.Tools.ClockFrames.RectangularFrame;
            clock1.ClockShape = Syncfusion.Windows.Forms.Tools.ClockShapes.Rectangle;
            clock1.ClockType = Syncfusion.Windows.Forms.Tools.ClockTypes.Digital;
            clock1.CurrentDateTime = new DateTime(2026, 2, 17, 10, 49, 55, 547);
            clock1.CustomTime = new DateTime(2026, 3, 5, 20, 54, 39, 734);
            clock1.DigitalRenderer = digitalClockRenderer3;
            clock1.DisplayDates = true;
            clock1.ForeColor = SystemColors.MenuHighlight;
            clock1.Location = new Point(795, 3);
            clock1.MinimumSize = new Size(75, 75);
            clock1.Name = "clock1";
            clock1.Now = new DateTime(0L);
            clock1.Remainder = new DateTime(2026, 2, 17, 10, 49, 55, 545);
            clock1.Renderer = clockRenderer3;
            clock1.ShowClockFrame = false;
            clock1.ShowCustomTimeClock = false;
            clock1.ShowHourDesignator = false;
            clock1.Size = new Size(162, 81);
            clock1.StopTimer = false;
            clock1.TabIndex = 16;
            clock1.Text = "clock1";
            // 
            // gridLayout1
            // 
            gridLayout1.Columns = 0;
            gridLayout1.Rows = 1;
            // 
            // panel8
            // 
            panel8.BackgroundImage = Properties.Resources.water_drops_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            panel8.BackgroundImageLayout = ImageLayout.Center;
            panel8.Location = new Point(13, 33);
            panel8.Name = "panel8";
            panel8.Size = new Size(29, 32);
            panel8.TabIndex = 1;
            // 
            // panel11
            // 
            panel11.BackColor = Color.Transparent;
            panel11.BackgroundImage = Properties.Resources.account_circle_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            panel11.BackgroundImageLayout = ImageLayout.Center;
            panel11.Location = new Point(11, 36);
            panel11.Name = "panel11";
            panel11.Size = new Size(34, 27);
            panel11.TabIndex = 17;
            // 
            // panel12
            // 
            panel12.BackColor = Color.WhiteSmoke;
            panel12.Location = new Point(39, 42);
            panel12.Name = "panel12";
            panel12.Size = new Size(244, 181);
            panel12.TabIndex = 0;
            // 
            // panel13
            // 
            panel13.BackColor = Color.Navy;
            panel13.Controls.Add(label1);
            panel13.Location = new Point(39, 42);
            panel13.Name = "panel13";
            panel13.Size = new Size(244, 45);
            panel13.TabIndex = 1;
            // 
            // panel14
            // 
            panel14.BackColor = Color.Navy;
            panel14.Controls.Add(label5);
            panel14.Location = new Point(308, 42);
            panel14.Name = "panel14";
            panel14.Size = new Size(244, 45);
            panel14.TabIndex = 3;
            // 
            // panel15
            // 
            panel15.BackColor = Color.WhiteSmoke;
            panel15.Location = new Point(308, 42);
            panel15.Name = "panel15";
            panel15.Size = new Size(244, 181);
            panel15.TabIndex = 2;
            // 
            // panel16
            // 
            panel16.BackColor = Color.Navy;
            panel16.Controls.Add(label8);
            panel16.Location = new Point(570, 42);
            panel16.Name = "panel16";
            panel16.Size = new Size(244, 45);
            panel16.TabIndex = 5;
            // 
            // panel17
            // 
            panel17.BackColor = Color.WhiteSmoke;
            panel17.Location = new Point(570, 42);
            panel17.Name = "panel17";
            panel17.Size = new Size(244, 181);
            panel17.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonFace;
            label1.Location = new Point(13, 13);
            label1.Name = "label1";
            label1.Size = new Size(163, 20);
            label1.TabIndex = 0;
            label1.Text = "MOST SOLD PRODUCT";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ButtonFace;
            label5.Location = new Point(12, 13);
            label5.Name = "label5";
            label5.Size = new Size(85, 20);
            label5.TabIndex = 1;
            label5.Text = "PRODUCTS";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = SystemColors.ButtonFace;
            label8.Location = new Point(15, 13);
            label8.Name = "label8";
            label8.Size = new Size(170, 20);
            label8.TabIndex = 2;
            label8.Text = "TOTAL MONTHLY SALES";
            // 
            // MainInventory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1208, 755);
            Controls.Add(PanelControl);
            Controls.Add(panel5);
            Controls.Add(panel1);
            Name = "MainInventory";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main Inventory";
            WindowState = FormWindowState.Maximized;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gradientPanel1).EndInit();
            gradientPanel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            PanelControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SupplierTable).EndInit();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ProductTable).EndInit();
            panel9.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            tabPage5.ResumeLayout(false);
            panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)sfDataGrid1).EndInit();
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gradientPanel2).EndInit();
            gradientPanel2.ResumeLayout(false);
            gradientPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridLayout1).EndInit();
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panel14.ResumeLayout(false);
            panel14.PerformLayout();
            panel16.ResumeLayout(false);
            panel16.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private TabControl PanelControl;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel1;
        private TabPage tabPage3;
        private Button btnLogout;
        private Button btnDashboard;
        private Button btnSales;
        private Button btnProducts;
        private Button btnSupplier;
        private Button btnAccounts;
        private Button btnStock;
        private Label label2;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private Label label6;
        private Panel panel2;
        private Panel panel4;
        private Panel panel3;
        private Label label7;
        private TextBox txtBoxSupplierSearch;
        private Label label3;
        private Panel panel7;
        private Button btnDeleteSupplier;
        private Button btnUpdateSupplier;
        private Button btnAddSupplier;
        private Panel panel6;
        private Button button2;
        private Button button1;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private Label lblWelcome;
        private Panel panel5;
        private Panel panel9;
        private Panel panel10;
        private TextBox textBox2;
        private Label label4;
        private Button btnDeleteProduct;
        private Button btnUpdateProduct;
        private Button btnAddProduct;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.Clock clock1;
        private Syncfusion.WinForms.DataGrid.SfDataGrid SupplierTable;
        private Syncfusion.WinForms.DataGrid.SfDataGrid ProductTable;
        private Syncfusion.Windows.Forms.Tools.GridLayout gridLayout1;
        private Panel panel8;
        private Panel panel16;
        private Panel panel17;
        private Panel panel14;
        private Panel panel15;
        private Panel panel13;
        private Label label1;
        private Panel panel12;
        private Panel panel11;
        private Label label8;
        private Label label5;
    }
}