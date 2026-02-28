namespace WeddingApi.Classes
{
    public class WeddingGuest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsAttending { get; set; }
    }
}
