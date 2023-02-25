using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Entities;
using ReminderTelegramBot.WebApp.Data.Repository;
using ReminderTelegramBot.WebApp.Models;

namespace ReminderTelegramBot.WebApp.RequestHandlers.AddReminderRequestHandler
{
    public class AddReminderRequestHandler
    {
        private readonly ILogger logger;
        private readonly IReminderRepository reminderRepository;
        private readonly ITelegramChatRepository telegramChatRepository;

        public AddReminderRequestHandler(ILogger<AddReminderRequestHandler> logger,
            IReminderRepository reminderRepository,
            ITelegramChatRepository telegramChatRepository)
        {
            this.logger = logger;
            this.reminderRepository = reminderRepository;
            this.telegramChatRepository = telegramChatRepository;
        }

        public async Task<IActionResult> Handle(AddReminderReqeust request)
        {
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

            var reminder = Reminder.BuildDatabaseItem(telegramChatKey, request.RemindTime, request.ReminderTitle, request.ReminderDescription);
            try
            {
                await reminderRepository.AddReminderAsync(reminder);
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
            return new OkResult();
        }
    }


}
