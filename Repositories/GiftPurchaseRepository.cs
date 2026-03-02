using Microsoft.EntityFrameworkCore;
using WeddingApi.Classes;
using WeddingApi.Data;
using WeddingApi.Repositories.Interfaces;

namespace WeddingApi.Repositories
{
    public class GiftPurchaseRepository : IGiftPurchaseRepository
    {
        private readonly WeddingDbContext _context;

        public GiftPurchaseRepository(WeddingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GiftPurchase>> GetAllPurchasesAsync()
        {
            return await _context.GiftPurchases
                .Include(p => p.Gift)
                .ToListAsync();
        }

        public async Task<IEnumerable<GiftPurchase>> GetPurchasesByGiftIdAsync(int giftId)
        {
            return await _context.GiftPurchases
                .Where(p => p.GiftId == giftId)
                .Include(p => p.Gift)
                .ToListAsync();
        }

        public async Task<GiftPurchase?> GetPurchaseByIdAsync(int id)
        {
            return await _context.GiftPurchases
                .Include(p => p.Gift)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreatePurchaseAsync(GiftPurchase purchase)
        {
            await _context.GiftPurchases.AddAsync(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePurchaseAsync(GiftPurchase purchase)
        {
            await _context.SaveChangesAsync();
        }
    }
}