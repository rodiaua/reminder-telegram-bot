using Microsoft.Extensions.Logging;
using ReminderTelegramBot.Common.Http;
using ReminderTelegramBot.WebAPI.Client.Models;
using Serilog.Context;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using static ReminderTelegramBot.WebAPI.Client.Constants;

namespace ReminderTelegramBot.WebAPI.Client
{
    public class ReminderTelegramBotWebApiClient : IReminderTelegramBotWebApiClient
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUrl;
        private readonly ILogger logger;

        public ReminderTelegramBotWebApiClient(string baseUrl, ILogger<ReminderTelegramBotWebApiClient> logger)
        {
            httpClient = new HttpClient();
            this.baseUrl = new Uri(baseUrl);
            this.logger = logger;
        }

        public Task RegisterTelegramChat(AddTelegramChatRequest telegramChat)
        {
            return PostAsync(telegramChat, new Uri(baseUrl, TelegramChats));
        }

        #region private methods
        private async Task<T> GetAsync<T>(Uri uri)
        {
            var request = GenerateRequest(HttpMethod.Get, uri);
            using (LogContext.PushProperty("CorrelationId", $"[{request.GetCorrelationId()}] - "))
            {
                try
                {
                    logger.LogInformation("Sending request {method} - {url}", request.Method.Method, request.RequestUri.ToString());
                    var result = await httpClient.SendAsync(request);
                    return await ProcessResoponseAsync<T>(result);
                }
                catch
                {
                    logger.LogInformation("Something went wrong wile sending request:");
                    logger.LogInformation("{method} - {url}", request.Method.Method, request.RequestUri.ToString());
                    return default;
                }
            }
        }

        private async Task<HttpResponseMessage> PostAsync<T>(T data, Uri uri)
        {
            var request = GenerateRequest(HttpMethod.Post, uri);
            request.Content = new ObjectContent<T>(data, new JsonMediaTypeFormatter());
            using (LogContext.PushProperty("CorrelationId", $"[{Common.Http.Constants.CorrelationIdHeaderKey}: {request.GetCorrelationId()}]"))
            {
                try
                {
                    logger.LogInformation("Sending request {method} - {url}", request.Method.Method, request.RequestUri.ToString());
                    var result = await httpClient.SendAsync(request);
                    await ProcessResoponseAsync(result);
                    return result;
                }
                catch
                {
                    logger.LogError("Something went wrong wile sending request:");
                    logger.LogError("{method} - {url}", request.Method.Method, request.RequestUri.ToString());
                    return default;
                }
            }
        }

        private async Task<R> PostAsync<T, R>(T data, Uri uri)
        {
            var result = await PostAsync(data, uri);
            return await ProcessResoponseAsync<R>(result);
        }

        private async Task<HttpResponseMessage> ProcessResoponseAsync(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                logger.LogInformation("Response {method} - {url}: {status}", responseMessage.RequestMessage.Method, responseMessage.RequestMessage.RequestUri.ToString(), responseMessage.StatusCode);
                return responseMessage;
            }
            else
            {
                logger.LogError("Response {method} - {url}: {status}", responseMessage.RequestMessage.Method, responseMessage.RequestMessage.RequestUri.ToString(), responseMessage.StatusCode);
                logger.LogError((await responseMessage.Content.ReadFromJsonAsync<DefaultResponse>()).message);
            }
            return default;
        }

        private async Task<R> ProcessResoponseAsync<R>(HttpResponseMessage responseMessage)
        {
            var processedResponse = await ProcessResoponseAsync(responseMessage);
            if (processedResponse == null) return default;
            return await processedResponse.Content.ReadFromJsonAsync<R>();
        }

        private HttpRequestMessage GenerateRequest(HttpMethod method, Uri uri)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Add(HttpRequestHeader.ContentType.ToString(), JsonMediaType);
            request.Headers.Add(Common.Http.Constants.CorrelationIdHeaderKey, Guid.NewGuid().ToString());
            return request;
        }
        #endregion private methods
    }
}