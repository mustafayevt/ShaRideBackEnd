using System;
using System.Globalization;
using ShaRide.Application.Extensions;
using ShaRide.Application.Services.Interface;
using TimeZoneConverter;

namespace ShaRide.Application.Services.Concrete
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;

        public DateTime AzerbaijanDateTime
        {
            get
            {
                var info = TZConvert.GetTimeZoneInfo("Azerbaijan Standard Time");
                
                var dateTimeNow = TimeZoneInfo.ConvertTime(DateTime.Now, info);
                
                return dateTimeNow;
            }
        }
        public string FormattedDateTimeNow => FormattedDateTime(AzerbaijanDateTime);

        /// <summary>
        /// <example>
        /// 28 Mart
        /// 18:00
        /// </example>
        /// </summary>
        public string FormattedDateTime(DateTime dateTime) => dateTime.ToCustomFormat();
    }
}
