using HtmlAgilityPack;
using KpiSchedule.Common.Parsers.GroupSchedulePage;

namespace KpiSchedule.Common.IntegrationTests
{
    [TestFixture]
    public class ScraperTests
    {
        private HtmlDocument groupScheduleDocument;
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var groupScheduleHtml = File.ReadAllText("RozKpiApiResponses/group-schedule-page.html");
            groupScheduleDocument = new HtmlDocument();
            groupScheduleDocument.LoadHtml(groupScheduleHtml);
        }

        [Test]
        public void GroupScheduleScraper_ParsesGroupSchedule()
        {
            var parser = new GroupSchedulePageParser(groupScheduleDocument);

            var schedule = parser.Parse();

            Assert.IsNotNull(schedule);
        }
    }
}
