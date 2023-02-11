using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class GroupScheduleCellParser : BaseParser<IEnumerable<RozKpiApiGroupPair>>
    {

        public GroupScheduleCellParser(ILogger logger) : base(logger)
        {
        }

        public override IEnumerable<RozKpiApiGroupPair> Parse(HtmlNode cellNode)
        {
            return Parse(cellNode, 1);
        }

        public IEnumerable<RozKpiApiGroupPair> Parse(HtmlNode cellNode, int pairNumber)
        {
            return new List<RozKpiApiGroupPair>() { new RozKpiApiGroupPair() };
        }
    }
}
