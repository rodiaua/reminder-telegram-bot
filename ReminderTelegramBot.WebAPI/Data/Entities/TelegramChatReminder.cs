namespace ReminderTelegramBot.WebAPI.Data.Entities
{
    public class TelegramChatReminder
    {
        public long TelegramChatKey { get; set; }
        public long ReminderKey { get; set; }

        public TelegramChat TelegramChat { get; set; }
        public Reminder Reminder { get; set; }
    }
}
