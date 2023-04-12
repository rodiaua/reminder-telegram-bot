using Microsoft.AspNetCore.Mvc;
using ReminderTelegramBot.WebAPI.Data.Repository;
using ReminderTelegramBot.WebAPI.Models;

namespace ReminderTelegramBot.WebAPI.RequestHandlers.GetRemindersRequestHandler
{
    public class GetRemindersByChatIdRequestHandler
    {
        private readonly IReminderRepository reminderRepository;
        private readonly ITelegramChatRepository telegramChatRepository;
        private readonly ILogger logger;

        public GetRemindersByChatIdRequestHandler(IReminderRepository reminderRepository,
            ITelegramChatRepository telegramChatRepository, ILogger<GetRemindersByChatIdRequestHandler> logger)
        {
            this.reminderRepository = reminderRepository;
            this.telegramChatRepository = telegramChatRepository;
            this.logger = logger;
        }

        public async Task<ActionResult<IReadOnlyCollection<GetRemindersByChatIdResponse>>> Hanlde(GetRemindersByChatIdRequest request)
        {
            var telegramChat = await telegramChatRepository.GetTelegramChatIdsKeysAsync(new long[] { request.TelegramChatId });
            if (!telegramChat.Any())
            {
                logger.LogInformation("The telegram chat - {id} was not found.", request.TelegramChatId);
                return new ObjectResult(new DefaultResponse("The telegram chat was not found"))
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            var reminders = await reminderRepository.GetRemindersByTelegramChatKeyNoTrackingAsync(telegramChat.FirstOrDefault().Value);

            var remindersRespnose = reminders.Select(r => new GetRemindersByChatIdResponse()
            {
                DateTimeUtcUnix = r.ReminderTimeUtc.ToUnixTimeMilliseconds(),
                ReminderDescription = r.ReminderDescription,
                ReminderKey = r.ReminderKey,
                ReminderTitle = r.ReminderTitle
            });

            return new OkObjectResult(remindersRespnose);
        }
    }
}
