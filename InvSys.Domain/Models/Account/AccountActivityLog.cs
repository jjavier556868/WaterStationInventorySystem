public class AccountActivityLog
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Action { get; set; } // "Login" or "Logout"
    public DateTime Timestamp { get; set; }
}