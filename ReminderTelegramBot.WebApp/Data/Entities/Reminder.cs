namespace ReminderTelegramBot.WebApp.Data.Entities
{
    public class Reminder
    {
        private Reminder()
        {

        }
        public long ReminderKey { get; private set; }
        public long TelegramChatKey { get; private set; }
        public DateTimeOffset ReminderTimeUtc { get; private set; }
        public DateTimeOffset ReminderTimeLocal { get; private set; }
        public string ReminderTitle { get; private set; }
        public string ReminderDescription { get; private set; }
        public bool RepeatEveryDay { get; private set; }

        public TelegramChat TelegramChat { get; private set; }

        public static Reminder BuildDatabaseItem(long telegramChatKey, DateTimeOffset remindTime,
            string reminderTitle, string reminderDescription, bool repeatEveryDay)
        {
            return new Reminder()
            {
                ReminderDescription = reminderDescription,
                ReminderTitle = reminderTitle,
                TelegramChatKey = telegramChatKey,
                ReminderTimeUtc = remindTime.UtcDateTime,
                ReminderTimeLocal = remindTime.LocalDateTime,
                RepeatEveryDay = repeatEveryDay
            };
        }


        public void UpdateDatabaseItem(DateTimeOffset remindTime)
        {
            ReminderTimeUtc = remindTime.UtcDateTime;
            ReminderTimeLocal = remindTime.LocalDateTime;
        }

        public void UpdateDatabaseItem(DateTimeOffset remindTime,
            string reminderTitle, string reminderDescription, bool repeatEveryDay)
        {
            ReminderDescription = reminderDescription;
            ReminderTitle = reminderTitle;
            ReminderTimeUtc = remindTime.UtcDateTime;
            ReminderTimeLocal = remindTime.LocalDateTime;
            RepeatEveryDay = repeatEveryDay;
        }

        
    }
}
