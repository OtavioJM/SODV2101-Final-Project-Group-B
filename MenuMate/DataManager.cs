using System.Globalization;
namespace MenuMate
{
    // Reads and saves users.txt, menu.txt and orders.txt.
    public class DataManager
    {
        public static readonly string DataFolder = Path.Combine(AppContext.BaseDirectory, "data");

        // Builds the full path of a file inside the data folder.
        // Example: GetPath("users.txt") -> ...\bin\Debug\net10.0-windows\data\users.txt
        private string GetPath(string fileName)
        {
            return Path.Combine(DataFolder, fileName);
        }

        // Saves lines safely, even if the other window is reading the file at the same moment.
        // 1) Write everything to a temporary file (e.g. menu.tmp).
        // 2) Swap the temporary file into place in one quick step.
        // If the file is busy, wait 100 ms and try again, up to 5 times.
        private void SaveLinesSafely(string fileName, List<string> lines)
        {
            string realPath = GetPath(fileName);
            string tempPath = Path.ChangeExtension(realPath, ".tmp");

            for (int attempt = 1; attempt <= 5; attempt++)
            {
                try
                {
                    File.WriteAllLines(tempPath, lines);
                    File.Move(tempPath, realPath, true); // true = replace the old file
                    return; // it worked, we're done
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                {
                    if (attempt == 5)
                    {
                        // Give up and tell the caller the file is busy
                        throw new IOException("Could not save " + fileName + ". The file is busy.", ex);
                    }
                    Thread.Sleep(100); // wait a moment, then try again
                }
            }
        }

        // The date format used in orders.txt, e.g. 2026-11-10 14:05:00
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

        // Turns "Latte:4.50:1;Croissant:3.25:1" into a list of CartItems.
        // Only Name and Price are saved in orders.txt
        private List<CartItem> ParseItems(string text)
        {
            var items = new List<CartItem>();

            foreach (string itemText in text.Split(';'))
            {
                string[] parts = itemText.Split(':');
                if (parts.Length != 3) continue; // skip broken items

                if (!decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price)) continue;
                if (!int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int quantity)) continue;

                items.Add(new CartItem
                {
                    Item = new MenuItem { Name = parts[0], Price = price },
                    Quantity = quantity
                });
            }

            return items;
        }

        // Create the data folder and the sample files if they don't exist yet.
        public void EnsureDataFiles()
        {
            // Create the data folder. If it already exists, this does nothing.
            Directory.CreateDirectory(DataFolder);

            // Sample users (only written the first time)
            if (!File.Exists(GetPath("users.txt")))
            {
                File.WriteAllLines(GetPath("users.txt"), new[]
                {
                    "mike|owner123|Owner",
                    "sarah|1234|Customer",
                    "tom|1234|Customer"
                });
            }

            // Sample menu: Name|Category|Price|PrepMinutes
            if (!File.Exists(GetPath("menu.txt")))
            {
                File.WriteAllLines(GetPath("menu.txt"), new[]
                {
                    "Latte|Drinks|4.50|3",
                    "Cappuccino|Drinks|4.75|3",
                    "Iced Tea|Drinks|3.25|1",
                    "Hot Chocolate|Drinks|4.00|3",
                    "Croissant|Food|3.25|2",
                    "Muffin|Food|3.00|1",
                    "Bagel|Food|3.50|3",
                    "Sandwich|Food|7.50|6",
                    "Brownie|Desserts|3.50|1",
                    "Cheesecake|Desserts|5.25|2",
                    "Cookie|Desserts|2.25|1"
                });
            }

            // Orders start empty
            if (!File.Exists(GetPath("orders.txt")))
            {
                File.WriteAllText(GetPath("orders.txt"), "");
            }
        }

        // users.txt line: Username|Password|Role
        public List<User> LoadUsers()
        {
            var users = new List<User>();

            foreach (string line in File.ReadAllLines(GetPath("users.txt")))
            {
                string[] parts = line.Split('|');
                if (parts.Length != 3) continue; // skip empty or broken lines

                users.Add(new User
                {
                    Username = parts[0],
                    Password = parts[1],
                    Role = parts[2]
                });
            }

            return users;
        }

