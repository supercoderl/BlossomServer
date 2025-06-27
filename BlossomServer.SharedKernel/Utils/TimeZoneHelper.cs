using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlossomServer.SharedKernel.Utils
{
    public static class TimeZoneHelper
    {
        /// <summary>
        /// Trả về TimeZoneId của hệ thống hiện tại (ví dụ: "SE Asia Standard Time")
        /// </summary>
        public static string GetLocalTimeZoneId()
        {
            return TimeZoneInfo.Local.Id;
        }

        /// <summary>
        /// Convert from UTC to local by TimeZoneId
        /// </summary>
        public static DateTime GetCurrentTimeByZoneId(string timeZoneId)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
        }

        /// <summary>
        /// Return current system's local time
        /// </summary>
        public static DateTime GetLocalTimeNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Try convert UTC to time by TimeZoneId, if error, return null
        /// </summary>
        public static DateTime? TryGetCurrentTimeByZoneId(string timeZoneId)
        {
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert any DateTime UTC to local time by system's TimeZoneId
        /// </summary>
        public static DateTime ConvertUtcToLocal(DateTime utcTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Convert any DateTime UTC to local time by system's TimeZoneId
        /// </summary>
        public static DateTimeOffset ConvertUtcToLocal(DateTimeOffset utcTime, TimeZoneInfo timeZone)
        {
            var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime.UtcDateTime, timeZone);
            var offset = timeZone.GetUtcOffset(localDateTime);
            return new DateTimeOffset(localDateTime, offset);
        }
    }
}
