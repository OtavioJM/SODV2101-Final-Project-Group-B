namespace MenuMate
{
    // The customer's order before checkout.
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        // If an item with the same Name is already in the cart, add 1 to its Quantity; otherwise add it with Quantity 1.
        public void AddItem(MenuItem item) { /* TODO (Keane) */ }

        // Remove the line whose item has the same Name.
        public void RemoveItem(MenuItem item) { /* TODO (Keane) */ }

        // Set the quantity of that item. If qty is 0 or less, remove it.
        public void ChangeQuantity(MenuItem item, int qty) { /* TODO (Keane) */ }

        public decimal GetSubtotal() { return 0; /* TODO (Keane): sum of GetLineTotal() */ }

        public decimal GetGST() { return 0; /* TODO (Keane): Math.Round(GetSubtotal() * 0.05m, 2) */ }

        public decimal GetTotal() { return 0; /* TODO (Keane): GetSubtotal() + GetGST() */ }
    }
}
