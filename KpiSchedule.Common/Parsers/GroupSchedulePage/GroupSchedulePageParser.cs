using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class GroupSchedulePageParser : BaseParser<RozKpiApiGroupSchedule>
    {
        private readonly GroupScheduleWeekTableParser tableParser;

        public GroupSchedulePageParser(ILogger logger, GroupScheduleWeekTableParser tableParser) : base(logger)
        {
            this.tableParser = tableParser;
        }

        public override RozKpiApiGroupSchedule Parse(HtmlNode documentNode)
        {
            var document = documentNode.OwnerDocument;
            var labelHeaderNode = document.GetElementbyId("ctl00_MainContent_lblHeader");

            var groupNamePrefix = "Розклад занять для ";
            var groupName = labelHeaderNode.InnerText.Substring(groupNamePrefix.Length);
            logger.Information("Parsing schedule tables for {groupName}", groupName);
            using (LogContext.PushProperty("groupName", groupName))
            {
                var firstWeekTableNode = document.GetElementbyId("ctl00_MainContent_FirstScheduleTable");
                var firstWeek = tableParser.Parse(firstWeekTableNode);

                var secondWeekTableNode = document.GetElementbyId("ctl00_MainContent_SecondScheduleTable");
                var secondWeek = tableParser.Parse(secondWeekTableNode);

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
}
