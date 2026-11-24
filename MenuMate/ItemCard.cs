using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MenuMate
{
    public partial class ItemCard : UserControl
    {
        private readonly DataManager dataManager = new DataManager();

        public ItemCard()
        {
            InitializeComponent();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ItemName
        {
            get { return lblItemName.Text; }
            set { lblItemName.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ItemPrice
        {
            get { return lblPrice.Text; }
            set { lblPrice.Text = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Image Image
        {
            get { return picBoxItemImg.Image; }
            set { picBoxItemImg.Image = value; }
        }
    }
}
