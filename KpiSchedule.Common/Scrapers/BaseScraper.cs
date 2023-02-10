using HtmlAgilityPack;

namespace KpiSchedule.Common.Scrapers
{
    /// <summary>
    /// Base scraper class with helper methods to parse roz.kpi.ua pages.
    /// Name of child classes should reflect what HTML element/node is parsed.
    /// Result class name should reflect what domain model is an output of parsing.
    /// </summary>
    /// <typeparam name="TResult">Scraper output result type.</typeparam>
    public abstract class BaseScraper<TResult> : IScraper<TResult>
    {
        protected readonly HtmlNode node;
        protected readonly HtmlDocument document;
        protected BaseScraper(HtmlDocument document)
        {
            this.document = document;
        }

        protected BaseScraper(HtmlNode node)
        {
            this.node = node;
        }

        public abstract TResult Parse();
    }
}
