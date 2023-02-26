using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Entities;
using ReminderTelegramBot.WebApp.Data.Repository;
using ReminderTelegramBot.WebApp.Models;

namespace ReminderTelegramBot.WebApp.RequestHandlers.UpdateReminderRequestHandler
{
    public class UpdateReminderRequestHandler
    {
        private readonly IReminderRepository reminderRepository;
        private readonly ILogger logger;

        public UpdateReminderRequestHandler(IReminderRepository reminderRepository, ILogger<UpdateReminderRequestHandler> logger)
        {
            this.reminderRepository = reminderRepository;
            this.logger = logger;
        }

        public async Task<IActionResult> Handle(UpdateReminderRequest request)
        {
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
            reminder.UpdateDatabaseItem(request.ReminderTime, request.ReminderTitle, request.Description);
            try
            {
                await reminderRepository.UpdateReminderAsync(reminder);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Failed to update reminder - {reminderKey} for telegram chat - {chatKey}", request.ReminderKey, reminder.TelegramChatKey);
                return new ObjectResult(new DefaultResponse("Unexcpected error while updating reminder"))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            logger.LogInformation("Reminder - {reminderKey} successfuly updated", request.ReminderKey);
            return new OkResult();
        }
    }
}
