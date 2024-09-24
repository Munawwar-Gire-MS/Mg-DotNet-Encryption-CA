using Microsoft.AspNetCore.Mvc;
using MyEncryptionApp.Models; // Ensure this matches your project namespace
using MyEncryptionApp.Services; // Ensure this matches your project namespace

namespace MyEncryptionApp.Controllers // Ensure this matches your project name
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly EncryptionService _encryptionService;

        public DataController(EncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] EncryptRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Input))
            {
                return BadRequest("Input cannot be empty.");
            }

            var encrypted = _encryptionService.Encrypt(request.Input);
            return Ok(new { EncryptedValue = encrypted });
        }
    }
}
