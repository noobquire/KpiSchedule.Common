using KpiSchedule.Common.Clients;
using KpiSchedule.Common.Exceptions;
using Moq;
using Serilog;
using System.Net;

namespace KpiSchedule.Common.UnitTests.Clients
{
    internal class TestClient : ClientBase
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
    }
}
