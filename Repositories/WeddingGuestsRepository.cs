using Microsoft.EntityFrameworkCore;
using WeddingApi.Classes;
using WeddingApi.Data;
using WeddingApi.Repositories.Interfaces;

namespace WeddingApi.Repositories
{
    public class WeddingGuestsRepository : IWeddingGuestsRepository
    {
        private readonly WeddingDbContext _context;

        public WeddingGuestsRepository(WeddingDbContext context)
        {
            _context = context;
        }

            public async Task CreateGuestAsync(WeddingGuest weddingGuest)
            {
                await _context.WeddingGuests.AddAsync(weddingGuest);
                await _context.SaveChangesAsync();
            }

            public async Task<IEnumerable<WeddingGuest>> GetAllGuestsAsync()
            {
                return await _context.WeddingGuests.ToListAsync();
            }

            public async Task<WeddingGuest?> GetGuestByIdAsync(int id)
            {
                return await _context.WeddingGuests.FirstOrDefaultAsync(wg => wg.Id == id);
            }

            public async Task UpdateGuestAsync(WeddingGuest weddingGuest)
        {

            await _context.SaveChangesAsync();
        }
    }
}
