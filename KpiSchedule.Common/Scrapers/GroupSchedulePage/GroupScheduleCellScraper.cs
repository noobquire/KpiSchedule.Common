using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;

namespace KpiSchedule.Common.Scrapers.GroupSchedulePage
{
    internal class GroupScheduleCellScraper : BaseScraper<IEnumerable<RozKpiApiGroupPair>>
    {
        public GroupScheduleCellScraper(HtmlNode node) : base(node)
        {
        }

        public override IEnumerable<RozKpiApiGroupPair> Parse()
        {
            return null;
        }
    }
}