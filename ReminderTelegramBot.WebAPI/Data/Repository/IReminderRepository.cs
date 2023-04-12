using ReminderTelegramBot.WebAPI.Data.Entities;

namespace ReminderTelegramBot.WebAPI.Data.Repository
{
    public interface IReminderRepository : IDisposable
    {
        Task<long> AddReminderAsync(Reminder reminder);
        Task RemoveReminderAsync(IReadOnlyCollection<Reminder> reminders);
        Task UpdateReminderAsync(Reminder reminder);
        Task<IReadOnlyCollection<Reminder>> GetRemindersAsync(IReadOnlyCollection<long> reminderKeys);
        Task<IReadOnlyCollection<Reminder>> GetRemindersNoTrackingAsync(IReadOnlyCollection<long> reminderKeys);
        Task<IReadOnlyCollection<Reminder>> GetRemindersByTelegramChatKeyNoTrackingAsync(long telegramChatKey);
    }
}