        public void SaveUsers(List<User> users)
        {
            var lines = new List<string>();

            foreach (User user in users)
            {
                lines.Add(user.Username + "|" + user.Password + "|" + user.Role);
            }

            SaveLinesSafely("users.txt", lines);
        }

        // menu.txt line: Name|Category|Price|PrepMinutes
        public List<MenuItem> LoadMenu()
        {
            var items = new List<MenuItem>();

            foreach (string line in File.ReadAllLines(GetPath("menu.txt")))
            {
                string[] parts = line.Split('|');
                if (parts.Length != 4) continue;

                // TryParse returns false instead of crashing if the text isn't a number
                if (!decimal.TryParse(parts[2], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price)) continue;
                if (!int.TryParse(parts[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out int prepMinutes)) continue;

                items.Add(new MenuItem
                {
                    Name = parts[0],
                    Category = parts[1],
                    Price = price,
                    PrepMinutes = prepMinutes
                });
            }

            return items;
        }

        public void SaveMenu(List<MenuItem> items)
        {
            var lines = new List<string>();

            foreach (MenuItem item in items)
            {
                lines.Add(item.Name + "|" + item.Category + "|"
                    + item.Price.ToString("0.00", CultureInfo.InvariantCulture) + "|"
                    + item.PrepMinutes.ToString(CultureInfo.InvariantCulture));
            }

            SaveLinesSafely("menu.txt", lines);
        }

        // orders.txt line: OrderNumber|CustomerName|Date|Type|Status|Total|ReadyTime|Items
        // If the file is busy, the IOException goes up to the caller (no try/catch here on purpose).
        public List<Order> LoadOrders()
        {
            var orders = new List<Order>();

            foreach (string line in File.ReadAllLines(GetPath("orders.txt")))
            {
                string[] parts = line.Split('|');
                if (parts.Length != 8) continue;

                if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int number)) continue;
                if (!DateTime.TryParseExact(parts[2], DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)) continue;
                if (!decimal.TryParse(parts[5], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal total)) continue;
                if (!DateTime.TryParseExact(parts[6], DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime readyTime)) continue;

                orders.Add(new Order
                {
                    OrderNumber = number,
                    CustomerName = parts[1],
                    Date = date,
                    Type = parts[3],
                    Status = parts[4],
                    Total = total,
                    ReadyTime = readyTime,
                    Items = ParseItems(parts[7])
                });
            }

            return orders;
        }

        public void SaveOrders(List<Order> orders)
        {
            var lines = new List<string>();

            foreach (Order order in orders)
            {
                // First build the Items text: "Latte:4.50:1;Croissant:3.25:1"
                var itemTexts = new List<string>();
                foreach (CartItem cartItem in order.Items)
                {
                    itemTexts.Add(cartItem.Item.Name + ":"
                        + cartItem.Item.Price.ToString("0.00", CultureInfo.InvariantCulture) + ":"
                        + cartItem.Quantity.ToString(CultureInfo.InvariantCulture));
                }

                // Then join all 8 fields with |
                lines.Add(string.Join("|",
                    order.OrderNumber.ToString(CultureInfo.InvariantCulture),
                    order.CustomerName,
                    order.Date.ToString(DateFormat, CultureInfo.InvariantCulture),
                    order.Type,
                    order.Status,
                    order.Total.ToString("0.00", CultureInfo.InvariantCulture),
                    order.ReadyTime.ToString(DateFormat, CultureInfo.InvariantCulture),
                    string.Join(";", itemTexts)));
            }

            SaveLinesSafely("orders.txt", lines);
        }

        // Loads orders on a background thread so the window never freezes.
        // Returns null if the file is busy (the other window is saving); the caller just skips that refresh.
        public async Task<List<Order>?> LoadOrdersAsync()
        {
            try
            {
                // Run LoadOrders() on a background thread and wait for it without blocking the window
                return await Task.Run(() => LoadOrders());
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                // The file is busy (the other window is saving). The caller skips this refresh.
                return null;
            }
        }
    }
}
