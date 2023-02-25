using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Entities;

namespace ReminderTelegramBot.WebApp.Data.Context
{
    public class ReminderDbContext : DbContext
    {
        public ReminderDbContext(DbContextOptions<ReminderDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new RemindersConfiguration().Configure(modelBuilder.Entity<Reminder>());
            new TelegramChatConfiguration().Configure(modelBuilder.Entity<TelegramChat>());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<TelegramChat> TelegramChats { get; set; }
    }
}
