using HtmlAgilityPack;
using KpiSchedule.Common.Models;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class PairInfoInTeacherScheduleCellParser : BaseParser<RozKpiApiPairInfo>
    {
        public PairInfoInTeacherScheduleCellParser(ILogger logger) : base(logger)
        {
        }

        public override RozKpiApiPairInfo Parse(HtmlNode cellNode)
        {
            var linkPairInfo = cellNode.SelectSingleNode("span[@class=\"disLabel\"]/following-sibling::a[contains(@href, \"google.com\")]");
            if(linkPairInfo != null)
            {
                return ParseLinkPairInfo(linkPairInfo);
            }
            var plainPairInfo = cellNode.SelectSingleNode("span[@class=\"disLabel\"]/following-sibling::text()[contains(., \"Лек\") or contains(., \"Прак\") or contains (., \"Лаб\")]");
            if(plainPairInfo != null)
            {
                return ParsePlainInfo(plainPairInfo);
            }

            return new RozKpiApiPairInfo()
            {
                PairType = PairType.Lecture,
                Rooms = Array.Empty<string>(),
                IsOnline = false
            };
        }

        private RozKpiApiPairInfo ParseLinkPairInfo(HtmlNode node)
        {
            var infoSpaceIndex = node.InnerHtml.IndexOf(' ');

            var room = infoSpaceIndex == -1 ? node.InnerHtml : node.InnerHtml.Substring(0, infoSpaceIndex);
            var pairType = infoSpaceIndex == -1 ? node.InnerHtml : node.InnerHtml.Substring(node.InnerHtml.IndexOf(' ') + 1);
            var isOnline = node.InnerHtml.Contains("on-line");

            var pairInfo = new RozKpiApiPairInfo()
            {
                PairType = PairTypeParser.ParsePairType(pairType),
                Rooms = new[] { room },
                IsOnline = isOnline
            };

            return pairInfo;
        }

        private RozKpiApiPairInfo ParsePlainInfo(HtmlNode plainInfoNode)
        {
            var pairInfoString = plainInfoNode.InnerText.Trim();

            var pairInfo = new RozKpiApiPairInfo()
            {
                PairType = PairTypeParser.ParsePairType(pairInfoString),
                Rooms = Array.Empty<string>(),
                IsOnline = pairInfoString.Contains("on-line")
            };

            return pairInfo;
        }
    }
}
