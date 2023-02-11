using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class GroupSchedulePageParser : BaseParser<RozKpiApiGroupSchedule>
    {
        public GroupSchedulePageParser(HtmlDocument document) : base(document)
        {
        }

        public override RozKpiApiGroupSchedule Parse()
        {           
            var labelHeaderNode = document.GetElementbyId("ctl00_MainContent_lblHeader");
            // Розклад занять для groupName
            var groupName = labelHeaderNode.InnerText.Substring(19);

            var firstWeekTableNode = document.GetElementbyId("ctl00_MainContent_FirstScheduleTable");
            var firstWeekTableParser = new GroupScheduleWeekTableParser(firstWeekTableNode);
            var firstWeek = firstWeekTableParser.Parse();

            var secondWeekTableNode = document.GetElementbyId("ctl00_MainContent_SecondScheduleTable");
            var secondWeekTableParser = new GroupScheduleWeekTableParser(secondWeekTableNode);
            var secondWeek = secondWeekTableParser.Parse();

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
