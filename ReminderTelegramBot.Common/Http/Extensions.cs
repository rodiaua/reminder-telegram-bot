namespace ReminderTelegramBot.Common.Http
{
    public static class Extensions
    {
        public static Guid GetCorrelationId(this HttpRequestMessage httpRequestMessage)
        {
            return new Guid(httpRequestMessage.Headers.GetValues(Constants.CorrelationIdHeaderKey).FirstOrDefault());
        }
    }
}
