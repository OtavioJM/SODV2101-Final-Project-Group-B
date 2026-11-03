using System.Linq;

namespace MenuMate
{
    // Holds the orders and estimates wait times.
    public class Kitchen
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        // Orders whose Status equals stage, oldest (lowest OrderNumber) first.
        public List<Order> GetOrdersAt(string stage)
        {
            return Orders
                .Where(o => o.Status == stage)
                .OrderBy(o => o.OrderNumber)
                .ToList();
        }

        // Minutes until the latest ReadyTime among "Received" and "Preparing" orders (0 if none or already passed),
        // plus the sum of PrepMinutes x Quantity for every item in the cart.
        public int EstimateWaitMinutes(Cart cart)
        {
            // Step 1: how long until the kitchen finishes the orders already in line?
            int queueMinutes = 0;

            List<Order> inLine = Orders
                .Where(o => o.Status == "Received" || o.Status == "Preparing")
                .ToList();

            if (inLine.Count > 0)
            {
                DateTime latestReadyTime = inLine.Max(o => o.ReadyTime);
                double minutes = (latestReadyTime - DateTime.Now).TotalMinutes;

                if (minutes > 0)
                {
                    queueMinutes = (int)Math.Ceiling(minutes);
                }
            }

            // Step 2: how long will the new cart take to prepare?
            int prepMinutes = 0;
            foreach (CartItem item in cart.Items)
            {
                prepMinutes += item.Item.PrepMinutes * item.Quantity;
            }

            return queueMinutes + prepMinutes;
        }
    }
}
