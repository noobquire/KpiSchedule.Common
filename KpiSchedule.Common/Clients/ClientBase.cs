using KpiSchedule.Common.Exceptions;
using Serilog;
using System.Runtime.CompilerServices;

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
        /// Check if response status code indicates success. If not, log an error message and throw a <see cref="KpiScheduleClientException"/>
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="requestApiName">Name of API we sent request to.</param>
        /// <returns>Task</returns>
        /// <exception cref="KpiScheduleClientException">Http response code does not indicate success.</exception>
        protected internal async Task CheckIfSuccessfulResponse(HttpResponseMessage response, string requestApiName)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response?.Content?.ReadAsStringAsync() ?? "";
                logger.Error("Response {responseCode} from request {requestApi} API does not indicate success: {responseMessage}", response.StatusCode, requestApiName, responseBody);
                throw new KpiScheduleClientException($"Response status code {response.StatusCode} does not indicate success.");
            }
        }

        /// <summary>
        /// Check if response body is null or empty. If it is, log an error message and throw a <see cref="KpiScheduleClientException"/>
        /// </summary>
        /// <param name="response">HTTP response.</param>
        /// <param name="requestApiName">Name of API we sent request to.</param>
        /// <returns>Task</returns>
        /// <exception cref="KpiScheduleClientException">Response body is null or empty.</exception>
        protected internal async Task CheckIfResponseBodyIsNullOrEmpty(HttpResponseMessage response, string requestApiName)
        {
            if (response.Content is null || string.IsNullOrWhiteSpace(await response.Content.ReadAsStringAsync()))
            {
                logger.Error("Response body from calling API {requestApi} is null or empty.", requestApiName);
                throw new KpiScheduleClientException($"Response body is null or empty.");
            }
        }
    }
}
