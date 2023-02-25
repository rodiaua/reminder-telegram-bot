using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Context;
using ReminderTelegramBot.WebApp.Data.Entities;

namespace ReminderTelegramBot.WebApp.Data.Repository
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly ReminderDbContext dbContext;

        public ReminderRepository(ReminderDbContext reminderDbContext)
        {
            this.dbContext = reminderDbContext;
        }

        public Task AddReminderAsync(Reminder reminder)
        {
            dbContext.Reminders.Add(reminder);
            return SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Reminder>> GetRemindersAsync(IReadOnlyCollection<long> reminderKeys)
        {
            var reminders = await GetRemindersByKeysQuery(reminderKeys).ToListAsync();
            return reminders;
        }

        public async Task<IReadOnlyCollection<Reminder>> GetRemindersNoTrackingAsync(IReadOnlyCollection<long> reminderKeys)
        {
            var reminders = await GetRemindersByKeysQuery(reminderKeys).AsNoTracking().ToListAsync();
            return reminders;
        }

        public Task RemoveReminderAsync(IReadOnlyCollection<Reminder> reminders)
        {
            dbContext.Reminders.RemoveRange(reminders);
            return SaveChangesAsync();
        }

        public Task UpdateReminderAsync(Reminder reminder)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Reminder> GetRemindersByKeysQuery(IReadOnlyCollection<long> reminderKeys)
        {
            return dbContext.Reminders.Where(e => reminderKeys.Contains(e.ReminderKey));
        }

        private Task SaveChangesAsync()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
