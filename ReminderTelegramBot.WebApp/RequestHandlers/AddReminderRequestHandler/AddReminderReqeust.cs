namespace ReminderTelegramBot.WebApp.RequestHandlers.AddReminderRequestHandler
{
    public class AddReminderReqeust
    {
        public long TelegramChatId { get; set; }
        public DateTimeOffset RemindTime { get; set; }
        public string ReminderTitle { get; set; }
        public string ReminderDescription { get; set; }
    }

    
}
