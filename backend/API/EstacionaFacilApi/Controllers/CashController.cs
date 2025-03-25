using EstacionaFacilAPI.Models;
using EstacionaFacilAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstacionaFacilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CashController : ControllerBase
    {
        private readonly DailyCashService _cashService;

        public CashController(DailyCashService cashService)
        {
            _cashService = cashService;
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetToday()
        {
            var result = await _cashService.GetTodayAsync();

            if (result == null)
            {
                return Ok(new DailyCash
                {
                    Date = DateTime.Today,
                    Total = 0
                });
            }

            return Ok(result);
        }


        [HttpGet("{date}")]
        public async Task<IActionResult> GetByDate(string date)
        {
            if (!DateTime.TryParse(date, out var parsed))
                return BadRequest("Data inválida");

            var result = await _cashService.GetByDateAsync(parsed);

            if (result == null)
            {
                return Ok(new DailyCash
                {
                    Date = parsed.Date,
                    Total = 0
                });
            }

            return Ok(result);
        }

    }
}
