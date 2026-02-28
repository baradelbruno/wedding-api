using WeddingApi.Classes;
using WeddingApi.Repositories.Interfaces;
using WeddingApi.Services.Interfaces;

namespace WeddingApi.Services
{
    public class WeddingGuestsService(IWeddingGuestsRepository iWeddingGuestsRepository) : IWeddingGuestsService
    {
        public IWeddingGuestsRepository IWeddingGuestsRepository { get; set; } = iWeddingGuestsRepository;

        public async Task ConfirmAttendanceWeddingGuestAsync(int id, bool isAttending, string email, string phoneNumber)
        {
            var weddingGuestToBeUpdated = await IWeddingGuestsRepository.GetGuestByIdAsync(id);

            if (weddingGuestToBeUpdated == null)
            {
                throw new Exception($"No wedding guest found with id {id}");
            }

            weddingGuestToBeUpdated.IsAttending = isAttending;
            weddingGuestToBeUpdated.Email = email;
            weddingGuestToBeUpdated.PhoneNumber = phoneNumber;

            await IWeddingGuestsRepository.UpdateGuestAsync(weddingGuestToBeUpdated!);
        }

        public async Task CreateGuestAsync(string name)
        {
            var weddingGuest = new WeddingGuest
            {
                Name = name,
                IsAttending = false
            };

            await IWeddingGuestsRepository.CreateGuestAsync(weddingGuest);
        }

        public async Task<IEnumerable<WeddingGuest>> GetAllGuestsAsync()
        {
            
            return await IWeddingGuestsRepository.GetAllGuestsAsync();
        }
    }
}
