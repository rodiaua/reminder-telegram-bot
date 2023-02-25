using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReminderTelegramBot.WebApp.Data.Entities;

namespace ReminderTelegramBot.WebApp.Data.Context
{
    public class RemindersConfiguration : IEntityTypeConfiguration<Reminder>
    {
        public void Configure(EntityTypeBuilder<Reminder> builder)
        {
            builder.HasKey(e => e.ReminderKey);
            builder.HasOne(e => e.TelegramChat).WithMany(e => e.Reminders).HasForeignKey(e => e.TelegramChatKey);
        }
    }
}
