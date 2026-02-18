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
            Syncfusion.Windows.Forms.Tools.DigitalClockRenderer digitalClockRenderer2 = new Syncfusion.Windows.Forms.Tools.DigitalClockRenderer();
            Syncfusion.Windows.Forms.Tools.ClockRenderer clockRenderer2 = new Syncfusion.Windows.Forms.Tools.ClockRenderer();
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
            label1 = new Label();
            tabPage2 = new TabPage();
            label2 = new Label();
            tabPage3 = new TabPage();
            SupplierTable = new DataGridView();
            Id = new DataGridViewTextBoxColumn();
            SupplierName = new DataGridViewTextBoxColumn();
            Email = new DataGridViewTextBoxColumn();
            Location = new DataGridViewTextBoxColumn();
            ContactNo = new DataGridViewTextBoxColumn();
            Products = new DataGridViewTextBoxColumn();
            CreatedDate = new DataGridViewTextBoxColumn();
            isActive = new DataGridViewCheckBoxColumn();
            panel7 = new Panel();
            panel8 = new Panel();
            txtBoxSupplierSearch = new TextBox();
            label3 = new Label();
            btnDeleteSupplier = new Button();
            btnUpdateSupplier = new Button();
            btnAddSupplier = new Button();
            tabPage4 = new TabPage();
            ProductTable = new DataGridView();
            IDColumn = new DataGridViewTextBoxColumn();
            NameColumn = new DataGridViewTextBoxColumn();
            PriceColumn = new DataGridViewTextBoxColumn();
            QuantityColumn = new DataGridViewTextBoxColumn();
            NameOfSupplierColumn = new DataGridViewTextBoxColumn();
            CreationDateColumn = new DataGridViewTextBoxColumn();
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
            panel8.SuspendLayout();
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
            panel4.BackColor = Color.Transparent;
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
            btnLogout.Location = new Point(-2, 662);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(226, 70);
            btnLogout.TabIndex = 8;
            btnLogout.Text = "Log Out";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // panel3
            // 
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
            label7.Location = new Point(21, 29);
            label7.Name = "label7";
            label7.Size = new Size(180, 37);
            label7.TabIndex = 0;
            label7.Text = "H2Organizer";
            // 
            // btnStock
            // 
            btnStock.BackColor = Color.PaleTurquoise;
            btnStock.FlatAppearance.BorderColor = Color.White;
            btnStock.FlatAppearance.BorderSize = 0;
            btnStock.FlatStyle = FlatStyle.Flat;
            btnStock.Font = new Font("Yu Gothic UI", 14.25F);
            btnStock.Location = new Point(0, 178);
            btnStock.Name = "btnStock";
            btnStock.Size = new Size(226, 79);
            btnStock.TabIndex = 13;
            btnStock.Text = "Stock Tracker";
            btnStock.UseVisualStyleBackColor = false;
            btnStock.Click += btnStock_Click;
            // 
            // btnDashboard
            // 
            btnDashboard.BackColor = Color.PaleTurquoise;
            btnDashboard.FlatAppearance.BorderColor = Color.White;
            btnDashboard.FlatAppearance.BorderSize = 0;
            btnDashboard.FlatStyle = FlatStyle.Flat;
            btnDashboard.Font = new Font("Yu Gothic UI", 14.25F);
            btnDashboard.Location = new Point(-2, 93);
            btnDashboard.Name = "btnDashboard";
            btnDashboard.Size = new Size(226, 79);
            btnDashboard.TabIndex = 2;
            btnDashboard.Text = "Dashboard";
            btnDashboard.UseVisualStyleBackColor = false;
            btnDashboard.Click += btnDashboard_Click;
            // 
            // btnAccounts
            // 
            btnAccounts.BackColor = Color.PaleTurquoise;
            btnAccounts.FlatAppearance.BorderColor = Color.White;
            btnAccounts.FlatAppearance.BorderSize = 0;
            btnAccounts.FlatStyle = FlatStyle.Flat;
            btnAccounts.Font = new Font("Yu Gothic UI", 14.25F);
            btnAccounts.Location = new Point(-2, 518);
            btnAccounts.Name = "btnAccounts";
            btnAccounts.Size = new Size(226, 79);
            btnAccounts.TabIndex = 12;
            btnAccounts.Text = "Accounts";
            btnAccounts.UseVisualStyleBackColor = false;
            btnAccounts.Click += btnAccounts_Click;
            // 
            // btnSupplier
            // 
            btnSupplier.BackColor = Color.PaleTurquoise;
            btnSupplier.FlatAppearance.BorderColor = Color.White;
            btnSupplier.FlatAppearance.BorderSize = 0;
            btnSupplier.FlatStyle = FlatStyle.Flat;
            btnSupplier.Font = new Font("Yu Gothic UI", 14.25F);
            btnSupplier.Location = new Point(-2, 263);
            btnSupplier.Name = "btnSupplier";
            btnSupplier.Size = new Size(226, 79);
            btnSupplier.TabIndex = 9;
            btnSupplier.Text = "Supplier";
            btnSupplier.UseVisualStyleBackColor = false;
            btnSupplier.Click += btnSupplier_Click;
            // 
            // btnSales
            // 
            btnSales.BackColor = Color.PaleTurquoise;
            btnSales.FlatAppearance.BorderColor = Color.White;
            btnSales.FlatAppearance.BorderSize = 0;
            btnSales.FlatStyle = FlatStyle.Flat;
            btnSales.Font = new Font("Yu Gothic UI", 14.25F);
            btnSales.Location = new Point(-1, 433);
            btnSales.Name = "btnSales";
            btnSales.Size = new Size(225, 79);
            btnSales.TabIndex = 11;
            btnSales.Text = "Sales";
            btnSales.UseVisualStyleBackColor = false;
            btnSales.Click += btnSales_Click;
            // 
            // btnProducts
            // 
            btnProducts.BackColor = Color.PaleTurquoise;
            btnProducts.FlatAppearance.BorderColor = Color.White;
            btnProducts.FlatAppearance.BorderSize = 0;
            btnProducts.FlatStyle = FlatStyle.Flat;
            btnProducts.Font = new Font("Yu Gothic UI", 14.25F);
            btnProducts.Location = new Point(-2, 348);
            btnProducts.Name = "btnProducts";
            btnProducts.Size = new Size(226, 79);
            btnProducts.TabIndex = 10;
            btnProducts.Text = "Products";
            btnProducts.UseVisualStyleBackColor = false;
            btnProducts.Click += btnProducts_Click;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.BackColor = Color.Transparent;
            lblWelcome.Font = new Font("Segoe UI Light", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.Location = new Point(19, 25);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(208, 40);
            lblWelcome.TabIndex = 15;
            lblWelcome.Text = "Welcome, User!";
            lblWelcome.Click += userNameLabel_Click;
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
            PanelControl.ItemSize = new Size(0, 1);
            PanelControl.Location = new Point(223, 93);
            PanelControl.Margin = new Padding(0);
            PanelControl.Multiline = true;
            PanelControl.Name = "PanelControl";
            PanelControl.SelectedIndex = 0;
            PanelControl.Size = new Size(977, 662);
            PanelControl.SizeMode = TabSizeMode.Fixed;
            PanelControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel2);
            tabPage1.Location = new Point(4, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(919, 654);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            tabPage1.Click += tabPage1_Click;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.GradientActiveCaption;
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(913, 648);
            panel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 48F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(257, 193);
            label1.Name = "label1";
            label1.Size = new Size(424, 86);
            label1.TabIndex = 1;
            label1.Text = "Coming Soon";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new Point(4, 4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(919, 654);
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
            tabPage3.Size = new Size(919, 654);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // SupplierTable
            // 
            SupplierTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            SupplierTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            SupplierTable.Columns.AddRange(new DataGridViewColumn[] { Id, SupplierName, Email, Location, ContactNo, Products, CreatedDate, isActive });
            SupplierTable.Dock = DockStyle.Fill;
            SupplierTable.Location = new Point(3, 66);
            SupplierTable.Name = "SupplierTable";
            SupplierTable.ReadOnly = true;
            SupplierTable.Size = new Size(913, 585);
            SupplierTable.TabIndex = 1;
            SupplierTable.CellDoubleClick += SupplierTable_CellDoubleClick;
            // 
            // Id
            // 
            Id.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Id.DataPropertyName = "Id";
            Id.HeaderText = "ID";
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Width = 108;
            // 
            // SupplierName
            // 
            SupplierName.DataPropertyName = "Name";
            SupplierName.HeaderText = "Name";
            SupplierName.Name = "SupplierName";
            SupplierName.ReadOnly = true;
            SupplierName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Email
            // 
            Email.DataPropertyName = "Email";
            Email.HeaderText = "Email";
            Email.Name = "Email";
            Email.ReadOnly = true;
            // 
            // Location
            // 
            Location.DataPropertyName = "Location";
            Location.HeaderText = "Location";
            Location.Name = "Location";
            Location.ReadOnly = true;
            // 
            // ContactNo
            // 
            ContactNo.DataPropertyName = "ContactNo";
            ContactNo.HeaderText = "Contact No.";
            ContactNo.Name = "ContactNo";
            ContactNo.ReadOnly = true;
            // 
            // Products
            // 
            Products.DataPropertyName = "Products";
            Products.HeaderText = "Products";
            Products.Name = "Products";
            Products.ReadOnly = true;
            Products.Visible = false;
            // 
            // CreatedDate
            // 
            CreatedDate.DataPropertyName = "CreatedDate";
            CreatedDate.HeaderText = "Created Date";
            CreatedDate.Name = "CreatedDate";
            CreatedDate.ReadOnly = true;
            // 
            // isActive
            // 
            isActive.DataPropertyName = "isActive";
            isActive.HeaderText = "Active Status";
            isActive.Name = "isActive";
            isActive.ReadOnly = true;
            isActive.Resizable = DataGridViewTriState.True;
            isActive.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // panel7
            // 
            panel7.Controls.Add(panel8);
            panel7.Controls.Add(btnDeleteSupplier);
            panel7.Controls.Add(btnUpdateSupplier);
            panel7.Controls.Add(btnAddSupplier);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(913, 63);
            panel7.TabIndex = 0;
            // 
            // panel8
            // 
            panel8.Anchor = AnchorStyles.Left;
            panel8.Controls.Add(txtBoxSupplierSearch);
            panel8.Controls.Add(label3);
            panel8.Location = new Point(494, -9);
            panel8.Name = "panel8";
            panel8.Size = new Size(469, 80);
            panel8.TabIndex = 7;
            // 
            // txtBoxSupplierSearch
            // 
            txtBoxSupplierSearch.Anchor = AnchorStyles.Left;
            txtBoxSupplierSearch.Location = new Point(78, 33);
            txtBoxSupplierSearch.MaxLength = 0;
            txtBoxSupplierSearch.Name = "txtBoxSupplierSearch";
            txtBoxSupplierSearch.Size = new Size(383, 23);
            txtBoxSupplierSearch.TabIndex = 6;
            txtBoxSupplierSearch.TextChanged += txtBoxSupplierSearch_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Yu Gothic UI", 11.25F);
            label3.Location = new Point(16, 31);
            label3.Name = "label3";
            label3.Size = new Size(56, 20);
            label3.TabIndex = 5;
            label3.Text = "Search:";
            // 
            // btnDeleteSupplier
            // 
            btnDeleteSupplier.BackColor = Color.Firebrick;
            btnDeleteSupplier.FlatStyle = FlatStyle.Flat;
            btnDeleteSupplier.Font = new Font("Yu Gothic UI", 11.25F);
            btnDeleteSupplier.ForeColor = SystemColors.ButtonHighlight;
            btnDeleteSupplier.Location = new Point(327, 9);
            btnDeleteSupplier.Name = "btnDeleteSupplier";
            btnDeleteSupplier.Size = new Size(156, 48);
            btnDeleteSupplier.TabIndex = 4;
            btnDeleteSupplier.Text = "Delete Supplier";
            btnDeleteSupplier.UseVisualStyleBackColor = false;
            btnDeleteSupplier.Click += btnDeleteSupplier_Click;
            // 
            // btnUpdateSupplier
            // 
            btnUpdateSupplier.BackColor = Color.GreenYellow;
            btnUpdateSupplier.FlatStyle = FlatStyle.Flat;
            btnUpdateSupplier.Font = new Font("Yu Gothic UI", 11.25F);
            btnUpdateSupplier.Location = new Point(165, 9);
            btnUpdateSupplier.Name = "btnUpdateSupplier";
            btnUpdateSupplier.Size = new Size(156, 48);
            btnUpdateSupplier.TabIndex = 3;
            btnUpdateSupplier.Text = "Update Supplier";
            btnUpdateSupplier.UseVisualStyleBackColor = false;
            btnUpdateSupplier.Click += btnUpdateSupplier_Click;
            // 
            // btnAddSupplier
            // 
            btnAddSupplier.BackColor = Color.RoyalBlue;
            btnAddSupplier.FlatStyle = FlatStyle.Flat;
            btnAddSupplier.Font = new Font("Yu Gothic UI", 11.25F);
            btnAddSupplier.ForeColor = SystemColors.ButtonHighlight;
            btnAddSupplier.Location = new Point(3, 9);
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
            tabPage4.Size = new Size(919, 654);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // ProductTable
            // 
            ProductTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ProductTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ProductTable.Columns.AddRange(new DataGridViewColumn[] { IDColumn, NameColumn, PriceColumn, QuantityColumn, NameOfSupplierColumn, CreationDateColumn });
            ProductTable.Dock = DockStyle.Fill;
            ProductTable.Location = new Point(3, 66);
            ProductTable.Name = "ProductTable";
            ProductTable.ReadOnly = true;
            ProductTable.Size = new Size(913, 585);
            ProductTable.TabIndex = 2;
            ProductTable.CellDoubleClick += ProductTable_CellDoubleClick;
            // 
            // IDColumn
            // 
            IDColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            IDColumn.DataPropertyName = "Id";
            IDColumn.HeaderText = "ID";
            IDColumn.Name = "IDColumn";
            IDColumn.ReadOnly = true;
            IDColumn.Width = 108;
            // 
            // NameColumn
            // 
            NameColumn.DataPropertyName = "Name";
            NameColumn.HeaderText = "Name";
            NameColumn.Name = "NameColumn";
            NameColumn.ReadOnly = true;
            // 
            // PriceColumn
            // 
            PriceColumn.DataPropertyName = "Price";
            PriceColumn.HeaderText = "Price";
            PriceColumn.Name = "PriceColumn";
            PriceColumn.ReadOnly = true;
            // 
            // QuantityColumn
            // 
            QuantityColumn.DataPropertyName = "QuantityInStock";
            QuantityColumn.HeaderText = "Quantity";
            QuantityColumn.Name = "QuantityColumn";
            QuantityColumn.ReadOnly = true;
            // 
            // NameOfSupplierColumn
            // 
            NameOfSupplierColumn.DataPropertyName = "SupplierName";
            NameOfSupplierColumn.HeaderText = "Name of Supplier";
            NameOfSupplierColumn.Name = "NameOfSupplierColumn";
            NameOfSupplierColumn.ReadOnly = true;
            // 
            // CreationDateColumn
            // 
            CreationDateColumn.DataPropertyName = "CreatedDate";
            CreationDateColumn.HeaderText = "Date of Creation";
            CreationDateColumn.Name = "CreationDateColumn";
            CreationDateColumn.ReadOnly = true;
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
            panel9.Size = new Size(913, 63);
            panel9.TabIndex = 1;
            // 
            // panel10
            // 
            panel10.Anchor = AnchorStyles.Left;
            panel10.Controls.Add(textBox2);
            panel10.Controls.Add(label4);
            panel10.Location = new Point(494, 3);
            panel10.Name = "panel10";
            panel10.Size = new Size(469, 57);
            panel10.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Left;
            textBox2.Location = new Point(78, 21);
            textBox2.MaxLength = 0;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(383, 23);
            textBox2.TabIndex = 6;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Yu Gothic UI", 11.25F);
            label4.Location = new Point(16, 19);
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
            btnDeleteProduct.Location = new Point(327, 9);
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
            btnUpdateProduct.Location = new Point(165, 9);
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
            btnAddProduct.Location = new Point(3, 9);
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
            tabPage5.Size = new Size(919, 654);
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
            panel6.Size = new Size(913, 71);
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
            sfDataGrid1.Size = new Size(913, 648);
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
            tabPage6.Size = new Size(968, 654);
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
            panel5.Size = new Size(977, 93);
            panel5.TabIndex = 8;
            // 
            // gradientPanel2
            // 
            gradientPanel2.BackgroundColor = new Syncfusion.Drawing.BrushInfo(Syncfusion.Drawing.GradientStyle.Horizontal, Color.RoyalBlue, Color.SkyBlue);
            gradientPanel2.Controls.Add(clock1);
            gradientPanel2.Controls.Add(lblWelcome);
            gradientPanel2.Dock = DockStyle.Fill;
            gradientPanel2.Location = new Point(0, 0);
            gradientPanel2.Name = "gradientPanel2";
            gradientPanel2.Size = new Size(977, 93);
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
            clock1.CustomTime = new DateTime(2026, 2, 17, 23, 1, 6, 793);
            clock1.DigitalRenderer = digitalClockRenderer2;
            clock1.DisplayDates = true;
            clock1.ForeColor = SystemColors.MenuHighlight;
            clock1.Location = new Point(787, 3);
            clock1.MinimumSize = new Size(75, 75);
            clock1.Name = "clock1";
            clock1.Now = new DateTime(0L);
            clock1.Remainder = new DateTime(2026, 2, 17, 10, 49, 55, 545);
            clock1.Renderer = clockRenderer2;
            clock1.ShowClockFrame = false;
            clock1.ShowCustomTimeClock = false;
            clock1.ShowHourDesignator = false;
            clock1.Size = new Size(162, 81);
            clock1.StopTimer = false;
            clock1.TabIndex = 16;
            clock1.Text = "clock1";
            // 
            // MainInventory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 755);
            Controls.Add(PanelControl);
            Controls.Add(panel5);
            Controls.Add(panel1);
            Name = "MainInventory";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main Inventory";
            WindowState = FormWindowState.Maximized;
            Load += MainInventory_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gradientPanel1).EndInit();
            gradientPanel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            PanelControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)SupplierTable).EndInit();
            panel7.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
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
        private Label label1;
        private Panel panel4;
        private Panel panel3;
        private Label label7;
        private Panel panel8;
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
        private DataGridView SupplierTable;
        private DataGridView ProductTable;
        private Panel panel9;
        private Panel panel10;
        private TextBox textBox2;
        private Label label4;
        private Button btnDeleteProduct;
        private Button btnUpdateProduct;
        private Button btnAddProduct;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn SupplierName;
        private DataGridViewTextBoxColumn Email;
        private DataGridViewTextBoxColumn Location;
        private DataGridViewTextBoxColumn ContactNo;
        private DataGridViewTextBoxColumn Products;
        private DataGridViewTextBoxColumn CreatedDate;
        private DataGridViewCheckBoxColumn isActive;
        private Syncfusion.Windows.Forms.Tools.GradientPanel gradientPanel2;
        private Syncfusion.Windows.Forms.Tools.Clock clock1;
        private DataGridViewTextBoxColumn IDColumn;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn PriceColumn;
        private DataGridViewTextBoxColumn QuantityColumn;
        private DataGridViewTextBoxColumn NameOfSupplierColumn;
        private DataGridViewTextBoxColumn CreationDateColumn;
    }
}