using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReminderTelegramBot.WebApp.Data.Entities;

namespace ReminderTelegramBot.WebApp.Data.Context
{
    public class TelegramChatConfiguration : IEntityTypeConfiguration<TelegramChat>
    {
        public void Configure(EntityTypeBuilder<TelegramChat> builder)
        {
            builder.HasKey(e => e.TelegramChatKey);
            builder.Property(e => e.TelegramChatKey).ValueGeneratedOnAdd();
        }
    }
}
