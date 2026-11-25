using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace MenuMate
{
    public partial class CustomerForm : Form
    {
        private readonly User currentUser;
        private readonly DataManager dataManager = new DataManager();
        private string selectedCategory = "Drinks";
        private Cart _customerCart = new Cart();

        public CustomerForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            this.Icon = Icon.FromHandle(Properties.Resources.logo.GetHicon());
            RefreshMenu(selectedCategory);
            btnDrinks.BackColor = Color.FromArgb(47, 107, 79);
            btnDrinks.ForeColor = Color.White;
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            lblGreet.Text = $"Hello, {currentUser.Username}";
            dataManager.LoadMenu();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Cart CustomerCart
        {
            get { return _customerCart; }
            set { _customerCart = value; }
        }
        private void RefreshCart()
        {
            foreach (var items in CustomerCart.Items)
            {
                Debug.WriteLine($"{items.Item.Name} - {items.Quantity}");
            }

            List<Control> CartRows = flwPnlYourOrder.Controls.Cast<Control>().ToList();
            foreach (Control cartRow in CartRows)
            {
                flwPnlYourOrder.Controls.Remove(cartRow);
                cartRow.Dispose();
            }

            foreach (var items in CustomerCart.Items)
            {
                Debug.WriteLine($"{items.Item.Name} - {items.Quantity}");
                CartRow cartRow = new CartRow();
                cartRow.ItemName = items.Item.Name;
                cartRow.ItemQuant = items.Quantity.ToString();
                flwPnlYourOrder.Controls.Add(cartRow);
                cartRow.btnAdd.Click += (sender, e) => btnAddSubtract_Click(sender, e, cartRow.ItemName, 1);
                cartRow.btnSubtract.Click += (sender, e) => btnAddSubtract_Click(sender, e, cartRow.ItemName, -1);
            }
            lblSubtotalVal.Text = $"${CustomerCart.GetSubtotal()}";
            lblGSTVal.Text = $"${CustomerCart.GetGST()}";
            lblTotalVal.Text = $"${CustomerCart.GetTotal()}";
        }
        private void btnAddSubtract_Click(object sender, EventArgs e, string ItemName, int Qty)
        {
            foreach (var item in dataManager.LoadMenu())
            {
                if (item.Name == ItemName)
                {
                    CustomerCart.ChangeQuantity(item, Qty);
                }
            }
            RefreshCart();
        }

        private void RefreshMenu(string selectedCategory)
        {
            // remove all controls in the flow panel
            List<Control> MenuCards = flwPnlMenu.Controls.Cast<Control>().ToList();
            foreach (Control menuCard in MenuCards)
            {
                flwPnlMenu.Controls.Remove(menuCard);
                menuCard.Dispose();
            }

            var filter = from MenuItem item in dataManager.LoadMenu()
                         where item.Category == selectedCategory
                         select item;

            foreach (MenuItem item in filter)
            {
                ItemCard itemCard = new ItemCard();
                itemCard.ItemName = item.Name;
                itemCard.ItemPrice = $"${item.Price}";
                if(File.Exists(Path.Combine(AppContext.BaseDirectory, "images", item.Name + ".png")))
                {
                    itemCard.Image = Image.FromFile(Path.Combine(AppContext.BaseDirectory, "images", item.Name + ".png"));
                }
                flwPnlMenu.Controls.Add(itemCard);
                itemCard.Click += (sender, e) => itemCard_Click(sender, e, itemCard.ItemName);
                itemCard.picBoxItemImg.Click += (sender, e) => itemCard_Click(sender, e, itemCard.ItemName);
                itemCard.lblItemName.Click += (sender, e) => itemCard_Click(sender, e, itemCard.ItemName);
                itemCard.lblPrice.Click += (sender, e) => itemCard_Click(sender, e, itemCard.ItemName);
            }
        }
        private void itemCard_Click(object sender, EventArgs e, string ItemName)
        {
            lblNoItems.Text = "";
            foreach (var item in dataManager.LoadMenu())
            {
                if (item.Name == ItemName)
                {
                    CustomerCart.AddItem(item);
                }
            }
            RefreshCart();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (CustomerCart.Items.Count <= 0)
            {
                lblNoItems.Text = "Your cart is empty!";
                return;
            }
            else
            {
                lblNoItems.Text = "";
                DialogResult result = MessageBox.Show($"Place your order? Your total is ${CustomerCart.GetTotal()}. Stop if you aren't ready.", "Finalize your order", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }
                else
                {
                    try
                    {
                        var orders = dataManager.LoadOrders();
                        var kitchen = new Kitchen { Orders = orders };
                        int wait = kitchen.EstimateWaitMinutes(CustomerCart);
                        Debug.WriteLine($"This order will be ready in {wait} minutes");
                        Order order = new Order();
                        if (orders.Count == 0)
                        {
                            order.OrderNumber = 1001;
                        }
                        else
                        {
                            order.OrderNumber = orders.Max(n => n.OrderNumber) + 1;
                        }
                        order.CustomerName = currentUser.Username;
                        order.Date = DateTime.Now;
                        if (rbDineIn.Checked)
                        {
                            order.Type = "Dine-in";
                        }
                        else
                        {
                            order.Type = "Takeaway";
                        }
                        order.ReadyTime = order.Date.AddMinutes(wait);
                        order.Items = new List<CartItem>(CustomerCart.Items);
                        order.Total = CustomerCart.GetTotal();
                        orders.Add(order);
                        try
                        {
                            dataManager.SaveOrders(orders);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("We couldn't place your order. Please try again.", $"Something went wrong while loading our existing orders", MessageBoxButtons.OK);
                            return;
                        }

                        TrackerForm tracker = new TrackerForm(order.OrderNumber);
                        tracker.Show();

                        CustomerCart = new Cart();
                        RefreshCart();
                        rbDineIn.Checked = true;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("We couldn't place your order. Please try again.", $"Something went wrong while saving your order", MessageBoxButtons.OK);
                        return;
                    }
                    return;
                }

            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMyOrders_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void btnDrinks_Click(object sender, EventArgs e)
        {
            selectedCategory = "Drinks";
            RefreshMenu(selectedCategory);
            btnDrinks.BackColor = Color.FromArgb(47, 107, 79);
            btnDrinks.ForeColor = Color.White;
            btnFood.BackColor = Color.White;
            btnFood.ForeColor = Color.Black;
            btnDesserts.BackColor = Color.White;
            btnDesserts.ForeColor = Color.Black;
        }

        private void btnFood_Click(object sender, EventArgs e)
        {
            selectedCategory = "Food";
            RefreshMenu(selectedCategory);
            btnFood.BackColor = Color.FromArgb(47, 107, 79);
            btnFood.ForeColor = Color.White;
            btnDrinks.BackColor = Color.White;
            btnDrinks.ForeColor = Color.Black;
            btnDesserts.BackColor = Color.White;
            btnDesserts.ForeColor = Color.Black;
        }

        private void btnDesserts_Click(object sender, EventArgs e)
        {
            selectedCategory = "Desserts";
            RefreshMenu(selectedCategory);
            btnDesserts.BackColor = Color.FromArgb(47, 107, 79);
            btnDesserts.ForeColor = Color.White;
            btnDrinks.BackColor = Color.White;
            btnDrinks.ForeColor = Color.Black;
            btnFood.BackColor = Color.White;
            btnFood.ForeColor = Color.Black;
        }
    }
}
