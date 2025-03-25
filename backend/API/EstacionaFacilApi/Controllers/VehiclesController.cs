using EstacionaFacilAPI.Models;
using EstacionaFacilAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace EstacionaFacilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔐 Exige token JWT
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleService _vehicleService;
        private readonly DailyCashService _cashService;

        public VehiclesController(VehicleService vehicleService, DailyCashService cashService)
        {
            _vehicleService = vehicleService;
            _cashService = cashService;
        }

        // 🚗 Entrada de veículo
        [HttpPost("entry")]
        public async Task<IActionResult> RegisterEntry(Vehicle vehicle)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                       ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue("name") ?? "Funcionário";

            vehicle.AttendedByUserId = userId;
            vehicle.EntryTime = DateTime.Now;
            vehicle.ExitTime = null;

            var result = await _vehicleService.RegisterEntryAsync(vehicle, _cashService, userName);
            return Ok(result);
        }

        // 🏁 Saída de veículo
        [HttpPut("exit/{id}")]
        public async Task<IActionResult> RegisterExit(string id, [FromBody] decimal? additionalAmount)
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                        ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var result = await _vehicleService.RegisterExitAsync(id, userId, additionalAmount, _cashService);
                if (result == null)
                    return NotFound("Veículo não encontrado ou já saiu.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 📋 Listar apenas veículos ativos (sem saída)
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveVehicles()
        {
            var list = await _vehicleService.GetActiveVehiclesAsync();
            return Ok(list);
        }

        // (opcional) 📋 Listar todos os veículos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _vehicleService.GetAllVehiclesAsync();
            return Ok(list);
        }
    }
}
