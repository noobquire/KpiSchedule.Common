using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class GroupScheduleCellParser : BaseParser<IEnumerable<RozKpiApiGroupPair>>
    {
        private readonly PairInfoInScheduleCellParser pairInfoParser;
        private readonly TeachersInGroupScheduleCellParser teachersParser;
        private readonly GroupSchedulePairDataGroupper pairDataGroupper;

        public GroupScheduleCellParser(ILogger logger,
            TeachersInGroupScheduleCellParser teachersParser,
            PairInfoInScheduleCellParser pairInfoParser,
            GroupSchedulePairDataGroupper pairDataGroupper) : base(logger)
        {
            this.teachersParser = teachersParser;
            this.pairInfoParser = pairInfoParser;
            this.pairDataGroupper = pairDataGroupper;
        }

        public override IEnumerable<RozKpiApiGroupPair> Parse(HtmlNode cellNode)
        {
            return Parse(cellNode, 1);
        }

        public IEnumerable<RozKpiApiGroupPair> Parse(HtmlNode cellNode, int pairNumber)
        {
            if (string.IsNullOrEmpty(cellNode.InnerHtml))
            {
                logger.Verbose("Cell is empty, skipping...");
                return Array.Empty<RozKpiApiGroupPair>();
            }

            var subjectNames = ParseSubjectNamesInCell(cellNode);
            var fullSubjectNames = ParseFullSubjectNamesInCell(cellNode);
            var teachers = teachersParser.Parse(cellNode);
            var pairInfos = pairInfoParser.Parse(cellNode);

            var pairData = new RozKpiApiGroupPairData()
            {
                SubjectNames = subjectNames,
                FullSubjectNames = fullSubjectNames,
                PairInfos = pairInfos,
                Teachers = teachers
            };

            var pairs = pairDataGroupper.GroupPairData(pairData);

            pairs.ToList().ForEach(p =>
            {
                p.PairNumber = pairNumber;
                var startAndEnd = PairSchedule.GetPairStartAndEnd(pairNumber);
                p.StartTime = startAndEnd.pairStart;
                p.EndTime = startAndEnd.pairEnd;
            });

            return pairs;
        }

        private HtmlNodeCollection GetSubjectLabelLinkNodes(HtmlNode cellNode)
        {
            return cellNode.SelectNodes("span[@class=\"disLabel\"]/a");
        }

        private IEnumerable<string> ParseFullSubjectNamesInCell(HtmlNode cellNode)
        {
            var subjectLabelLinkNodes = GetSubjectLabelLinkNodes(cellNode);
            var fullNames = subjectLabelLinkNodes.Select(n => n.Attributes["title"].Value).ToList();

            return fullNames;
        }

        private IEnumerable<string> ParseSubjectNamesInCell(HtmlNode cellNode)
        {
            var subjectLabelLinkNodes = GetSubjectLabelLinkNodes(cellNode);
            var names = subjectLabelLinkNodes.Select(n => n.InnerText).ToList();

            return names;
        }
    }
}
