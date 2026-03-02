using WeddingApi.Classes;
using WeddingApi.Repositories.Interfaces;
using WeddingApi.Services.Interfaces;

namespace WeddingApi.Services
{
    public class GiftService : IGiftService
    {
        private readonly IGiftRepository _giftRepository;
        private readonly IGiftPurchaseRepository _purchaseRepository;

        public GiftService(IGiftRepository giftRepository, IGiftPurchaseRepository purchaseRepository)
        {
            _giftRepository = giftRepository;
            _purchaseRepository = purchaseRepository;
        }

        public async Task<IEnumerable<Gift>> GetAllGiftsAsync()
        {
            return await _giftRepository.GetAllGiftsAsync();
        }

        public async Task<Gift?> GetGiftByIdAsync(int id)
        {
            return await _giftRepository.GetGiftByIdAsync(id);
        }

        public async Task CreateGiftAsync(string name, string? description, decimal price, string? imageUrl)
        {
            var gift = new Gift
            {
                Name = name,
                Description = description,
                Price = price,
                ImageUrl = imageUrl
            };

            await _giftRepository.CreateGiftAsync(gift);
        }

        public async Task UpdateGiftAsync(int id, string name, string? description, decimal price, string? imageUrl)
        {
            var gift = await _giftRepository.GetGiftByIdAsync(id);
            
            if (gift == null)
            {
                throw new Exception($"No gift found with id {id}");
            }

            gift.Name = name;
            gift.Description = description;
            gift.Price = price;
            gift.ImageUrl = imageUrl;

            await _giftRepository.UpdateGiftAsync(gift);
        }

        public async Task DeleteGiftAsync(int id)
        {
            await _giftRepository.DeleteGiftAsync(id);
        }

        public async Task<GiftPurchase> PurchaseGiftAsync(int giftId, string purchasedBy, string? email, string? phone, string pixCode)
        {
            var gift = await _giftRepository.GetGiftByIdAsync(giftId);
            
            if (gift == null)
            {
                throw new Exception($"No gift found with id {giftId}");
            }

            var purchase = new GiftPurchase
            {
                GiftId = giftId,
                PurchasedBy = purchasedBy,
                PurchaserEmail = email,
                PurchaserPhone = phone,
                PurchasedDate = DateTime.UtcNow,
                PixCode = pixCode,
                PaymentConfirmed = false
            };

            await _purchaseRepository.CreatePurchaseAsync(purchase);
            return purchase;
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