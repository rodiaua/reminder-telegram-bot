using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Entities;
using ReminderTelegramBot.WebApp.Data.Repository;
using ReminderTelegramBot.WebApp.Models;
using ReminderTelegramBot.WebApp.Services;
using ReminderTelegramBot.WebApp.Utils;

namespace ReminderTelegramBot.WebApp.RequestHandlers.UpdateReminderRequestHandler
{
    public class UpdateReminderRequestHandler
    {
        private readonly IReminderRepository reminderRepository;
        private readonly ILogger logger;
        private readonly ReminderScheduler reminderScheduler;
        private readonly ReminderService reminderService;

        public UpdateReminderRequestHandler(IReminderRepository reminderRepository,
            ILogger<UpdateReminderRequestHandler> logger,
            ReminderScheduler reminderScheduler,
            ReminderService reminderService)
        {
            this.reminderRepository = reminderRepository;
            this.logger = logger;
            this.reminderScheduler = reminderScheduler;
            this.reminderService = reminderService;
        }

        public async Task<IActionResult> Handle(UpdateReminderRequest request)
        {
            if (ReminderTimeExpired() && !request.RepeatEveryDay)
            {
                logger.LogInformation("Unable to update reminder - {reminderKey} with expired time - {utcDateTime}",
                    request.ReminderKey, request.ReminderTime.UtcDateTime);
                return new ObjectResult(new DefaultResponse("Reminder time is not valid"))
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            //if reminder is expired but should be repeated every day then set up next reminder one day later
            if (ReminderTimeExpired() && request.RepeatEveryDay)
            {
                request.ReminderTime.AddDays(1);
            }

            var reminders = await reminderRepository.GetRemindersNoTrackingAsync(new long[] { request.ReminderKey });
            if (!reminders.Any())
            {
                logger.LogInformation("The reminder - {key} was not found.", request.ReminderKey);
                return new ObjectResult(new DefaultResponse("The reminder doesn't exist"))
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            var reminder = reminders.FirstOrDefault();
            reminder.UpdateDatabaseItem(request.ReminderTime, request.ReminderTitle, request.Description, request.RepeatEveryDay);
            try
            {
                await reminderRepository.UpdateReminderAsync(reminder);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Failed to update reminder - {reminderKey} for telegram chat - {chatKey}", request.ReminderKey, reminder.TelegramChatKey);
                return new ObjectResult(new DefaultResponse("Unexpected error while updating reminder"))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            reminderScheduler.UpdateScheduledReminder(() =>
                reminderService.SendReminderToTelegramBot(reminder.TelegramChat.TelegramChatId, request.ReminderTitle, request.Description),
                 reminder.ReminderKey,
                 request.ReminderTime.UtcDateTime,
                 request.RepeatEveryDay);

            logger.LogInformation("Reminder - {reminderKey} successfully updated", request.ReminderKey);
            return new OkResult();

            bool ReminderTimeExpired()
            {
                return DateTimeOffset.UtcNow >= request.ReminderTime.UtcDateTime;
            }
        }
    }
}
