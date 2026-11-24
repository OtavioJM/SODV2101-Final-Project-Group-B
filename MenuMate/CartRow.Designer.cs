namespace MenuMate
{
    partial class CartRow
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
            lblItemName = new Label();
            btnSubtract = new Button();
            btnAdd = new Button();
            lblQuantity = new Label();
            SuspendLayout();
            // 
            // lblItemName
            // 
            lblItemName.AutoSize = true;
            lblItemName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblItemName.Location = new Point(7, 7);
            lblItemName.Name = "lblItemName";
            lblItemName.Size = new Size(91, 23);
            lblItemName.TabIndex = 0;
            lblItemName.Text = "ItemName";
            // 
            // btnSubtract
            // 
            btnSubtract.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSubtract.Location = new Point(198, 3);
            btnSubtract.Name = "btnSubtract";
            btnSubtract.Size = new Size(30, 30);
            btnSubtract.TabIndex = 1;
            btnSubtract.Text = "-";
            btnSubtract.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.Location = new Point(267, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(30, 30);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // lblQuantity
            // 
            lblQuantity.AutoSize = true;
            lblQuantity.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblQuantity.Location = new Point(239, 7);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(19, 23);
            lblQuantity.TabIndex = 3;
            lblQuantity.Text = "0";
            lblQuantity.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CartRow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblQuantity);
            Controls.Add(btnAdd);
            Controls.Add(btnSubtract);
            Controls.Add(lblItemName);
            Name = "CartRow";
            Size = new Size(300, 36);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblItemName;
        private Label lblQuantity;
        public Button btnSubtract;
        public Button btnAdd;
    }
}
