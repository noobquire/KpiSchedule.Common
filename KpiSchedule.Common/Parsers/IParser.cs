using HtmlAgilityPack;

namespace KpiSchedule.Common.Parsers
{
    internal interface IParser<out TResult>
    {
        TResult Parse(HtmlNode node);
    }
}
