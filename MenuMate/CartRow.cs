using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MenuMate
{
    public partial class CartRow : UserControl
    {
        public CartRow()
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
        public string ItemQuant
        {
            get { return lblQuantity.Text; }
            set { lblQuantity.Text = value; }
        }
    }
}
