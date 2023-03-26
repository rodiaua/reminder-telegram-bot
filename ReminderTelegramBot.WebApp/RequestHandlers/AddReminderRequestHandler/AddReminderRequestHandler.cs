using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Entities;
using ReminderTelegramBot.WebApp.Data.Repository;
using ReminderTelegramBot.WebApp.Models;
using ReminderTelegramBot.WebApp.Services;

namespace ReminderTelegramBot.WebApp.RequestHandlers.AddReminderRequestHandler
{
    public class AddReminderRequestHandler
    {
        private readonly ILogger logger;
        private readonly IReminderRepository reminderRepository;
        private readonly ITelegramChatRepository telegramChatRepository;
        private readonly ReminderScheduler reminderScheduler;
        private readonly ReminderService reminderService;

        public AddReminderRequestHandler(ILogger<AddReminderRequestHandler> logger,
            IReminderRepository reminderRepository,
            ITelegramChatRepository telegramChatRepository,
            ReminderScheduler reminderScheduler,
            ReminderService reminderService)
        {
            this.logger = logger;
            this.reminderRepository = reminderRepository;
            this.telegramChatRepository = telegramChatRepository;
            this.reminderScheduler = reminderScheduler;
            this.reminderService = reminderService;
        }

        public async Task<IActionResult> Handle(AddReminderReqeust request)
        {
            if (ReminderTimeExpired() && !request.RepeatEveryDay)
            {
                logger.LogInformation("Unable to set timer for the expired reminder - {dateTimeUTC}. Now - {nowDateTimeUTC}. Telegram chat id - {chatId}",
                    request.ReminderTime.UtcDateTime, DateTimeOffset.UtcNow, request.TelegramChatId);
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

            var telegramChatIdsKeys = await telegramChatRepository.GetTelegramChatIdsKeysAsync(new long[] { request.TelegramChatId });
            if (!telegramChatIdsKeys.Any())
            {
                logger.LogInformation("Telegram chat with id - {id} was not found.", request.TelegramChatId);
                return new ObjectResult(new DefaultResponse("Telegram chat not found"))
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var telegramChatKey = telegramChatIdsKeys[request.TelegramChatId];

            var reminder = Reminder.BuildDatabaseItem(telegramChatKey, request.ReminderTime, request.ReminderTitle,
                request.ReminderDescription, request.RepeatEveryDay);
            long reminderKey = default;
            try
            {
               reminderKey = await reminderRepository.AddReminderAsync(reminder);
            }
            catch (DbUpdateException exception)
            {
                logger.LogError(exception, "Failed to save reminder for telegram chat id - {id}", request.TelegramChatId);
                return new ObjectResult(new DefaultResponse("Unexpected error during saving reminder"))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            logger.LogInformation("Reminder successfuly added for telegram chat - {id}", request.TelegramChatId);

            reminderScheduler.ScheduleReminder(() => 
                reminderService.SendReminderToTelegramBot(request.TelegramChatId, reminder.ReminderTitle, reminder.ReminderDescription),
                reminderKey, reminder.ReminderTimeUtc, request.RepeatEveryDay);

            return new OkResult();

            bool ReminderTimeExpired()
            {
                return DateTimeOffset.UtcNow >= request.ReminderTime.UtcDateTime;
            }
        }


    }


}
