using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi.Group;
using Serilog;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class PairInfoInGroupScheduleCellParser : BaseParser<IEnumerable<RozKpiApiPairInfo>>
    {
        public PairInfoInGroupScheduleCellParser(ILogger logger) : base(logger)
        {
        }

        public override IEnumerable<RozKpiApiPairInfo> Parse(HtmlNode cellNode)
        {
            var pairInfos = new List<RozKpiApiPairInfo>();

            var linkPairInfos = cellNode.SelectNodes("span[@class=\"disLabel\"]/following-sibling::a[contains(@href, \"google.com\")]");
            if(linkPairInfos is not null && linkPairInfos.Any())
            {
                foreach(var node in linkPairInfos)
                {
                    var info = ParseLinkInfo(node);
                    pairInfos.Add(info);
                }
            }

            var plainPairInfos = cellNode.SelectNodes("span[@class=\"disLabel\"]/following-sibling::text()[contains(., \"Лек\") or contains(., \"Прак\") or contains (., \"Лаб\")]");
            if (plainPairInfos is not null && plainPairInfos.Any())
            {
                foreach (var node in plainPairInfos)
                {
                    var info = ParsePlainInfo(node);
                    pairInfos.Add(info);
                }
            }

            if(plainPairInfos is null && linkPairInfos is null)
            {
                pairInfos.Add(new RozKpiApiPairInfo()
                {
                    PairType = Models.PairType.Lecture,
                    Rooms = Enumerable.Empty<string>().ToList(),
                    IsOnline = false
                }) ;
            }

            return pairInfos;
        }

        private RozKpiApiPairInfo ParsePlainInfo(HtmlNode node)
        {
            // Прак on-line
            // Прак
            var pairInfoString = node.InnerText;
            var infoSpaceIndex = pairInfoString.IndexOf(' ');

            var pairInfo = new RozKpiApiPairInfo()
            {
                PairType = PairTypeParser.ParsePairType(pairInfoString.Split(" ")[0]),
                Rooms = Enumerable.Empty<string>().ToList(),
                IsOnline = pairInfoString.Contains("on-line")
            };

            return pairInfo;
        }

        private RozKpiApiPairInfo ParseLinkInfo(HtmlNode node)
        {
            // <a class="plainLink" href="http://maps.google.com?q=50.447021,30.456021">-18 Лек on-line</a>
            // <a class="plainLink" href="http://maps.google.com?q=50.447021,30.456021">404-18</a>
            var pairInfoString = node.InnerText;
            var infoSpaceIndex = pairInfoString.IndexOf(' ');
            
            var room = infoSpaceIndex == -1 ? pairInfoString : pairInfoString.Substring(0, infoSpaceIndex);
            var pairType = infoSpaceIndex == -1 ? pairInfoString : pairInfoString.Substring(node.InnerHtml.IndexOf(' ') + 1);
            var isOnline = pairInfoString.Contains("on-line");

            var pairInfo = new RozKpiApiPairInfo()
            {
                PairType = PairTypeParser.ParsePairType(pairType),
                Rooms = new[] { room },
                IsOnline = isOnline
            };

            return pairInfo;
        }
    }
}
