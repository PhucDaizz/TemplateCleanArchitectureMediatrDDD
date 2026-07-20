using Shared.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.BackgroundJobs.Consumer
{
    public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly ILogger<UserRegisteredConsumer> _logger;

        public UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation("===> [RabbitMQ Received] Đã nhận sự kiện UserRegisteredEvent cho User: {Email} (ID: {UserId})",
                message.Email, message.UserId);

            await Task.Delay(500);

            _logger.LogInformation("===> [Email Service] Đã gửi email chào mừng thành công cho {FullName}!", message.FullName);
        }
    }
}
