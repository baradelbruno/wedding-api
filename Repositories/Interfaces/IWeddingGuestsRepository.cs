using WeddingApi.Classes;

namespace WeddingApi.Repositories.Interfaces
{
    public interface IWeddingGuestsRepository
    {
        public Task<IEnumerable<WeddingGuest>> GetAllGuestsAsync();
        public Task CreateGuestAsync(WeddingGuest weddingGuest);
        public Task<WeddingGuest?> GetGuestByIdAsync(int id);
        public Task UpdateGuestAsync(WeddingGuest weddingGuest);
    }
}
