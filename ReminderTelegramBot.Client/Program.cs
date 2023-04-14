using Microsoft.Extensions.Options;
using ReminderTelegramBot.Client.Settings;
using ReminderTelegramBot.Common;
using ReminderTelegramBot.WebAPI.Client;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ReminderTelegramBot.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<TelegramBotSettings>(builder.Configuration.GetSection(nameof(TelegramBotSettings)));
            //add logging
            builder.Services.AddSerilogLogging();
            //register handler for telegram bot updates
            builder.Services.AddTransient<IUpdateHandler, UpdateHandler>();
            //register Telegram Bot
            builder.Services.AddTransient(provider =>
            {
                var telegramBotSettings = provider.GetRequiredService<IOptions<TelegramBotSettings>>().Value;
                var telegramBot = new TelegramBotClient(telegramBotSettings.APISecret);
                return telegramBot;
            });
            //add host service which starts receiving Telegram Bot updates and gracefully stop receiving it
            builder.Services.AddHostedService<TelegramBotUpdatesRecieverManager>();
            //register client which make http calls to Reminder Telegram Bot Web API
            builder.Services.AddScoped<IReminderTelegramBotWebApiClient>(options =>
            {
                var baseUrl = builder.Configuration.GetSection("ReminderTelegramBotWebApiClient").Value;
                return new ReminderTelegramBotWebApiClient(baseUrl);
            });

            var app = builder.Build();

            app.Run();
        }
    }
}