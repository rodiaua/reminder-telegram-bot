using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Context;
using ReminderTelegramBot.WebApp.Data.Entities;

namespace ReminderTelegramBot.WebApp.Data.Repository
{
    public class TelegramChatRepository : ITelegramChatRepository
    {
        private readonly ReminderDbContext dbContext;

        public TelegramChatRepository(ReminderDbContext reminderDbContext)
        {
            this.dbContext = reminderDbContext;
        }

        public Task AddTelegramChatAsync(TelegramChat telegramChat)
        {
            dbContext.TelegramChats.Add(telegramChat);
            return SaveChangesAsync();
        }

        public async Task<IReadOnlyDictionary<long, long>> GetTelegramChatIdsKeysAsync(IReadOnlyCollection<long> ids)
        {
            var idsKeys = await dbContext.TelegramChats
                .AsNoTracking()
                .Where(e => ids.Contains(e.TelegramChatId))
                .ToDictionaryAsync(x => x.TelegramChatId, x => x.TelegramChatKey);
            return idsKeys;
        }

        private Task SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
