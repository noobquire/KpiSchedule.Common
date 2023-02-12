using HtmlAgilityPack;
using KpiSchedule.Common.Exceptions;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;

namespace KpiSchedule.Common.Parsers.ScheduleGroupSelection
{
    public class ConflictingGroupNamesParser : BaseParser<IEnumerable<RozKpiApiGroup>>
    {
        public ConflictingGroupNamesParser(ILogger logger) : base(logger)
        {
        }

        public override IEnumerable<RozKpiApiGroup> Parse(HtmlNode documentNode)
        {
            var groups = new List<RozKpiApiGroup>();
            var document = documentNode.OwnerDocument;

            var groupListTableNode = document.GetElementbyId("ctl00_MainContent_ctl00_GroupListPanel");
            foreach(var link in groupListTableNode.SelectNodes("table/tr/td/a"))
            {
                var groupName = link.InnerText;
                var groupLink = link.Attributes["href"].Value;
                var groupIdPrefix = "ViewSchedule.aspx?g=";
                var groupId = new Guid(groupLink.Substring(groupIdPrefix.Length));

                var group = new RozKpiApiGroup()
                {
                    GroupName = groupName,
                    Id = groupId
                };
                groups.Add(group);
            }

            var conflictingGroupNames = string.Join(", ", groups.Select(g => g.GroupName));
            logger.Information("Parsed {count} conflicting group names: {conflictingGroupNames}", groups.Count, conflictingGroupNames);
            return groups;
        }

        public bool IsGroupNotFoundPage(HtmlNode documentNode)
        {
            return documentNode.InnerHtml.Contains("Групи з такою назвою не знайдено!");
        }
    }
}
