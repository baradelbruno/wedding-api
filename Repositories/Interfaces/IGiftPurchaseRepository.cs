using WeddingApi.Classes;

namespace WeddingApi.Repositories.Interfaces
{
    public interface IGiftPurchaseRepository
    {
        Task<IEnumerable<GiftPurchase>> GetAllPurchasesAsync();
        Task<IEnumerable<GiftPurchase>> GetPurchasesByGiftIdAsync(int giftId);
        Task<GiftPurchase?> GetPurchaseByIdAsync(int id);
        Task CreatePurchaseAsync(GiftPurchase purchase);
        Task UpdatePurchaseAsync(GiftPurchase purchase);
    }
}