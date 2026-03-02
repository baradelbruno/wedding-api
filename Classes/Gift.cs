namespace WeddingApi.Classes
{
    public class Gift
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation property - a gift can have multiple purchases
        public ICollection<GiftPurchase> Purchases { get; set; } = new List<GiftPurchase>();
    }
}