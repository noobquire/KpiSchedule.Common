using HtmlAgilityPack;

namespace KpiSchedule.Common.Parsers
{
    /// <summary>
    /// Base parser class with helper methods to parse roz.kpi.ua pages.
    /// Name of child classes should reflect what HTML element/node is parsed.
    /// Result class name should reflect what domain model is an output of parsing.
    /// </summary>
    /// <typeparam name="TResult">Scraper output result type.</typeparam>
    public abstract class BaseParser<TResult> : IParser<TResult>
    {
        protected readonly HtmlNode node;
        protected readonly HtmlDocument document;
        protected BaseParser(HtmlDocument document)
        {
            this.document = document;
            this.node = document.DocumentNode;
        }

        protected BaseParser(HtmlNode node)
        {
            this.node = node;
            this.document = node.OwnerDocument;
        }

        public abstract TResult Parse();
    }
}
