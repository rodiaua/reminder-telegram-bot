using ReminderTelegramBot.WebAPI.Client.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static ReminderTelegramBot.WebAPI.Client.Constants;

namespace ReminderTelegramBot.WebAPI.Client
{
    public class ReminderTelegramBotWebApiClient : IReminderTelegramBotWebApiClient
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUrl;

        public ReminderTelegramBotWebApiClient(string baseUrl)
        {
            httpClient = new HttpClient();
            this.baseUrl = new Uri(baseUrl);
        }

        public Task RegisterTelegramChat(AddTelegramChatRequest telegramChat)
        {
            return PostAsync(telegramChat, new Uri(baseUrl, TelegramChats));
        }

        #region private methods
        private Task<T> GetAsync<T>(Uri uri)
        {
            return httpClient.GetFromJsonAsync<T>(uri);
        }

        private Task<HttpResponseMessage> PostAsync<T>(T data,Uri uri)
        {
            var reqeust = GenerateRequest(HttpMethod.Post, uri);
            reqeust.Content = new ObjectContent<T>(data, new JsonMediaTypeFormatter());
            return httpClient.SendAsync(reqeust);
        }

        private async Task<R> PostAsync<T,R>(T data,Uri uri)
        {
            var result = await PostAsync(data, uri);
            return await result?.Content?.ReadFromJsonAsync<R>();
        } 

        private HttpRequestMessage GenerateRequest(HttpMethod method, Uri uri)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Add(HttpRequestHeader.ContentType.ToString(), JsonMediaType);
            return request;
        }
        #endregion private methods
    }
}