using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebAPI.Data.Entities;
using ReminderTelegramBot.WebAPI.Data.Repository;
using ReminderTelegramBot.WebAPI.Models;

namespace ReminderTelegramBot.WebAPI.RequestHandlers.AddTelegramChatRequestHandler
{
    public class AddTelegramChatRequestHandler
    {
        private readonly ILogger logger;
        private readonly ITelegramChatRepository telegramChatRepository;

        public AddTelegramChatRequestHandler(ILogger<AddTelegramChatRequestHandler> logger, ITelegramChatRepository telegramChatRepository)
        {
            this.logger = logger;
            this.telegramChatRepository = telegramChatRepository;
        }


        public async Task<IActionResult> Handle(AddTelegramChatRequest request)
        {
            try
            {
                await telegramChatRepository.AddTelegramChatAsync(TelegramChat.BuildDatabaseItem(request.TelegramChatId));
                throw new DbUpdateException();
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Failed to save telegram chat id {id}", request.TelegramChatId);
                return new ObjectResult(new DefaultResponse("Unexpected error during saving telegram chat"))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            logger.LogInformation("Telegram chat - {id} successfuly saved", request.TelegramChatId);
            return new OkResult();
        }
    }
}
