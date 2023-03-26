﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Entities;
using ReminderTelegramBot.WebApp.Data.Repository;
using ReminderTelegramBot.WebApp.Models;
using ReminderTelegramBot.WebApp.Services;

namespace ReminderTelegramBot.WebApp.RequestHandlers.RemoveRminderRequestHandler
{
    public class RemoveReminderRequestHandler
    {
        private readonly ILogger logger;
        private readonly IReminderRepository reminderRepository;
        private readonly ReminderScheduler reminderScheduler;

        public RemoveReminderRequestHandler(ILogger<RemoveReminderRequestHandler> logger,
            IReminderRepository reminderRepository,
            ReminderScheduler reminderScheduler)
        {
            this.logger = logger;
            this.reminderRepository = reminderRepository;
            this.reminderScheduler = reminderScheduler;
        }

        public async Task<IActionResult> Handle(RemoveReminderRequest request)
        {
            var getRemindersResult = await reminderRepository.GetRemindersNoTrackingAsync(new long[] { request.ReminderKey });
            if (!getRemindersResult.Any())
            {
                logger.LogInformation("The reminder - {key} was not found.", request.ReminderKey);
                return new ObjectResult(new DefaultResponse("The reminder has been already removed or doesn't exist"))
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            var reminder = getRemindersResult.FirstOrDefault();
            try
            {
                await reminderRepository.RemoveReminderAsync(new Reminder[] { reminder });
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Failed to remove reminder - {reminderKey} for telegram chat - {chatKey}", request.ReminderKey, reminder.TelegramChatKey);
                return new ObjectResult(new DefaultResponse("Unexcpected error while removing reminder"))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            reminderScheduler.RemoveScheduledReminder(request.ReminderKey);

            logger.LogInformation("Reminder - {reminderKey} successfuly removed", request.ReminderKey);
            return new OkResult();
        }
    }
}
