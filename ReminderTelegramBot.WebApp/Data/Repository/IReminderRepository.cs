using ReminderTelegramBot.WebApp.Data.Entities;

namespace ReminderTelegramBot.WebApp.Data.Repository
{
    public interface IReminderRepository
    {
        Task AddReminderAsync(Reminder reminder);
        Task RemoveReminderAsync(IReadOnlyCollection<Reminder> reminders);
        Task UpdateReminderAsync(Reminder reminder);
        Task<IReadOnlyCollection<Reminder>> GetRemindersAsync(IReadOnlyCollection<long> reminderKeys);
        Task<IReadOnlyCollection<Reminder>> GetRemindersNoTrackingAsync(IReadOnlyCollection<long> reminderKeys);
        Task<IReadOnlyCollection<Reminder>> GetRemindersByTelegramChatKeyNoTrackingAsync(long telegramChatKey);
    }
}
