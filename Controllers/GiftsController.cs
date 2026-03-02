using Microsoft.AspNetCore.Mvc;
using WeddingApi.Services.Interfaces;

namespace WeddingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GiftsController : ControllerBase
    {
        private readonly IGiftService _giftService;
        private readonly IWebHostEnvironment _environment;

        public GiftsController(IGiftService iGiftService, IWebHostEnvironment environment)
        {
            _giftService = iGiftService;
            _environment = environment;
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
        public async Task<ActionResult> Post([FromForm] CreateGiftRequest request)
        {
            string? imageUrl = null;
            string? imageFileName = null;

            // Handle image upload
            if (request.Image != null && request.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "gifts");
                Directory.CreateDirectory(uploadsFolder);

                imageFileName = $"{Guid.NewGuid()}_{request.Image.FileName}";
                var filePath = Path.Combine(uploadsFolder, imageFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/gifts/{imageFileName}";
            }
            else if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                imageUrl = request.ImageUrl;
            }

            await _giftService.CreateGiftAsync(request.Name, request.Description, request.Price, imageUrl, imageFileName);
            return Ok();
        }

        [HttpPut("{id}")]
        // PUT: GiftsController/5
        public async Task<ActionResult> Put(int id, [FromForm] UpdateGiftRequest request)
        {
            try
            {
                var gift = await _giftService.GetGiftByIdAsync(id);
                if (gift == null)
                {
                    return NotFound($"Gift with id {id} not found");
                }

                string? imageUrl = gift.ImageUrl;
                string? imageFileName = gift.ImageFileName;

                // Handle new image upload
                if (request.Image != null && request.Image.Length > 0)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(gift.ImageFileName))
                    {
                        var oldImagePath = Path.Combine(_environment.WebRootPath, "uploads", "gifts", gift.ImageFileName);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Upload new image
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "gifts");
                    Directory.CreateDirectory(uploadsFolder);

                    imageFileName = $"{Guid.NewGuid()}_{request.Image.FileName}";
                    var filePath = Path.Combine(uploadsFolder, imageFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.Image.CopyToAsync(stream);
                    }

                    imageUrl = $"/uploads/gifts/{imageFileName}";
                }
                else if (!string.IsNullOrEmpty(request.ImageUrl))
                {
                    imageUrl = request.ImageUrl;
                }

                await _giftService.UpdateGiftAsync(id, request.Name, request.Description, request.Price, imageUrl, imageFileName);
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
            var gift = await _giftService.GetGiftByIdAsync(id);
            
            // Delete associated image file
            if (gift != null && !string.IsNullOrEmpty(gift.ImageFileName))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, "uploads", "gifts", gift.ImageFileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

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
        public record CreateGiftRequest(string Name, string? Description, decimal Price, IFormFile? Image, string? ImageUrl);
        public record UpdateGiftRequest(string Name, string? Description, decimal Price, IFormFile? Image, string? ImageUrl);
        public record PurchaseGiftRequest(string PurchasedBy, string? Email, string? Phone, string PixCode);
    }
}