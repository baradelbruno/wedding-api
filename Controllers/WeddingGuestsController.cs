using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WeddingApi.Services.Interfaces;

namespace WeddingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeddingGuestsController(IWeddingGuestsService iWeddingGuestsService) : ControllerBase
    {
        public IWeddingGuestsService IWeddingGuestsService { get; } = iWeddingGuestsService;

        [HttpGet]
        // GET: WeddingGuestsController
        public async Task<ActionResult> Get()
        {
            Console.WriteLine("Getting all wedding guests...");
            var guests = await IWeddingGuestsService.GetAllGuestsAsync();
            return Ok(guests);
        }

        [HttpPost]
        //Post: WeddingGuestsController
        public async Task<ActionResult> Post([FromBody] CreateWeddingGuestRequest request)
        {
            await IWeddingGuestsService.CreateGuestAsync(request.Name);
            return Ok();
        }

        [HttpPost("upload-csv")]
        //Post: WeddingGuestsController/upload-csv
        public async Task<ActionResult> PostCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (!file.ContentType.Equals("text/csv", StringComparison.OrdinalIgnoreCase)
                && !file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid file format. Please upload a CSV file.");
            }

            var guestsCreated = 0;
            var errors = new List<string>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var name = line.Trim();

                    try
                    {
                        await IWeddingGuestsService.CreateGuestAsync(name);
                        guestsCreated++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Failed to create guest '{name}': {ex.Message}");
                    }
                }
            }

            return Ok(new
            {
                Message = $"Successfully created {guestsCreated} guest(s).",
                GuestsCreated = guestsCreated,
                Errors = errors.Count > 0 ? errors : null
            });
        }

        [HttpPut]
        //PUT: WeddingGuestsController/5
        public async Task<ActionResult> Put(int id, [FromBody] ConfirmAttendanceWeddingGuestRequest request)
        {
            await IWeddingGuestsService.ConfirmAttendanceWeddingGuestAsync(id, request.IsAttending, request.Email, request.PhoneNumber);
            return Ok();
        }

        // DTO for the request
        public record CreateWeddingGuestRequest(string Name);
        public record ConfirmAttendanceWeddingGuestRequest(string Email, string PhoneNumber, bool IsAttending);
    }
}
