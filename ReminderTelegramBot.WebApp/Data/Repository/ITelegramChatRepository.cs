using ReminderTelegramBot.WebApp.Data.Entities;

namespace ReminderTelegramBot.WebApp.Data.Repository
{
    public interface ITelegramChatRepository
    {
        Task AddTelegramChatAsync(TelegramChat telegramChat);
        Task<IReadOnlyDictionary<long, long>> GetTelegramChatIdsKeysAsync(IReadOnlyCollection<long> ids);
    }
}
