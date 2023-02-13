using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Exceptions;
using Moq;
using System.Text.Json;
using Serilog;
using System.Net;
using KpiSchedule.Common.Models.RozKpiApi;
using KpiSchedule.Common.Models.ScheduleKpiApi;
using FluentAssertions;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace KpiSchedule.Common.UnitTests.Clients
{
    internal class TestClient : BaseClient
    {
        public TestClient() : base(new Mock<ILogger>().Object) { }
    }

    [TestFixture]
    public class ClientBaseTests
    {
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NoContent)]
        [TestCase(HttpStatusCode.Created)]
        public void CheckIfSuccessfulResponse_WhenSuccessCode_DoesNotThrow(HttpStatusCode statusCode)
        {
            var client = new TestClient();
            var response = new HttpResponseMessage();
            response.StatusCode = statusCode;

            Assert.DoesNotThrowAsync(() => client.CheckIfSuccessfulResponse(response, "/Test"));
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.NotFound)]
        public void CheckIfSuccessfulResponse_WhenFailureCode_ThrowsClientException(HttpStatusCode statusCode)
        {
            var client = new TestClient();
            var response = new HttpResponseMessage();
            response.StatusCode = statusCode;

            Assert.ThrowsAsync<KpiScheduleClientException>(() => client.CheckIfSuccessfulResponse(response, "/Test"));
        }

        [TestCase("test")]
        [TestCase("@!/-=")]
        public void CheckIfResponseBodyIsNullOrEmpty_WhenResponseNotNullOrEmpty_DoesNotThrow(string responseBody)
        {
            var client = new TestClient();
            var response = new HttpResponseMessage();
            response.Content = new StringContent(responseBody);

            Assert.DoesNotThrowAsync(() => client.CheckIfResponseBodyIsNullOrEmpty(response, "/Test"));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void CheckIfResponseBodyIsNullOrEmpty_WhenResponseIsEmptyOrWhiteSpace_ThrowsClientException(string responseBody)
        {
            var client = new TestClient();
            var response = new HttpResponseMessage();
            response.Content = new StringContent(responseBody);

            Assert.ThrowsAsync<KpiScheduleClientException>(() => client.CheckIfResponseBodyIsNullOrEmpty(response, "/Test"));
        }

        [Test]
        public void HandleNonSerializableResponse_ThrowsClientException()
        {
            var jsonException = new JsonException();
            var client = new TestClient();
            var response = "testResponse";

            Assert.Throws<KpiScheduleClientException>(() => client.HandleNonSerializableResponse<BaseRozKpiApiResponse>(response, jsonException));
        }

        [Test]
        public async Task VerifyAndParseResponseBody_ValidResponse_ShouldReturnParsedModel()
        {
            var testResponse = new ScheduleKpiApiGroup()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = "ІІ-11",
                Faculty = "Test"
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
            var responseJson = JsonSerializer.Serialize(testResponse, options);
            var client = new TestClient();
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://test")
                },
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseJson)
            };

            var result = await client.VerifyAndParseResponseBody<ScheduleKpiApiGroup>(response);

            result.Should().NotBeNull()
                .And.BeEquivalentTo(testResponse);
        }

        [Test]
        public void VerifyAndParseResponseBody_EmptyResponse_ShouldThrowClientException()
        {
            var client = new TestClient();
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://test")
                },
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            };

            Assert.ThrowsAsync<KpiScheduleClientException>(() => client.VerifyAndParseResponseBody<ScheduleKpiApiGroup>(response));
        }

        [Test]
        public void VerifyAndParseResponseBody_UnsuccessfulResponse_ShouldThrowClientException()
        {
            var testResponse = new ScheduleKpiApiGroup()
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = "ІІ-11",
                Faculty = "Test"
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };
            var responseJson = JsonSerializer.Serialize(testResponse, options);
            var client = new TestClient();
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://test")
                },
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(responseJson)
            };

            Assert.ThrowsAsync<KpiScheduleClientException>(() => client.VerifyAndParseResponseBody<ScheduleKpiApiGroup>(response));
        }
    }
}
