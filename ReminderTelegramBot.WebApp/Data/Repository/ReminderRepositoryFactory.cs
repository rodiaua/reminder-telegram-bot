using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebApp.Data.Context;

namespace ReminderTelegramBot.WebApp.Data.Repository
{
    public class ReminderRepositoryFactory : IReminderRepositoryFactory
    {
        private readonly IDbContextFactory<ReminderDbContext> dbContextFactory;

        public ReminderRepositoryFactory(IDbContextFactory<ReminderDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public IReminderRepository CreateReminderRepository()
        {
            return new ReminderRepository(dbContextFactory.CreateDbContext());
        }
    }

    
}

