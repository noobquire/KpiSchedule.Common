using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Scrapers.GroupSchedulePage
{
    public class GroupSchedulePageScraper : BaseScraper<RozKpiApiGroupSchedule>
    {
        public GroupSchedulePageScraper(HtmlDocument document) : base(document)
        {
        }

        public override RozKpiApiGroupSchedule Parse()
        {           
            var labelHeaderNode = document.GetElementbyId("ctl00_MainContent_lblHeader");
            // Розклад занять для groupName
            var groupName = labelHeaderNode.InnerText.Substring(19);

            var firstWeekTableNode = document.GetElementbyId("ctl00_MainContent_FirstScheduleTable");
            var firstWeekTableScraper = new GroupScheduleWeekTableScraper(firstWeekTableNode);
            var firstWeek = firstWeekTableScraper.Parse();

            var secondWeekTableNode = document.GetElementbyId("ctl00_MainContent_SecondScheduleTable");
            var secondWeekTableScraper = new GroupScheduleWeekTableScraper(secondWeekTableNode);
            var secondWeek = secondWeekTableScraper.Parse();

            var schedule = new RozKpiApiGroupSchedule()
            {
                GroupName = groupName,
                FirstWeek = firstWeek,
                SecondWeek = secondWeek
            };
            return schedule;
        }
    }
}
