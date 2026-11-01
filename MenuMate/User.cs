namespace MenuMate
{
    // A person who can log in.
    public class User
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Customer"; // "Owner" or "Customer"

        public bool IsOwner()
        {
            return Role == "Owner";
        }
    }
}
