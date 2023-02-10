using HtmlAgilityPack;

namespace KpiSchedule.Common.Scrapers
{
    public class ScheduleGroupSelectionFormValidationScraper : IScraper<string>
    {
        private readonly HtmlDocument document;

        public ScheduleGroupSelectionFormValidationScraper(HtmlDocument document)
        {
            this.document = document;
        }

        public string Parse()
        {
            var eventValidationElement = document.GetElementbyId("__EVENTVALIDATION");

            return eventValidationElement.Attributes["value"].Value;
        }
    }
}
