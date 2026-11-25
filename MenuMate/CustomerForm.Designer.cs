namespace MenuMate
{
    partial class CustomerForm
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
            pnlHeader = new Panel();
            btnMyOrders = new Button();
            btnLogOut = new Button();
            lblGreet = new Label();
            lblSeparate = new Label();
            lblMenuMate = new Label();
            pnlCategories = new Panel();
            btnDesserts = new Button();
            btnFood = new Button();
            btnDrinks = new Button();
            flwPnlMenu = new FlowLayoutPanel();
            lblYourOrder = new Label();
            flwPnlYourOrder = new FlowLayoutPanel();
            pnlYourOrder = new Panel();
            lblNoItems = new Label();
            btnCheckout = new Button();
            rbTakeaway = new RadioButton();
            rbDineIn = new RadioButton();
            lblTotalVal = new Label();
            lblTotal = new Label();
            lblGSTVal = new Label();
            lblGST = new Label();
            lblSubtotalVal = new Label();
            lblSubtotal = new Label();
            pnlHeader.SuspendLayout();
            pnlCategories.SuspendLayout();
            pnlYourOrder.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(47, 107, 79);
            pnlHeader.Controls.Add(btnMyOrders);
            pnlHeader.Controls.Add(btnLogOut);
            pnlHeader.Controls.Add(lblGreet);
            pnlHeader.Controls.Add(lblSeparate);
            pnlHeader.Controls.Add(lblMenuMate);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(982, 50);
            pnlHeader.TabIndex = 0;
            // 
            // btnMyOrders
            // 
            btnMyOrders.FlatStyle = FlatStyle.Flat;
            btnMyOrders.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnMyOrders.ForeColor = SystemColors.Control;
            btnMyOrders.Location = new Point(776, 11);
            btnMyOrders.Name = "btnMyOrders";
            btnMyOrders.Size = new Size(94, 29);
            btnMyOrders.TabIndex = 4;
            btnMyOrders.Text = "My Orders";
            btnMyOrders.UseVisualStyleBackColor = true;
            btnMyOrders.Click += btnMyOrders_Click;
            // 
            // btnLogOut
            // 
            btnLogOut.FlatStyle = FlatStyle.Flat;
            btnLogOut.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogOut.ForeColor = SystemColors.Control;
            btnLogOut.Location = new Point(876, 11);
            btnLogOut.Name = "btnLogOut";
            btnLogOut.Size = new Size(94, 29);
            btnLogOut.TabIndex = 3;
            btnLogOut.Text = "Log Out";
            btnLogOut.UseVisualStyleBackColor = true;
            btnLogOut.Click += btnLogOut_Click;
            // 
            // lblGreet
            // 
            lblGreet.AutoSize = true;
            lblGreet.Font = new Font("Segoe UI Semilight", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGreet.ForeColor = SystemColors.Control;
            lblGreet.Location = new Point(115, 13);
            lblGreet.Name = "lblGreet";
            lblGreet.Size = new Size(101, 23);
            lblGreet.TabIndex = 2;
            lblGreet.Text = "Hello, [User]";
            lblGreet.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSeparate
            // 
            lblSeparate.AutoSize = true;
            lblSeparate.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSeparate.ForeColor = SystemColors.Control;
            lblSeparate.Location = new Point(103, 13);
            lblSeparate.Name = "lblSeparate";
            lblSeparate.Size = new Size(16, 23);
            lblSeparate.TabIndex = 1;
            lblSeparate.Text = "|";
            // 
            // lblMenuMate
            // 
            lblMenuMate.AutoSize = true;
            lblMenuMate.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMenuMate.ForeColor = SystemColors.Control;
            lblMenuMate.Location = new Point(12, 13);
            lblMenuMate.Name = "lblMenuMate";
            lblMenuMate.Size = new Size(96, 23);
            lblMenuMate.TabIndex = 0;
            lblMenuMate.Text = "MenuMate";
            lblMenuMate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlCategories
            // 
            pnlCategories.Controls.Add(btnDesserts);
            pnlCategories.Controls.Add(btnFood);
            pnlCategories.Controls.Add(btnDrinks);
            pnlCategories.Dock = DockStyle.Left;
            pnlCategories.Location = new Point(0, 50);
            pnlCategories.Name = "pnlCategories";
            pnlCategories.Size = new Size(300, 553);
            pnlCategories.TabIndex = 3;
            // 
            // btnDesserts
            // 
            btnDesserts.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDesserts.Location = new Point(68, 376);
            btnDesserts.Name = "btnDesserts";
            btnDesserts.Size = new Size(159, 120);
            btnDesserts.TabIndex = 2;
            btnDesserts.Text = "Desserts";
            btnDesserts.UseVisualStyleBackColor = true;
            btnDesserts.Click += btnDesserts_Click;
            // 
            // btnFood
            // 
            btnFood.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFood.Location = new Point(68, 208);
            btnFood.Name = "btnFood";
            btnFood.Size = new Size(159, 120);
            btnFood.TabIndex = 1;
            btnFood.Text = "Food";
            btnFood.UseVisualStyleBackColor = true;
            btnFood.Click += btnFood_Click;
            // 
            // btnDrinks
            // 
            btnDrinks.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDrinks.Location = new Point(68, 49);
            btnDrinks.Name = "btnDrinks";
            btnDrinks.Size = new Size(159, 120);
            btnDrinks.TabIndex = 0;
            btnDrinks.Text = "Drinks";
            btnDrinks.UseVisualStyleBackColor = true;
            btnDrinks.Click += btnDrinks_Click;
            // 
            // flwPnlMenu
            // 
            flwPnlMenu.AutoScroll = true;
            flwPnlMenu.Dock = DockStyle.Fill;
            flwPnlMenu.Font = new Font("Microsoft Sans Serif", 8.25F);
            flwPnlMenu.Location = new Point(300, 50);
            flwPnlMenu.Name = "flwPnlMenu";
            flwPnlMenu.Size = new Size(382, 553);
            flwPnlMenu.TabIndex = 4;
            // 
            // lblYourOrder
            // 
            lblYourOrder.Dock = DockStyle.Top;
            lblYourOrder.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblYourOrder.Location = new Point(0, 0);
            lblYourOrder.Name = "lblYourOrder";
            lblYourOrder.Size = new Size(300, 39);
            lblYourOrder.TabIndex = 0;
            lblYourOrder.Text = "Your Order";
            lblYourOrder.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flwPnlYourOrder
            // 
            flwPnlYourOrder.AutoScroll = true;
            flwPnlYourOrder.Location = new Point(0, 61);
            flwPnlYourOrder.Name = "flwPnlYourOrder";
            flwPnlYourOrder.Size = new Size(300, 393);
            flwPnlYourOrder.TabIndex = 1;
            // 
            // pnlYourOrder
            // 
            pnlYourOrder.Controls.Add(lblNoItems);
            pnlYourOrder.Controls.Add(btnCheckout);
            pnlYourOrder.Controls.Add(rbTakeaway);
            pnlYourOrder.Controls.Add(rbDineIn);
            pnlYourOrder.Controls.Add(lblTotalVal);
            pnlYourOrder.Controls.Add(lblTotal);
            pnlYourOrder.Controls.Add(lblGSTVal);
            pnlYourOrder.Controls.Add(lblGST);
            pnlYourOrder.Controls.Add(lblSubtotalVal);
            pnlYourOrder.Controls.Add(lblSubtotal);
            pnlYourOrder.Controls.Add(flwPnlYourOrder);
            pnlYourOrder.Controls.Add(lblYourOrder);
            pnlYourOrder.Dock = DockStyle.Right;
            pnlYourOrder.Location = new Point(682, 50);
            pnlYourOrder.Name = "pnlYourOrder";
            pnlYourOrder.Size = new Size(300, 553);
            pnlYourOrder.TabIndex = 2;
            // 
            // lblNoItems
            // 
            lblNoItems.BackColor = Color.FromArgb(47, 107, 79);
            lblNoItems.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNoItems.ForeColor = Color.Red;
            lblNoItems.Location = new Point(0, 39);
            lblNoItems.Name = "lblNoItems";
            lblNoItems.Size = new Size(300, 24);
            lblNoItems.TabIndex = 11;
            lblNoItems.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnCheckout
            // 
            btnCheckout.BackColor = Color.FromArgb(232, 131, 58);
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCheckout.ForeColor = SystemColors.Control;
            btnCheckout.Location = new Point(186, 517);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(102, 29);
            btnCheckout.TabIndex = 10;
            btnCheckout.Text = "Checkout";
            btnCheckout.UseVisualStyleBackColor = false;
            btnCheckout.Click += btnCheckout_Click;
            // 
            // rbTakeaway
            // 
            rbTakeaway.AutoSize = true;
            rbTakeaway.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbTakeaway.Location = new Point(186, 489);
            rbTakeaway.Name = "rbTakeaway";
            rbTakeaway.Size = new Size(102, 27);
            rbTakeaway.TabIndex = 9;
            rbTakeaway.Text = "Takeaway";
            rbTakeaway.UseVisualStyleBackColor = true;
            // 
            // rbDineIn
            // 
            rbDineIn.AutoSize = true;
            rbDineIn.Checked = true;
            rbDineIn.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rbDineIn.Location = new Point(186, 466);
            rbDineIn.Name = "rbDineIn";
            rbDineIn.Size = new Size(86, 27);
            rbDineIn.TabIndex = 8;
            rbDineIn.TabStop = true;
            rbDineIn.Text = "Dine In";
            rbDineIn.UseVisualStyleBackColor = true;
            // 
            // lblTotalVal
            // 
            lblTotalVal.AutoSize = true;
            lblTotalVal.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblTotalVal.Location = new Point(82, 515);
            lblTotalVal.Name = "lblTotalVal";
            lblTotalVal.Size = new Size(55, 23);
            lblTotalVal.TabIndex = 7;
            lblTotalVal.Text = "$0.00";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            lblTotal.Location = new Point(3, 515);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(54, 23);
            lblTotal.TabIndex = 6;
            lblTotal.Text = "Total:";
            // 
            // lblGSTVal
            // 
            lblGSTVal.AutoSize = true;
            lblGSTVal.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGSTVal.Location = new Point(83, 492);
            lblGSTVal.Name = "lblGSTVal";
            lblGSTVal.Size = new Size(50, 23);
            lblGSTVal.TabIndex = 5;
            lblGSTVal.Text = "$0.00";
            // 
            // lblGST
            // 
            lblGST.AutoSize = true;
            lblGST.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblGST.Location = new Point(3, 492);
            lblGST.Name = "lblGST";
            lblGST.Size = new Size(82, 23);
            lblGST.TabIndex = 4;
            lblGST.Text = "GST (5%):";
            // 
            // lblSubtotalVal
            // 
            lblSubtotalVal.AutoSize = true;
            lblSubtotalVal.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtotalVal.Location = new Point(83, 469);
            lblSubtotalVal.Name = "lblSubtotalVal";
            lblSubtotalVal.Size = new Size(50, 23);
            lblSubtotalVal.TabIndex = 3;
            lblSubtotalVal.Text = "$0.00";
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtotal.Location = new Point(3, 469);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(78, 23);
            lblSubtotal.TabIndex = 2;
            lblSubtotal.Text = "Subtotal:";
            // 
            // CustomerForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 246, 239);
            ClientSize = new Size(982, 603);
            Controls.Add(flwPnlMenu);
            Controls.Add(pnlCategories);
            Controls.Add(pnlYourOrder);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(1000, 650);
            Name = "CustomerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CustomerForm";
            Load += CustomerForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlCategories.ResumeLayout(false);
            pnlYourOrder.ResumeLayout(false);
            pnlYourOrder.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblMenuMate;
        private Label lblGreet;
        private Label lblSeparate;
        private Button btnLogOut;
        private Button btnMyOrders;
        private GroupBox groupBox1;
        private Panel pnlCategories;
        private FlowLayoutPanel flwPnlMenu;
        private Button btnDrinks;
        private Button btnDesserts;
        private Button btnFood;
        private Label lblYourOrder;
        private FlowLayoutPanel flwPnlYourOrder;
        private Panel pnlYourOrder;
        private Label lblSubtotal;
        private Label lblSubtotalVal;
        private Label lblGSTVal;
        private Label lblGST;
        private Label lblTotalVal;
        private Label lblTotal;
        private RadioButton rbTakeaway;
        private RadioButton rbDineIn;
        private Button btnCheckout;
        private Label lblNoItems;
    }
}