using WeddingApi.Classes;

namespace WeddingApi.Services.Interfaces
{
    public interface IGiftPurchaseService
    {
        Task<IEnumerable<GiftPurchase>> GetAllPurchasesAsync();
        Task<IEnumerable<GiftPurchase>> GetPurchasesByGiftIdAsync(int giftId);
        Task<GiftPurchase?> GetPurchaseByIdAsync(int id);
        Task ConfirmPaymentAsync(int purchaseId);
    }
}