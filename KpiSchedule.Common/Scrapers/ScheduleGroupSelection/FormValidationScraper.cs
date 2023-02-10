using HtmlAgilityPack;

namespace KpiSchedule.Common.Scrapers.ScheduleGroupSelection
{
    /// <summary>
    /// Scrapes event validation field value for group selection form.
    /// </summary>
    public class FormValidationScraper : BaseScraper<string>
    {
        public FormValidationScraper(HtmlDocument document) : base(document)
        {
        }

        public override string Parse()
        {
            var eventValidationElement = document.GetElementbyId("__EVENTVALIDATION");

            return eventValidationElement.Attributes["value"].Value;
        }
    }
}
