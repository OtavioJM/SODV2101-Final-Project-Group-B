namespace MenuMate
{
    // A menu item and how many the customer wants.
    public class CartItem
    {
        public MenuItem Item { get; set; } = new MenuItem();
        public int Quantity { get; set; }

        public decimal GetLineTotal()
        {
            return Item.Price * Quantity;
        }
    }
}
