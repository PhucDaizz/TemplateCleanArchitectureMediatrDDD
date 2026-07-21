using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Events;

namespace AuthService.Infrastructure.BackgroundJobs.Consumer
{
    public class ArticlePublishedConsumer : IConsumer<ArticlePublishedIntegrationEvent>
    {
        private readonly ILogger<ArticlePublishedConsumer> _logger;

        public ArticlePublishedConsumer(ILogger<ArticlePublishedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ArticlePublishedIntegrationEvent> context)
        {
            var eventData = context.Message;

            _logger.LogInformation("===> [Notification Service] Nhận được tin nhắn bài viết mới vừa xuất hiện: {ArticleId}", eventData.ArticleId);

            // 1. Giả lập query danh sách Subscriber Active từ Database
            var activeSubscribers = await GetActiveSubscribersMockAsync();

            // 2. Giả lập delay xử lý gửi email hàng loạt (SendGrid / SMTP API)
            await Task.Delay(500);

            // 3. In Log kết quả đúng định dạng đề bài
            _logger.LogInformation(
                "[Notification Service] Đã phát thông báo bài viết '{Title}' ({TotalSections} phần) tới {SubscriberCount} Subscribers!",
                eventData.Title,
                eventData.TotalSections,
                activeSubscribers.Count
            );

            // (Optional) Log chi tiết vài email nhận được
            foreach (var email in activeSubscribers.Take(3))
            {
                _logger.LogDebug("--> Đã gửi mail tới: {Email}", email);
            }
        }

        /// <summary>
        /// Giả lập dữ liệu 50 Subscriber active để phục vụ test UI/Console
        /// </summary>
        private static Task<List<string>> GetActiveSubscribersMockAsync()
        {
            var mockList = Enumerable.Range(1, 50)
                .Select(i => $"subscriber_{i}@gmail.com")
                .ToList();

            return Task.FromResult(mockList);
        }
    }
}
