using ReminderTelegramBot.WebAPI.Client.Models;

namespace ReminderTelegramBot.WebAPI.Client
{
    public interface IReminderTelegramBotWebApiClient
    {
        public Task RegisterTelegramChat(AddTelegramChatRequest telegramChat);
    }
}