namespace MenuMate
{
    // Holds the orders and estimates wait times.
    public class Kitchen
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        // Orders whose Status equals stage, oldest (lowest OrderNumber) first.
        public List<Order> GetOrdersAt(string stage) { return new List<Order>(); /* TODO (Abdulquadri) */ }

        // Minutes until the latest ReadyTime among "Received" and "Preparing" orders (0 if none or already passed),
        // plus the sum of PrepMinutes x Quantity for every item in the cart.
        public int EstimateWaitMinutes(Cart cart) { return 0; /* TODO (Abdulquadri) */ }
    }
}
