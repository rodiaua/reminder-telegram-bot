namespace ReminderTelegramBot.WebApp.Data.Entities
{
    public class Reminder
    {
        private Reminder()
        {

        }
        public long ReminderKey { get; private set;}
        public long TelegramChatKey { get; private set; }
        public DateTimeOffset RemindTimeUtc { get; private set; }
        public DateTimeOffset RemindTimeLocal { get; private set; }
        public string ReminderTitle { get; private set; }
        public string ReminderDescription { get; private set; }

        public TelegramChat TelegramChat { get; private set; }

        public static Reminder BuildDatabaseItem(long telegramChatKey, DateTimeOffset remindTime,
            string reminderTitle, string reminderDescription = null)
        {
            return new Reminder()
            {
                ReminderDescription = reminderDescription,
                ReminderTitle = reminderTitle,
                TelegramChatKey = telegramChatKey,
                RemindTimeUtc = remindTime.UtcDateTime,
                RemindTimeLocal = remindTime.LocalDateTime
            };
        }
    }
}
