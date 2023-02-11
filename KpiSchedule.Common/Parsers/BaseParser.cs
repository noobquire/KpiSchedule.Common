using HtmlAgilityPack;
using Serilog;

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
        protected readonly ILogger logger;
        protected BaseParser(ILogger logger)
        {
            this.logger = logger;
        }

        public abstract TResult Parse(HtmlNode node);
    }
}
