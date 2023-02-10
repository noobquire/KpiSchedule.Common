using HtmlAgilityPack;

namespace KpiSchedule.Common.Scrapers
{
    internal interface IScraper<TResult>
    {
        TResult Parse();
    }
}
