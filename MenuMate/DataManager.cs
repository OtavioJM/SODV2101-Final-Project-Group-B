namespace MenuMate
{
    // Reads and saves users.txt, menu.txt and orders.txt.
    public class DataManager
    {
        public static readonly string DataFolder = Path.Combine(AppContext.BaseDirectory, "data");

        // Create the data folder and the sample files if they don't exist yet.
        public void EnsureDataFiles() { /* TODO (Otavio) */ }

        public List<User> LoadUsers() { return new List<User>(); /* TODO (Otavio) */ }

        public void SaveUsers(List<User> users) { /* TODO (Otavio) */ }

        public List<MenuItem> LoadMenu() { return new List<MenuItem>(); /* TODO (Otavio) */ }

        public void SaveMenu(List<MenuItem> items) { /* TODO (Otavio) */ }

        public List<Order> LoadOrders() { return new List<Order>(); /* TODO (Otavio) */ }

        public void SaveOrders(List<Order> orders) { /* TODO (Otavio) */ }

        // Loads orders on a background thread so the window never freezes.
        // Returns null if the file is busy (the other window is saving); the caller just skips that refresh.
        public Task<List<Order>?> LoadOrdersAsync() { return Task.FromResult<List<Order>?>(new List<Order>()); /* TODO (Otavio) */ }
    }
}
