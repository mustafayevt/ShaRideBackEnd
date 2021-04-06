using System;
using System.Globalization;
using TimeZoneConverter;

namespace ShaRide.Application.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// <example>
        /// 28 Mart
        /// 18:00
        /// </example>
        /// </summary>
        public static string ToCustomFormat(this DateTime dateTime)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture("az-AZ");
            return dateTime.ToString("dd MMMM",cultureInfo) + Environment.NewLine + dateTime.ToString("t",cultureInfo);
        }

        public static DateTime ToAzerbaijanDateTime(this DateTime dateTime)
        {
            var info = TZConvert.GetTimeZoneInfo("Azerbaijan Standard Time");
                
            var dateTimeNow = TimeZoneInfo.ConvertTime(dateTime, info);
                
            return dateTimeNow;
        }
    }
}