using WeddingApi.Classes;

namespace WeddingApi.Services.Interfaces
{
    public interface IWeddingGuestsService
    {
        public Task<IEnumerable<WeddingGuest>> GetAllGuestsAsync();
        public Task CreateGuestAsync(string name);
        public Task ConfirmAttendanceWeddingGuestAsync(int id, bool isAttending, string email, string phoneNumber);
    }
}
