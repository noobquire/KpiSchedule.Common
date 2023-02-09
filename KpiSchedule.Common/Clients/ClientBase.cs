using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.ScheduleKpiApi;
using Serilog;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace KpiSchedule.Common.Clients
{
    /// <summary>
    /// Base class with helper methods for clients used to call external APIs.
    /// </summary>
    public abstract class ClientBase
    {
        /// <summary>
        /// Logging interface.
        /// </summary>
        protected readonly ILogger logger;

        /// <summary>
        /// Initialize a new instance of the <see cref="ClientBase"/> class.
        /// </summary>
        /// <param name="logger">Logging interface.</param>
        protected ClientBase(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Check if response status code indicates success. If not, log an error message and throw a <see cref="KpiApiClientException"/>
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="requestApiName">Name of API we sent request to.</param>
        /// <returns>Task</returns>
        /// <exception cref="KpiApiClientException">Http response code does not indicate success.</exception>
        protected internal async Task CheckIfSuccessfulResponse(HttpResponseMessage response, string requestApiName)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content?.ReadAsStringAsync() ?? string.Empty;
                logger.Error("Response {responseCode} from {requestApi} does not indicate success: {responseMessage}", response.StatusCode, requestApiName, responseBody);
                throw new KpiApiClientException($"Response status code {response.StatusCode} does not indicate success.");
            }
        }

        /// <summary>
        /// Check if response body is null or empty. If it is, log an error message and throw a <see cref="KpiApiClientException"/>
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="requestApiName">Name of API we sent request to.</param>
        /// <returns>Task</returns>
        /// <exception cref="KpiApiClientException">Response body is null or empty.</exception>
        protected internal async Task CheckIfResponseBodyIsNullOrEmpty(HttpResponseMessage response, string requestApiName)
        {
            if (response.Content is null || string.IsNullOrWhiteSpace(await ReadResponseBodyAsUtf8String(response)))
            {
                logger.Error("Response body from calling {requestApi} is null or empty.", requestApiName);
                throw new KpiApiClientException($"Response body is null or empty.");
            }
        }

        /// <summary>
        /// Logs an error message and throws a <see cref="KpiApiClientException"/> indicating that 
        /// client was not able to deserialize response from API into expected type.
        /// </summary>
        /// <typeparam name="T">Expected type.</typeparam>
        /// <param name="response">Response body.</param>
        /// <param name="exception">JSON deserialization exception.</param>
        /// <exception cref="KpiApiClientException">Unable to deserialize response.</exception>
        protected internal void HandleNonSerializableResponse<T>(string response, JsonException exception)
        {
            logger.Error("Unable to deserialize response body {responseBody} as {typeName}: {exceptionMessage}", response, nameof(T), exception.Message);
            throw new KpiApiClientException($"Could not deserialize response from KPI API.", exception);
        }

        /// <summary>
        /// Checks if <see cref="HttpResponseMessage"/> indicates success and its body is not null or empty.
        /// Parses and returns body as <typeparam name="TResponse"/>.
        /// </summary>
        /// <typeparam name="TResponse">Response body type.</typeparam>
        /// <param name="response">HTTP response message.</param>
        /// <returns>Parsed response body.</returns>
        /// <exception cref="KpiApiClientException"/>
        protected internal async Task<TResponse> VerifyAndParseResponseBody<TResponse>(HttpResponseMessage response) where TResponse : new()
        {
            var requestUrl = response.RequestMessage.RequestUri.ToString();
            await CheckIfSuccessfulResponse(response, requestUrl);
            await CheckIfResponseBodyIsNullOrEmpty(response, requestUrl);

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseModel = new TResponse();
            var deserializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
            try
            {
                responseModel = JsonSerializer.Deserialize<TResponse>(responseJson, deserializationOptions);
            }
            catch (JsonException ex)
            {
                HandleNonSerializableResponse<TResponse>(responseJson, ex);
            }

            return responseModel;
        }

        private async Task<string> ReadResponseBodyAsUtf8String(HttpResponseMessage response)
        {
            byte[] buf = await response.Content.ReadAsByteArrayAsync();
            string content = Encoding.UTF8.GetString(buf);
            return content;
        }
    }
}
