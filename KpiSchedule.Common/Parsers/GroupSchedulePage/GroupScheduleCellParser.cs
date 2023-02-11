using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    internal class GroupScheduleCellParser : BaseParser<IEnumerable<RozKpiApiGroupPair>>
    {
        public GroupScheduleCellParser(HtmlNode node) : base(node)
        {
        }

        public override IEnumerable<RozKpiApiGroupPair> Parse()
        {
            return null;
        }
    }
}
