using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace KpiSchedule.Common.Models
{
    [JsonConverter(typeof(JsonStringEnumConverterWithAttributeSupport))]
    public enum PairType
    {
        [EnumMember(Value = "lecture")]
        Lecture,
        [EnumMember(Value = "prac")]
        Practicum,
        [EnumMember(Value = "lab")]
        Lab,
        [EnumMember(Value = "seminar")]
        Seminar
    }
}