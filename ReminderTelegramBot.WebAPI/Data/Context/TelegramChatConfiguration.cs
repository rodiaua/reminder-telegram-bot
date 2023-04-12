using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReminderTelegramBot.WebAPI.Data.Entities;

namespace ReminderTelegramBot.WebAPI.Data.Context
{
    public class TelegramChatConfiguration : IEntityTypeConfiguration<TelegramChat>
    {
        public void Configure(EntityTypeBuilder<TelegramChat> builder)
        {
            builder.HasKey(e => e.TelegramChatKey);
            builder.Property(e => e.TelegramChatKey).ValueGeneratedOnAdd();
            builder.HasIndex(e => e.TelegramChatId).IsUnique();
        }
    }
}
