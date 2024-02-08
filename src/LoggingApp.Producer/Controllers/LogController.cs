using LoggingApp.Producer.Interfaces;
using LoggingApp.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingApp.Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpPost]
        public async Task<IActionResult> PushLog(int countLoop)
        {
            try
            {
                LogEntity logEntity = new()
                {
                    ClientId = "Order App",
                };

                for (int i = 0; i < countLoop; i++)
                {
                    logEntity.Id = Guid.NewGuid().ToString();
                    logEntity.MessageId = Guid.NewGuid().ToString();
                    logEntity.Message = $"Message ke {i}";
                    logEntity.CreatedDate = DateTime.Now;
                    
                    await _logService.ProduceAsync(logEntity);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
