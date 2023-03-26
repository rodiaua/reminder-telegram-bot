namespace ReminderTelegramBot.WebApp.RequestHandlers.AddReminderRequestHandler
{
    public class AddReminderReqeust
    {
        public long TelegramChatId { get; set; }
        public DateTimeOffset ReminderTime { get; set; }
        public string ReminderTitle { get; set; }
        public string ReminderDescription { get; set; }
        public bool RepeatEveryDay { get; set; }
    }

    
}
