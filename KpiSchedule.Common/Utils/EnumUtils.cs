using System.Runtime.Serialization;

namespace KpiSchedule.Common.Utils
{
    public static class EnumUtils
    {
        public static string ToEnumString<T>(this T enumValue) where T : struct
        {
            var type = enumValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException("ToEnumString<T>(): Must be of enum type", "T");

            // Tries to find an EnumMemberAttribute with overriden name.
            var memberInfo = type.GetMember(enumValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((EnumMemberAttribute)attrs[0]).Value;
                }
            }
            // If we have no EnumMemberAttribute, return ToString of the enum
            return enumValue.ToString();
        }
    }
}
