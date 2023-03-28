﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<long> AddReminderAsync(Reminder reminder)
        {
            var result = dbContext.Reminders.Add(reminder);
            await SaveChangesAsync();
            return result.Entity.ReminderKey;
        }

        public async Task<IReadOnlyCollection<Reminder>> GetRemindersAsync(IReadOnlyCollection<long> reminderKeys)
        {
            var reminders = await GetRemindersByKeysQuery(reminderKeys).ToListAsync();
            return reminders;
        }

        public async Task<IReadOnlyCollection<Reminder>> GetRemindersByTelegramChatKeyNoTrackingAsync(long telegramChatKey)
        {
            var reminders = await dbContext.Reminders.AsNoTracking().Where(e => e.TelegramChatKey == telegramChatKey).ToListAsync();
            return reminders;
        }

        public async Task<IReadOnlyCollection<Reminder>> GetRemindersNoTrackingAsync(IReadOnlyCollection<long> reminderKeys)
        {
            var reminders = await GetRemindersByKeysQuery(reminderKeys).AsNoTracking().Include(e => e.TelegramChat).ToListAsync();
            return reminders;
        }

        public Task RemoveReminderAsync(IReadOnlyCollection<Reminder> reminders)
        {
            dbContext.Reminders.RemoveRange(reminders);
            return SaveChangesAsync();
        }

        public Task UpdateReminderAsync(Reminder reminder)
        {
            dbContext.Reminders.Update(reminder);
            return SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
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
