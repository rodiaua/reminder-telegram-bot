namespace ReminderTelegramBot.WebAPI.Data.Entities
{
    public class TelegramChat
    {
        private TelegramChat()
        {

        }
        public long TelegramChatKey { get; private set; }
        public long TelegramChatId { get; private set; }

        public IReadOnlyCollection<Reminder> Reminders { get; set; }

        public static TelegramChat BuildDatabaseItem(long telegramChatId)
        {
            return new TelegramChat()
            {
                TelegramChatId = telegramChatId
            };
        }
    }
}
