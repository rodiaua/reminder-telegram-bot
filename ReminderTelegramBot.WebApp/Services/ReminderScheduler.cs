namespace ReminderTelegramBot.WebApp.Services
{
    public class ReminderScheduler
    {
        private IDictionary<long, ReminderTask> reminderTasks;
        private readonly ILogger logger;

        public ReminderScheduler(ILogger<ReminderScheduler> logger)
        {
            reminderTasks = new Dictionary<long, ReminderTask>();
            this.logger = logger;
        }

        public void ScheduleReminder(Func<Task> reminderFunction, long reminderKey, DateTimeOffset nextReminderUtcTime, bool repeatEveryDay = false)
        {
            var taskDaleyTimeSpan = nextReminderUtcTime - DateTimeOffset.UtcNow;

            var cts = new CancellationTokenSource();

            var reminderTask = new ReminderTask()
            {
                ReminderTaskCTS = cts,
                Task = Task.Run( async () =>
                {
                    await Task.Delay(taskDaleyTimeSpan, cts.Token);
                    await reminderFunction();
                    if(repeatEveryDay)
                    {
                        nextReminderUtcTime = nextReminderUtcTime.AddDays(1);
                        UpdateScheduledReminder(reminderFunction, reminderKey, nextReminderUtcTime, repeatEveryDay);
                    }
                    else
                    {
                        RemoveScheduledReminder(reminderKey);
                    }
                })
            };

            reminderTasks.Add(reminderKey, reminderTask);
            logger.LogInformation("Reminder - {reminderKey} task setup on {dateTime}", reminderKey, nextReminderUtcTime);
        }

        public void UpdateScheduledReminder(Func<Task> reminderAction, long reminderKey, DateTimeOffset nextReminderUtcTime, bool repeatEveryDay = false)
        {
            var reminderTask = reminderTasks.FirstOrDefault(t => t.Key == reminderKey).Value;
            if (reminderTask != null)
            {
                RemoveScheduledReminder(reminderKey);
            }
            ScheduleReminder(reminderAction, reminderKey, nextReminderUtcTime, repeatEveryDay);
        }

        public void RemoveScheduledReminder(long reminderKey)
        {
            var reminderTask = reminderTasks.FirstOrDefault(t => t.Key == reminderKey).Value;
            if (reminderTask != null)
            {
                reminderTask.ReminderTaskCTS.Cancel();
                reminderTasks.Remove(reminderKey);
                logger.LogInformation("Reminder - {reminderKey} task removed", reminderKey);
            }
        }

        private class ReminderTask
        {
            public CancellationTokenSource ReminderTaskCTS { get; set; }
            public Task Task { get; set; }
        }
    }

    
}
