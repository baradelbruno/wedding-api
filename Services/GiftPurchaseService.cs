using WeddingApi.Classes;
using WeddingApi.Repositories.Interfaces;
using WeddingApi.Services.Interfaces;

namespace WeddingApi.Services
{
    public class GiftPurchaseService : IGiftPurchaseService
    {
        private readonly IGiftPurchaseRepository _purchaseRepository;

        public GiftPurchaseService(IGiftPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<IEnumerable<GiftPurchase>> GetAllPurchasesAsync()
        {
            return await _purchaseRepository.GetAllPurchasesAsync();
        }

        public async Task<IEnumerable<GiftPurchase>> GetPurchasesByGiftIdAsync(int giftId)
        {
            return await _purchaseRepository.GetPurchasesByGiftIdAsync(giftId);
        }

        public async Task<GiftPurchase?> GetPurchaseByIdAsync(int id)
        {
            return await _purchaseRepository.GetPurchaseByIdAsync(id);
        }

        public async Task ConfirmPaymentAsync(int purchaseId)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);
            
            if (purchase == null)
            {
                throw new Exception($"No purchase found with id {purchaseId}");
            }

            purchase.PaymentConfirmed = true;
            purchase.PaymentConfirmedDate = DateTime.UtcNow;

            await _purchaseRepository.UpdatePurchaseAsync(purchase);
        }
    }
}