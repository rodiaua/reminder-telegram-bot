using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ReminderTelegramBot.Client
{
    public class TelegramBotUpdatesRecieverManager : IHostedService
    {
        private readonly IUpdateHandler updateHandler;
        private CancellationTokenSource tokenSource;
        private readonly TelegramBotClient telegramBotClient;
        private readonly ILogger logger;

        public TelegramBotUpdatesRecieverManager(IUpdateHandler updateHandler, TelegramBotClient telegramBotClient, ILogger<TelegramBotUpdatesRecieverManager> logger)
        {
            this.updateHandler = updateHandler;
            this.telegramBotClient = telegramBotClient;
            this.logger = logger;
            tokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            telegramBotClient.ReceiveAsync(updateHandler, new ReceiverOptions
                {
                    AllowedUpdates = new UpdateType[] { UpdateType.Message },
                    ThrowPendingUpdates = true
                }, tokenSource.Token);

            logger.LogInformation("Started receiving bot messages.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopped receiving bot messages.");
            tokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}