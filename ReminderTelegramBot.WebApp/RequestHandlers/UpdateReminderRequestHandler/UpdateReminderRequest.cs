namespace ReminderTelegramBot.WebApp.RequestHandlers.UpdateReminderRequestHandler
{
    public class UpdateReminderRequest
    {
        public long ReminderKey { get; set; }
        public string ReminderTitle { get; set; }
        public string Description { get; set; }
        public DateTimeOffset ReminderTime { get; set; }
        public bool RepeatEveryDay { get; set; }
    }
}
