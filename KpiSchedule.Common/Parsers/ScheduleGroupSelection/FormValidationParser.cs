using HtmlAgilityPack;
using Serilog;

namespace KpiSchedule.Common.Parsers.ScheduleGroupSelection
{
    /// <summary>
    /// Parses event validation field value for group selection form.
    /// </summary>
    public class FormValidationParser : BaseParser<string>
    {
        public FormValidationParser(ILogger logger) : base(logger)
        {
        }

        public override string Parse(HtmlNode documentNode)
        {
            var document = documentNode.OwnerDocument;
            var eventValidationElement = document.GetElementbyId("__EVENTVALIDATION");

            return eventValidationElement.Attributes["value"].Value;
        }
    }
}
