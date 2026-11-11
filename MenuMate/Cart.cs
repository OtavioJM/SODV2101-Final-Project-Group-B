namespace MenuMate
{
    // The customer's order before checkout.
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // If an item with the same Name is already in the cart, add 1 to its Quantity; otherwise add it with Quantity 1.
        public void AddItem(MenuItem item) 
        {
            CartItem newItem = new CartItem();
            newItem.Item = item;
            newItem.Quantity = 1;

            if(Items.Count == 0)
            {
                Items.Add(newItem);
            }
            else
            {
                foreach (CartItem Item in Items.ToList())
                {
                    if (newItem.Item.Name.Equals(Item.Item.Name))
                    {
                        Item.Quantity++;
                        return;
                    }
                }
                Items.Add(newItem);
            }
            
        }

        // Remove the line whose item has the same Name.
        public void RemoveItem(MenuItem item) 
        { 
            foreach(CartItem Item in Items.ToList())
            {
                if(Item.Item.Name == item.Name)
                {
                    Items.Remove(Item);
                }
            }
        }

        // Set the quantity of that item. If qty is 0 or less, remove it.
        public void ChangeQuantity(MenuItem item, int qty) 
        { 
            foreach(CartItem Item in Items.ToList())
            {
                if(Item.Item.Name == item.Name)
                {
                    Item.Quantity = qty;
                    if(Item.Quantity <= 0)
                    {
                        Items.Remove(Item);
                    }
                }
            }
        }

        public decimal GetSubtotal() 
        {
            decimal Subtotal = 0;
            foreach (CartItem Item in Items)
            {
                Subtotal += Item.GetLineTotal();
            }
            return Subtotal;
        }

        public decimal GetGST() 
        {
            decimal GST = Math.Round(GetSubtotal() * 0.05m, 2);
            return GST;
        }

        public decimal GetTotal() 
        {
            decimal Total = GetSubtotal() + GetGST();
            return Total;
        }
    }
}
