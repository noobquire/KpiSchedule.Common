using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi.Teacher;
using Serilog;
using Serilog.Context;

namespace KpiSchedule.Common.Parsers.TeacherSchedulePage
{
    public class TeacherSchedulePageParser : BaseParser<RozKpiApiTeacherSchedule>
    {
        private readonly TeacherScheduleWeekTableParser tableParser;

        public TeacherSchedulePageParser(ILogger logger, TeacherScheduleWeekTableParser tableParser) : base(logger)
        {
            this.tableParser = tableParser;
        }

        public override RozKpiApiTeacherSchedule Parse(HtmlNode documentNode)
        {
            var document = documentNode.OwnerDocument;
            var labelHeaderNode = document.GetElementbyId("ctl00_MainContent_lblHeader");

            var teacherNamePrefix = "Розклад занять, викладач: ";
            var teacherName = labelHeaderNode.InnerText.Substring(teacherNamePrefix.Length).Trim();
            logger.Information("Parsing schedule tables for {teacherName}", teacherName);
            using (LogContext.PushProperty("teacherName", teacherName))
            {
                var firstWeekTableNode = document.GetElementbyId("ctl00_MainContent_FirstScheduleTable");
                var firstWeek = tableParser.Parse(firstWeekTableNode);

                var secondWeekTableNode = document.GetElementbyId("ctl00_MainContent_SecondScheduleTable");
                var secondWeek = tableParser.Parse(secondWeekTableNode);

                var schedule = new RozKpiApiTeacherSchedule()
                {
                    TeacherName = teacherName,
                    FirstWeek = firstWeek,
                    SecondWeek = secondWeek
                };
                return schedule;
            }
        }
    }
}
