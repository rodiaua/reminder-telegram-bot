using ReminderTelegramBot.WebAPI.Client;
using ReminderTelegramBot.WebAPI.Client.Models;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ReminderTelegramBot.Client
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger logger;

        public UpdateHandler(IServiceScopeFactory serviceScopeFactory, ILogger<UpdateHandler> logger)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            var chatId = message.Chat.Id;
            if (message is not null && UserStartedBot(message.Text))
            {
                logger.LogInformation("User {username} - {userId} started bot on chat - {chatId}", message.From.Username, message.From.Id, chatId);
                await SendMessage(botClient, Messages.Greatings.StartGreating, chatId);
                await RegisterTelegramChat(chatId);
            }
        }

        private Task RegisterTelegramChat(long chatId)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var client = scope.ServiceProvider.GetRequiredService<IReminderTelegramBotWebApiClient>();
            return client.RegisterTelegramChat(new AddTelegramChatRequest { TelegramChatId = chatId });
        }

        private Task SendMessage(ITelegramBotClient client, string text, long chatId)
        {
            return client.SendTextMessageAsync(new ChatId(chatId), text);
        }

        private bool UserStartedBot(string message)
        {
            return message.Equals(Commands.Start, StringComparison.InvariantCultureIgnoreCase);
        }


    }

    public class Commands
    {
        public const string Start = "/start";
    }
}