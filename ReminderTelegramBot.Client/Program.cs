using Microsoft.Extensions.Options;
using ReminderTelegramBot.Client.Settings;
using RxTelegram.Bot;

namespace ReminderTelegramBot.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<TelegramBotSettings>(builder.Configuration.GetSection(nameof(TelegramBotSettings)));

            //register TelegramBot
            builder.Services.AddScoped(provider =>
            {
                var telegramBotSettings = provider.GetRequiredService<IOptions<TelegramBotSettings>>().Value;
                return new TelegramBot(telegramBotSettings.APISecret);
            });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }

    public class TelegramBotAPIManager
    {
        private readonly TelegramBot telegramBot;
        private readonly IDisposable messageUpdateSubscription;

        public TelegramBotAPIManager(TelegramBot telegramBot)
        {
            this.telegramBot = telegramBot;
            messageUpdateSubscription = telegramBot.Updates.Message.Subscribe();
        }


    }

    public class UpdatesHandlers
    {

    }
}