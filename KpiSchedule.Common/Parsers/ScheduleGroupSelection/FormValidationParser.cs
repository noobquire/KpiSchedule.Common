using HtmlAgilityPack;

namespace KpiSchedule.Common.Parsers.ScheduleGroupSelection
{
    /// <summary>
    /// Parses event validation field value for group selection form.
    /// </summary>
    public class FormValidationParser : BaseParser<string>
    {
        public FormValidationParser(HtmlDocument document) : base(document)
        {
        }

        public override string Parse()
        {
            var eventValidationElement = document.GetElementbyId("__EVENTVALIDATION");

            return eventValidationElement.Attributes["value"].Value;
        }
    }
}
