using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;

namespace KpiSchedule.Common.Parsers.ScheduleGroupSelection
{
    public class ConflictingGroupNamesParser : BaseParser<IEnumerable<RozKpiApiGroup>>
    {
        public ConflictingGroupNamesParser(ILogger logger) : base(logger)
        {
        }

        public override IEnumerable<RozKpiApiGroup> Parse(HtmlNode node)
        {
            var groups = new List<RozKpiApiGroup>();
            var document = node.OwnerDocument;

            var groupListTableNode = document.GetElementbyId("ctl00_MainContent_ctl00_GroupListPanel");
            foreach(var link in groupListTableNode.SelectNodes("table/tr/td/a"))
            {
                var groupName = link.InnerText;
                var groupLink = link.Attributes["href"].Value;
                // ViewSchedule.aspx?g=groupId
                var groupId = new Guid(groupLink.Substring(20));
                var group = new RozKpiApiGroup()
                {
                    GroupName = groupName,
                    Id = groupId
                };
                groups.Add(group);
            }

            return groups;
        }
    }
}
