using System.Globalization;
using System.Text;

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
        public void MoveToNextStage()
        {
            if (Status == "Received")
            {
                Status = "Preparing";
            }
            else if (Status == "Preparing")
            {
                Status = "Ready";
            }
            else if (Status == "Ready")
            {
                Status = "Picked up";
            }
            // At "Picked up" nothing happens.
        }

        // 0 if Ready or Picked up, or if ReadyTime has passed. Otherwise minutes until ReadyTime, rounded up.
        public int GetMinutesLeft()
        {
            if (Status == "Ready" || Status == "Picked up")
            {
                return 0;
            }

            double minutes = (ReadyTime - DateTime.Now).TotalMinutes;

            if (minutes <= 0)
            {
                return 0;
            }

            // Round up, so 6.2 minutes shows as 7.
            return (int)Math.Ceiling(minutes);
        }

        // Several lines: order number, date, type, each item like "2 x Latte   $9.00", subtotal, GST and total.
        public string GetReceiptText()
        {
            const int width = 32;
            string divider = new string('-', width);

            // Subtotal = sum of item line totals; GST = Total - subtotal.
            decimal subtotal = 0;
            foreach (CartItem item in Items)
            {
                subtotal += item.GetLineTotal();
            }
            decimal gst = Math.Round(Total - subtotal, 2);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MenuMate - Order #" + OrderNumber.ToString(CultureInfo.InvariantCulture));
            sb.AppendLine(Date.ToString("MMM d, yyyy h:mm tt", CultureInfo.InvariantCulture) + " - " + Type);
            sb.AppendLine(divider);

            foreach (CartItem item in Items)
            {
                string left = item.Quantity.ToString(CultureInfo.InvariantCulture) + " x " + item.Item.Name;
                sb.AppendLine(FormatLine(left, item.GetLineTotal(), width));
            }

            sb.AppendLine(divider);
            sb.AppendLine(FormatLine("Subtotal", subtotal, width));
            sb.AppendLine(FormatLine("GST (5%)", gst, width));
            sb.AppendLine(FormatLine("Total", Total, width));

            return sb.ToString();
        }

        // Private helper: puts the text on the left and the price on the right.
        private static string FormatLine(string left, decimal amount, int width)
        {
            string price = "$" + Math.Round(amount, 2).ToString("0.00", CultureInfo.InvariantCulture);
            int leftWidth = Math.Max(left.Length + 1, width - price.Length);
            return left.PadRight(leftWidth) + price;
        }
    }
}