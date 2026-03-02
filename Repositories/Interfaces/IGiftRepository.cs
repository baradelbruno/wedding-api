using WeddingApi.Classes;

namespace WeddingApi.Repositories.Interfaces
{
    public interface IGiftRepository
    {
        Task<IEnumerable<Gift>> GetAllGiftsAsync();
        Task<Gift?> GetGiftByIdAsync(int id);
        Task CreateGiftAsync(Gift gift);
        Task UpdateGiftAsync(Gift gift);
        Task DeleteGiftAsync(int id);
    }
}