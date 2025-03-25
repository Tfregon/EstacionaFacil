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

        public VehiclesController(VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // 🚗 Entrada de veículo
        [HttpPost("entry")]
        public async Task<IActionResult> RegisterEntry(Vehicle vehicle)
        {
            // Pega o ID do usuário logado a partir do token
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)?? User.FindFirstValue(ClaimTypes.NameIdentifier);


            vehicle.AttendedByUserId = userId;
            vehicle.EntryTime = DateTime.Now;
            vehicle.ExitTime = null;

            var result = await _vehicleService.RegisterEntryAsync(vehicle);
            return Ok(result);
        }

        // 🏁 Saída de veículo
        [HttpPut("exit/{id}")]
        public async Task<IActionResult> RegisterExit(string id)
        {
            var userId =
                User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _vehicleService.RegisterExitAsync(id, userId);
            if (result == null)
                return NotFound("Veículo não encontrado ou já saiu.");

            return Ok(result);
        }



        // 📋 Listar todos os veículos (inclusive os já saíram)
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveVehicles()
        {
            var list = await _vehicleService.GetActiveVehiclesAsync();
            return Ok(list);
        }
    }
}
