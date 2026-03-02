using Microsoft.EntityFrameworkCore;
using WeddingApi.Classes;
using WeddingApi.Data;
using WeddingApi.Repositories.Interfaces;

namespace WeddingApi.Repositories
{
    public class GiftRepository : IGiftRepository
    {
        private readonly WeddingDbContext _context;

        public GiftRepository(WeddingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gift>> GetAllGiftsAsync()
        {
            return await _context.Gifts
                .Include(g => g.Purchases)
                .ToListAsync();
        }

        public async Task<Gift?> GetGiftByIdAsync(int id)
        {
            return await _context.Gifts
                .Include(g => g.Purchases)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task CreateGiftAsync(Gift gift)
        {
            await _context.Gifts.AddAsync(gift);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGiftAsync(Gift gift)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGiftAsync(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift != null)
            {
                _context.Gifts.Remove(gift);
                await _context.SaveChangesAsync();
            }
        }
    }
}