using ReminderTelegramBot.WebAPI.Data.Entities;

namespace ReminderTelegramBot.WebAPI.Data.Repository
{
    public interface ITelegramChatRepository
    {
        Task AddTelegramChatAsync(TelegramChat telegramChat);
        Task<IReadOnlyDictionary<long, long>> GetTelegramChatIdsKeysAsync(IReadOnlyCollection<long> ids);
    }
}
