using Microsoft.EntityFrameworkCore;
using ReminderTelegramBot.WebAPI.Data.Context;

namespace ReminderTelegramBot.WebAPI.Data.Repository
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

