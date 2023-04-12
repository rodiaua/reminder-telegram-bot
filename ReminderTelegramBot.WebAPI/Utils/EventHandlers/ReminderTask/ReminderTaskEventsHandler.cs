using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebAPI.Data.Context;
using ReminderTelegramBot.WebAPI.Data.Entities;
using ReminderTelegramBot.WebAPI.Data.Repository;

namespace ReminderTelegramBot.WebAPI.Utils.EventHandlers.ReminderTask
{
    public class ReminderTaskEventsHandler
    {
        private readonly ILogger<ReminderTaskEventsHandler> logger;
        private readonly IReminderRepositoryFactory reminderRepositoryFactory;

        public ReminderTaskEventsHandler(ILogger<ReminderTaskEventsHandler> logger, IReminderRepositoryFactory reminderRepositoryFactory)
        {
            this.logger = logger;
            this.reminderRepositoryFactory = reminderRepositoryFactory;
        }

        public async Task HandleTaskTimerUpdatedEvent(ReminderTaskUpdatedArgs args)
        {
            using (var reminderRepository = reminderRepositoryFactory.CreateReminderRepository())
            {
                var getRemindersResult = await reminderRepository.GetRemindersAsync(new long[] { args.ReminderKey });

                var reminder = getRemindersResult.FirstOrDefault();

                if (reminder is null)
                {
                    logger.LogError("The reminder update is failed as the reminder was not found in db");
                }
                else
                {
                    reminder.UpdateDatabaseItem(remindTime: args.NewTime);

                    try
                    {
                        await reminderRepository.UpdateReminderAsync(reminder);
                    }
                    catch (DbUpdateException ex)
                    {
                        logger.LogError(ex, "Failed to update reminder - {reminderKey} for telegram chat - {chatKey}", reminder.ReminderKey, reminder.TelegramChatKey);
                    }

                }
            }
        }

        public async Task HandleTaskRemovedEvent(long reminderKey)
        {
            using (var reminderRepository = reminderRepositoryFactory.CreateReminderRepository())
            {
                var getRemindersResult = await reminderRepository.GetRemindersAsync(new long[] { reminderKey });

                var reminder = getRemindersResult.FirstOrDefault();

                if (reminder == null)
                {
                    logger.LogError("The reminder removal is failed as the reminder was not found in db");
                }
                else
                {
                    try
                    {
                        await reminderRepository.RemoveReminderAsync(new Reminder[] { reminder });
                    }
                    catch (DbUpdateException ex)
                    {
                        logger.LogError(ex, "Failed to remove reminder - {reminderKey} for telegram chat - {chatKey}", reminder.ReminderKey, reminder.TelegramChatKey);
                    }
                }
            }
        }
    }

    public record ReminderTaskUpdatedArgs(long ReminderKey, DateTimeOffset NewTime);
}
