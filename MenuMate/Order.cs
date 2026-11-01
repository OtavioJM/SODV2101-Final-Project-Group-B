namespace MenuMate
{
    // A confirmed order.
    public class Order
    {
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; } = "";
        public DateTime Date { get; set; }
        public string Type { get; set; } = "Dine-in";    // "Dine-in" or "Takeaway"
        public string Status { get; set; } = "Received";  // "Received", "Preparing", "Ready", "Picked up"
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total { get; set; }                 // includes GST
        public DateTime ReadyTime { get; set; }            // estimated time the order will be ready

        // Move one stage forward. Does nothing at "Picked up".
        public void MoveToNextStage() { /* TODO (Abdulquadri) */ }

        // 0 if Ready or Picked up, or if ReadyTime has passed. Otherwise minutes until ReadyTime, rounded up.
        public int GetMinutesLeft() { return 0; /* TODO (Abdulquadri) */ }

        // Several lines: order number, date, type, each item like "2 x Latte   $9.00", subtotal, GST and total.
        // Subtotal = sum of item line totals; GST = Total - subtotal.
        public string GetReceiptText() { return ""; /* TODO (Abdulquadri) */ }
    }
}
