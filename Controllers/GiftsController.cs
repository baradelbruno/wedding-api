using Microsoft.AspNetCore.Mvc;
using WeddingApi.Services.Interfaces;

namespace WeddingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GiftsController : ControllerBase
    {
        private readonly IGiftService _giftService;

        public GiftsController(IGiftService iGiftService)
        {
            _giftService = iGiftService;
        }

        [HttpGet]
        // GET: GiftsController
        public async Task<ActionResult> Get()
        {
            Console.WriteLine("Getting all wedding gifts...");
            var gifts = await _giftService.GetAllGiftsAsync();
            return Ok(gifts);
        }

        [HttpGet("{id}")]
        // GET: GiftsController/5
        public async Task<ActionResult> GetById(int id)
        {
            var gift = await _giftService.GetGiftByIdAsync(id);
            
            if (gift == null)
            {
                return NotFound($"Gift with id {id} not found");
            }
            
            return Ok(gift);
        }

        [HttpPost]
        // POST: GiftsController
        public async Task<ActionResult> Post([FromBody] CreateGiftRequest request)
        {
            await _giftService.CreateGiftAsync(request.Name, request.Description, request.Price, request.ImageUrl);
            return Ok();
        }

        [HttpPut("{id}")]
        // PUT: GiftsController/5
        public async Task<ActionResult> Put(int id, [FromBody] UpdateGiftRequest request)
        {
            try
            {
                await _giftService.UpdateGiftAsync(id, request.Name, request.Description, request.Price, request.ImageUrl);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        // DELETE: GiftsController/5
        public async Task<ActionResult> Delete(int id)
        {
            await _giftService.DeleteGiftAsync(id);
            return Ok();
        }

        [HttpPost("{id}/purchase")]
        // POST: GiftsController/5/purchase
        public async Task<ActionResult> Purchase(int id, [FromBody] PurchaseGiftRequest request)
        {
            try
            {
                var purchase = await _giftService.PurchaseGiftAsync(id, request.PurchasedBy, request.Email, request.Phone, request.PixCode);
                return Ok(purchase);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("purchases/{purchaseId}/confirm")]
        // POST: GiftsController/purchases/5/confirm
        public async Task<ActionResult> ConfirmPayment(int purchaseId)
        {
            try
            {
                await _giftService.ConfirmPaymentAsync(purchaseId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DTOs for the requests
        public record CreateGiftRequest(string Name, string? Description, decimal Price, string? ImageUrl);
        public record UpdateGiftRequest(string Name, string? Description, decimal Price, string? ImageUrl);
        public record PurchaseGiftRequest(string PurchasedBy, string? Email, string? Phone, string PixCode);
    }
}