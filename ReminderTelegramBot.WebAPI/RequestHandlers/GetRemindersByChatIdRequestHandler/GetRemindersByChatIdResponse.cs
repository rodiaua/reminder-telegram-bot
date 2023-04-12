namespace ReminderTelegramBot.WebAPI.RequestHandlers.GetRemindersRequestHandler
{
    public class GetRemindersByChatIdResponse
    {
        public long ReminderKey { get; set; }
        public string ReminderTitle { get; set; }
        public string ReminderDescription { get; set; }
        public long DateTimeUtcUnix { get; set; }
    }
}
