using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi.Teacher;
using Serilog;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class TeachersInGroupScheduleCellParser : BaseParser<RozKpiApiTeacher[][]>
    {
        public TeachersInGroupScheduleCellParser(ILogger logger) : base(logger)
        {
        }

        public override RozKpiApiTeacher[][] Parse(HtmlNode cellNode)
        {
            var teachers = new List<RozKpiApiTeacher[]>();

            var scheduleLinkPrefix = "/Schedules/ViewSchedule.aspx?v=";

            var shortNames = GetTeacherLinkNodes(cellNode)?.Select(n => n.InnerText);
            var fullNames = GetTeacherLinkNodes(cellNode)?.Select(n => n.Attributes["title"].Value);
            var scheduleIds = GetTeacherLinkNodes(cellNode)?
                .Select(n => n.Attributes["href"].Value)
                .Select(l => l.Substring(scheduleLinkPrefix.Length));

            for (int i = 0; i < shortNames?.Count(); i++)
            {
                var teacher = new RozKpiApiTeacher()
                {
                    ShortName = shortNames.ElementAt(i),
                    FullName = fullNames.ElementAt(i),
                    ScheduleId = new Guid(scheduleIds.ElementAt(i))
                };
                teachers.Add(new[] { teacher });
            }

            logger.Verbose("Parsed {teachersCount} teachers in cell", teachers.Count);
            return teachers.ToArray();
        }

        private HtmlNodeCollection GetTeacherLinkNodes(HtmlNode cellNode)
        {
            return cellNode.SelectNodes("a[contains(@href, \"/Schedules/ViewSchedule\")]");
        }
    }
}
