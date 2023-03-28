namespace ReminderTelegramBot.WebApp.Services
{
    public class ReminderService
    {
        private readonly ILogger logger;

        public ReminderService(ILogger<ReminderService> logger)
        {
            this.logger = logger;
        }

        public Task SendReminderToTelegramBot(long telegramChatId, string reminderTitle, string reminderDescription)
        {
            logger.LogInformation("Reminder info sent to Telegram Bot to chat - {telegramChatId}", telegramChatId);
            return Task.CompletedTask;
        }
    }
}
