using ReminderTelegramBot.WebAPI.Data.Repository;
using ReminderTelegramBot.WebAPI.Utils.EventHandlers.ReminderTask;
using static ReminderTelegramBot.WebAPI.Utils.ReminderTasksStorage;

namespace ReminderTelegramBot.WebAPI.Utils
{
    public delegate Task ReminderTaskTimeUpdatedEvent(ReminderTaskUpdatedArgs args);
    public delegate Task ReminderTaskRemovedEvent(long reminderKey);

    public class ReminderScheduler
    {
        private readonly ILogger logger;
        private readonly ReminderTaskTimeUpdatedEvent reminderTaskTimeUpdated;
        private readonly ReminderTaskRemovedEvent reminderTaskRemoved;

        public ReminderScheduler(ILogger<ReminderScheduler> logger, ReminderTaskEventsHandler reminderTaskEventsHandler)
        {
            reminderTaskTimeUpdated += reminderTaskEventsHandler.HandleTaskTimerUpdatedEvent;
            reminderTaskRemoved += reminderTaskEventsHandler.HandleTaskRemovedEvent;
            this.logger = logger;
        }

        public void ScheduleReminder(Func<Task> reminderFunction, long reminderKey, DateTimeOffset nextReminderUtcTime, bool repeatEveryDay = false)
        {
            var taskDaleyTimeSpan = nextReminderUtcTime - DateTimeOffset.UtcNow;

            var cts = new CancellationTokenSource();

            var reminderTask = new ReminderTask()
            {
                ReminderTaskCTS = cts,
                Task = Task.Run(async () =>
                {
                    await Task.Delay(taskDaleyTimeSpan, cts.Token);
                    await reminderFunction();

                    if (repeatEveryDay)
                    {
                        //adding one day to previous reminder time
                        nextReminderUtcTime = nextReminderUtcTime.AddDays(1);

                        UpdateScheduledReminder(reminderFunction, reminderKey, nextReminderUtcTime, repeatEveryDay);

                        await reminderTaskTimeUpdated(new ReminderTaskUpdatedArgs(reminderKey, nextReminderUtcTime));
                    }
                    else
                    {
                        RemoveScheduledReminder(reminderKey);
                        await reminderTaskRemoved(reminderKey); 
                    }
                })
            };

            ReminderTasks.Add(reminderKey, reminderTask);
            logger.LogInformation("Reminder - {reminderKey} task setup on {dateTime}", reminderKey, nextReminderUtcTime);
        }

        public void UpdateScheduledReminder(Func<Task> reminderAction, long reminderKey, DateTimeOffset nextReminderUtcTime, bool repeatEveryDay = false)
        {
            var reminderTask = ReminderTasks.FirstOrDefault(t => t.Key == reminderKey).Value;
            if (reminderTask != null)
            {
                RemoveScheduledReminder(reminderKey);
            }
            ScheduleReminder(reminderAction, reminderKey, nextReminderUtcTime, repeatEveryDay);
        }

        public void RemoveScheduledReminder(long reminderKey)
        {
            var reminderTask = ReminderTasks.FirstOrDefault(t => t.Key == reminderKey).Value;
            if (reminderTask != null)
            {
                reminderTask.ReminderTaskCTS.Cancel();
                ReminderTasks.Remove(reminderKey);
                logger.LogInformation("Reminder - {reminderKey} task removed", reminderKey);
            }
        }

    }


}
