using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using KpiSchedule.Common.Parsers.GroupSchedulePage;
using Serilog;

namespace KpiSchedule.Common.Parsers.TeacherSchedulePage
{
    public class TeacherScheduleCellParser : BaseParser<RozKpiApiTeacherPair>
    {
        private readonly PairInfoInTeacherScheduleCellParser pairInfoParser;

        public TeacherScheduleCellParser(ILogger logger, PairInfoInTeacherScheduleCellParser pairInfoParser) : base(logger)
        {
            this.pairInfoParser = pairInfoParser;
        }

        public RozKpiApiTeacherPair Parse(HtmlNode cellNode, int pairNumber)
        {
            if (string.IsNullOrEmpty(cellNode.InnerHtml))
            {
                logger.Verbose("Cell is empty, skipping...");
                return null;
            }

            var subjectName = ParseSubjectNameInCell(cellNode);
            var fullSubjectName = ParseFullSubjectNameInCell(cellNode);
            var groups = ParseGroupsInCell(cellNode);
            var pairInfos = pairInfoParser.Parse(cellNode);

            var pair = GroupPairData(subjectName, fullSubjectName, groups, pairInfos);

            pair.PairNumber = pairNumber;
            var startAndEnd = PairSchedule.GetPairStartAndEnd(pairNumber);
            pair.StartTime = startAndEnd.pairStart;
            pair.EndTime = startAndEnd.pairEnd;

            return pair;
        }

        private RozKpiApiTeacherPair GroupPairData(string subjectName, string fullSubjectName, IEnumerable<string> groupNames, RozKpiApiPairInfo pairInfo)
        {
            var subject = new RozKpiApiSubject()
            {
                SubjectName = subjectName,
                SubjectFullName = fullSubjectName
            };

            var pair = new RozKpiApiTeacherPair()
            {
                Subject = subject,
                Type = pairInfo.PairType,
                Rooms = pairInfo.Rooms,
                GroupNames = groupNames.Distinct().OrderBy(g => g).ToList(),
                IsOnline = pairInfo.IsOnline
            };

            return pair;
        }

        private IEnumerable<string> ParseGroupsInCell(HtmlNode cellNode)
        {
            var groupsString = cellNode
                .SelectSingleNode("span[@class=\"disLabel\"]/following-sibling::text()[last()]")
                .InnerText;

            var groups = groupsString.Split(", ");
            return groups.OrderBy(g => g);
        }

        public override RozKpiApiTeacherPair Parse(HtmlNode cellNode)
        {
            return Parse(cellNode, 1);
        }

        private HtmlNode GetSubjectLabelLinkNode(HtmlNode cellNode)
        {
            return cellNode.SelectSingleNode("span[@class=\"disLabel\"]/a");
        }

        private string ParseFullSubjectNameInCell(HtmlNode cellNode)
        {
            var subjectLabelLinkNode = GetSubjectLabelLinkNode(cellNode);
            var fullName = subjectLabelLinkNode.Attributes["title"].Value;

            return fullName;
        }

        private string ParseSubjectNameInCell(HtmlNode cellNode)
        {
            var subjectLabelLinkNode = GetSubjectLabelLinkNode(cellNode);
            var name = subjectLabelLinkNode.InnerText;

            return name;
        }
    }
}
