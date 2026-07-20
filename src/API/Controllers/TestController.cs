using AuthService.Application.Common.Interfaces;
using Shared.Contracts.Events;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IIntegrationEventService _eventService;

        public TestController(IIntegrationEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Hello word!");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Application.DTOs.RegisterRequest request)
        {
            var userId = Guid.NewGuid();

            var userRegisteredEvent = new UserRegisteredEvent
            {
                UserId = userId,
                Email = request.Email,
                FullName = request.FullName,
                CreatedAt = DateTime.UtcNow
            };

            await _eventService.PublishAsync(userRegisteredEvent);

            return Ok(new { Message = "Đăng ký thành công!", UserId = userId });
        }

    }
}
