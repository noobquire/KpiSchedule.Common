namespace KpiSchedule.Common.Parsers
{
    internal interface IParser<TResult>
    {
        TResult Parse();
    }
}
