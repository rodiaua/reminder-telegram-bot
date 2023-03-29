using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using ReminderTelegramBot.WebApp.Data.Context;
using ReminderTelegramBot.WebApp.Data.Entities;
using ReminderTelegramBot.WebApp.Data.Repository;
using System;

namespace ReminderTelegramBot.WebApp.Tests.Database
{
    [TestFixture]
    public class TelegramChatRepositoryTests
    {
        private readonly TelegramChatRepository repository;
        private readonly ReminderDbContext reminderDbContext;
        private int telegramChatRecordsCount;
        private long telegramChatId = 1;

        public TelegramChatRepositoryTests()
        {
            reminderDbContext = new ReminderDbContext(
                new DbContextOptionsBuilder<ReminderDbContext>()
                .UseSqlServer("Data Source = (localDB)\\MSSQLLocalDB; Initial Catalog = DBIntegrationTests; Integrated Security=True;").Options);
            repository = new TelegramChatRepository(reminderDbContext);
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            await reminderDbContext.Database.EnsureCreatedAsync();
        }

        [SetUp]
        public async Task Setup()
        {
            telegramChatRecordsCount = await reminderDbContext.TelegramChats.CountAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTeardown()
        {
            await reminderDbContext.Database.EnsureDeletedAsync();
            await reminderDbContext.DisposeAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            var telegramChats = await reminderDbContext.TelegramChats.ToListAsync();
            reminderDbContext.TelegramChats.RemoveRange(telegramChats);
            await reminderDbContext.SaveChangesAsync();
        }

        [Test]
        public async Task test_AddTelegramChatAsync_adds_telegram_chat_to_database()
        {
            //arrange
            var telegramChat = GenerateTelegramChats(1);

            //act
            await repository.AddTelegramChatAsync(telegramChat.First());
            var currentTelegramChatTableCount = await reminderDbContext.TelegramChats.CountAsync();

            //assert
            currentTelegramChatTableCount.Should().Be(telegramChatRecordsCount + 1);
        }

        [Test]
        public async Task test_GetTelegramChatIdsKeysAsync_returns_id_key_pars_for()
        {
            //arrange
            var numberOfEntities = 10;
            var telegramChatsToAdd = GenerateTelegramChats(numberOfEntities).ToList();
            var idsToGetKeys = telegramChatsToAdd.Select(x => x.TelegramChatId).ToList();

            //act
            await SeedTelegramChatsTableWith(telegramChatsToAdd);
            var result = await repository.GetTelegramChatIdsKeysAsync(idsToGetKeys);

            //assert
            result.Count.Should().Be(numberOfEntities);
            result.Should().HaveSameCount(idsToGetKeys);
            result.ToList().ForEach(kvp =>
            {
                idsToGetKeys.Should().Contain(kvp.Key);
                kvp.Value.Should().BeGreaterThan(0);
            });
            result.Values.Should().OnlyHaveUniqueItems();
            result.Keys.Should().OnlyHaveUniqueItems();
        }

        private IEnumerable<TelegramChat> GenerateTelegramChats(int amount)
        {
            for (int id = 1; id <= amount; id++)
            {
                yield return TelegramChat.BuildDatabaseItem(id);
            }
        }

        private async Task SeedTelegramChatsTableWith(IReadOnlyCollection<TelegramChat> chats)
        {
            foreach(var chat in chats)
            {
                await repository.AddTelegramChatAsync(chat);
            }
        }
    }
}
