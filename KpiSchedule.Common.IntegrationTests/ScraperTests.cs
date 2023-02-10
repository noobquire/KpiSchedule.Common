using HtmlAgilityPack;
using KpiSchedule.Common.Scrapers.GroupSchedulePage;

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
            var scraper = new GroupSchedulePageScraper(groupScheduleDocument);

            var schedule = scraper.Parse();

            Assert.IsNotNull(schedule);
        }
    }
}
