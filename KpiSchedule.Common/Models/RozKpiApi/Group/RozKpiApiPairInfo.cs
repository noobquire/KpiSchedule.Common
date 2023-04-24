namespace KpiSchedule.Common.Models.RozKpiApi.Group
{
    public class RozKpiApiPairInfo
    {
        public IList<string> Rooms { get; set; }
        public PairType PairType { get; set; }
        public bool IsOnline { get; set; }
    }
}
