namespace MenuMate
{
    // One item on the menu.
    public class MenuItem
    {
        public string Name { get; set; } = "";
        public string Category { get; set; } = "Drinks"; // "Drinks", "Food" or "Desserts"
        public decimal Price { get; set; }
        public int PrepMinutes { get; set; }             // minutes to prepare one
    }
}
