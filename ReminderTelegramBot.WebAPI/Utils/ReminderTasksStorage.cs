namespace ReminderTelegramBot.WebAPI.Utils
{
    public static class ReminderTasksStorage
    {
        //long - ReminderKey
        public static IDictionary<long, ReminderTask> ReminderTasks { get; } = new Dictionary<long, ReminderTask>();

        public class ReminderTask
        {
            public CancellationTokenSource ReminderTaskCTS { get; set; }
            public Task Task { get; set; }
        }
    }


}
