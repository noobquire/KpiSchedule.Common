using HtmlAgilityPack;
using KpiSchedule.Common.Models.RozKpiApi;
using Serilog;
using System.Text.RegularExpressions;

namespace KpiSchedule.Common.Parsers.GroupSchedulePage
{
    public class PairInfoInScheduleCellParser : BaseParser<IEnumerable<RozKpiApiPairInfo>>
    {
        public PairInfoInScheduleCellParser(ILogger logger) : base(logger)
        {
        }

        public override IEnumerable<RozKpiApiPairInfo> Parse(HtmlNode cellNode)
        {
            var pairInfos = new List<RozKpiApiPairInfo>();

            var pairInfoMatch = Regex.Match(cellNode.InnerHtml, "(<\\/a><br>|<br> |(?<!<\\/span>)<br>)+(.+)$");

            if (!pairInfoMatch.Success)
            {
                logger.Verbose("No pair infos matched in cell");
                return pairInfos;
            }

            var pairInfosString = pairInfoMatch.Groups[2].ToString();
            var splitPairInfoStrings = Regex.Split(pairInfosString, "(?!\\d), (?!\\d)").Select(s => s.Trim());

            foreach (var pairInfoString in splitPairInfoStrings)
            {
                var pairInfo = pairInfoString.Contains("maps.google.com") ?
                    ParseLinkInfo(pairInfoString) :
                    ParsePlainInfo(pairInfoString);

                pairInfos.Add(pairInfo);
            }

            return pairInfos;
        }

        private RozKpiApiPairInfo ParsePlainInfo(string pairInfoString)
        {
            var pairInfo = new RozKpiApiPairInfo()
            {
                PairType = pairInfoString.Split(" ")[0],
                Rooms = Array.Empty<string>(),
                IsOnline = pairInfoString.Contains("on-line")
            };

            return pairInfo;
        }

        private RozKpiApiPairInfo ParseLinkInfo(string pairInfoString)
        {
            var node = CreateHtmlNode(pairInfoString);

            var room = node.InnerHtml.Substring(0, node.InnerHtml.IndexOf(' '));
            var pairType = node.InnerHtml.Substring(node.InnerHtml.IndexOf(' ') + 1);
            var isOnline = node.InnerHtml.Contains("on-line");

            var pairInfo = new RozKpiApiPairInfo()
            {
                PairType = pairType,
                Rooms = new[] { room },
                IsOnline = isOnline
            };

            return pairInfo;
        }

        private HtmlNode CreateHtmlNode(string htmlString)
        {
            var document = new HtmlDocument();
            var div = document.CreateElement("div");
            div.InnerHtml = htmlString.Trim();
            return div.ChildNodes.First();
        }
    }
}
