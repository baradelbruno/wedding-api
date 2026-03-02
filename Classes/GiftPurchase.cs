namespace WeddingApi.Classes
{
    public class GiftPurchase
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public required string PurchasedBy { get; set; }
        public string? PurchaserEmail { get; set; }
        public string? PurchaserPhone { get; set; }
        public DateTime PurchasedDate { get; set; }
        public required string PixCode { get; set; }
        public bool PaymentConfirmed { get; set; }
        public DateTime? PaymentConfirmedDate { get; set; }
        
        // Navigation property
        public Gift Gift { get; set; } = null!;
    }
}