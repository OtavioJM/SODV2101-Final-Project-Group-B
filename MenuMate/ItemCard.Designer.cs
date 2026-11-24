namespace MenuMate
{
    partial class ItemCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            picBoxItemImg = new PictureBox();
            lblItemName = new Label();
            lblPrice = new Label();
            ((System.ComponentModel.ISupportInitialize)picBoxItemImg).BeginInit();
            SuspendLayout();
            // 
            // picBoxItemImg
            // 
            picBoxItemImg.Location = new Point(10, 10);
            picBoxItemImg.MaximumSize = new Size(130, 70);
            picBoxItemImg.MinimumSize = new Size(130, 70);
            picBoxItemImg.Name = "picBoxItemImg";
            picBoxItemImg.Size = new Size(130, 70);
            picBoxItemImg.SizeMode = PictureBoxSizeMode.Zoom;
            picBoxItemImg.TabIndex = 0;
            picBoxItemImg.TabStop = false;
            // 
            // lblItemName
            // 
            lblItemName.AutoSize = true;
            lblItemName.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblItemName.Location = new Point(-1, 99);
            lblItemName.MaximumSize = new Size(120, 0);
            lblItemName.Name = "lblItemName";
            lblItemName.Size = new Size(94, 23);
            lblItemName.TabIndex = 1;
            lblItemName.Text = "ItemName";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPrice.Location = new Point(101, 99);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(50, 23);
            lblPrice.TabIndex = 2;
            lblPrice.Text = "$0.00";
            // 
            // ItemCard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(lblPrice);
            Controls.Add(lblItemName);
            Controls.Add(picBoxItemImg);
            MaximumSize = new Size(150, 150);
            MinimumSize = new Size(150, 150);
            Name = "ItemCard";
            Size = new Size(148, 148);
            ((System.ComponentModel.ISupportInitialize)picBoxItemImg).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public PictureBox picBoxItemImg;
        public Label lblItemName;
        public Label lblPrice;
    }
}
