using Microsoft.AspNetCore.Mvc;

namespace EstacionaFacilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        // Endpoint para checar versão e autor do projeto
        [HttpGet("version")]
        public IActionResult GetVersion()
        {
            return Ok(new
            {
                Project = "EstacionaFácil",
                Version = "1.0.0",
                Author = "Thomas Edson Fregoneze",
                LastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        // Endpoint para checar status rápido do servidor
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                ServerTime = DateTime.Now,
                Backend = "Online",
                Database = "MongoDB Atlas",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            });
        }
    }
}
