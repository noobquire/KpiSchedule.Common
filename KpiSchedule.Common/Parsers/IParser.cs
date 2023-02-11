using HtmlAgilityPack;

namespace KpiSchedule.Common.Parsers
{
    internal interface IParser<TResult>
    {
        TResult Parse(HtmlNode node);
    }
}
