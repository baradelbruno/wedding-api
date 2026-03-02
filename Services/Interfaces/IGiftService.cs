using WeddingApi.Classes;

namespace WeddingApi.Services.Interfaces
{
    public interface IGiftService
    {
        Task<IEnumerable<Gift>> GetAllGiftsAsync();
        Task<Gift?> GetGiftByIdAsync(int id);
        Task CreateGiftAsync(string name, string? description, decimal price, string? imageUrl, string? imageFileName);
        Task UpdateGiftAsync(int id, string name, string? description, decimal price, string? imageUrl, string? imageFileName);
        Task DeleteGiftAsync(int id);
        Task<GiftPurchase> PurchaseGiftAsync(int giftId, string purchasedBy, string? email, string? phone, string pixCode);
        Task ConfirmPaymentAsync(int purchaseId);
    }
}