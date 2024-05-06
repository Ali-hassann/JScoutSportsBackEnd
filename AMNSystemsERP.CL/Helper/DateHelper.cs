using System.Globalization;

namespace AMNSystemsERP.CL.Helper
{
    public static class DateHelper
    {
        #region Future Work For Date Conversion between Client & Server API Request

        //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-PK");
        //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-PK");

        //DateTime date1 = DateTime.UtcNow;
        //TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
        //DateTime date2 = TimeZoneInfo.ConvertTime(date1, tz);
        #endregion

        public static DateTime GetCurrentDate()
        {
            return DateTime.Now;
        }

        public static string GetDateFormat(DateTime dateTime, bool isTwelveHourFormat = true)
        {
            return isTwelveHourFormat ? dateTime.ToString("MMM dd yyyy hh:mm:ss tt") : dateTime.ToString("MMM dd yyyy HH:mm:ss");
        }

        public static string GetDateFormat(DateTime dateTime
        , bool isTwelveHourFormat = true
        , bool isToIncludeTimeSpan = true
        , string dateFormatToCovert = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(dateFormatToCovert))
                {
                    if (isTwelveHourFormat && isToIncludeTimeSpan)
                    {
                        return dateTime.ToString($"{dateFormatToCovert} hh:mm:ss tt");
                    }
                    else if (!isTwelveHourFormat && isToIncludeTimeSpan)
                    {
                        return dateTime.ToString($"{dateFormatToCovert} HH:mm:ss");
                    }
                    else
                    {
                        return dateTime.ToString(dateFormatToCovert);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return string.Empty;
        }

        public static DateTime AddCurrentTimeSpanToDate(DateTime dateToUpdate)
        {
            try
            {
                dateToUpdate = dateToUpdate.Date.Add(GetCurrentDate().TimeOfDay);
                return dateToUpdate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DateTime GetDateFormat(string dateTimeToConvert)
        {
            var convertedDate = DateTime.MinValue;
            string[] formats = {
                "yyyy-MM-dd HH:mm:ss",
                "MM/dd/yyyy HH:mm:ss",
                "MM/dd/yyyy",
                "dd/MM/yyyy",
                "MM-dd-yyyy",
                "dd-MM-yyyy",
                "yyyy-MM-dd",
                "yyyy-MM-dd",
                "yyyy-MM-dd",
                "M/d/yyyy",
                "d/M/yyyy",
                "yyyy-MM-ddTHH:mm:ss.fffZ",
                "dd-MM-yyyy HH:mm:ss" };

            DateTime.TryParseExact(dateTimeToConvert, formats, CultureInfo.CurrentCulture, DateTimeStyles.None, out convertedDate);

            if (IsMinValue(convertedDate))
            {
                throw new Exception($"{dateTimeToConvert} is not a valid date.");
            }

            return convertedDate;
        }

        private static bool IsMinValue(DateTime dateToCheck)
        {
            return dateToCheck == DateTime.MinValue;
        }

        public static int GetTotalMinutes(DateTime minDate, DateTime maxDate)
        {
            try
            {
                // Calculating Minutes Between Two Dates (maxDate - minDate)
                TimeSpan timeSpan = maxDate - minDate;
                return (int)timeSpan.TotalMinutes;
                //
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetReportFormatDate(string fromDateToConvert, string toDateToConvert)
        {
            try
            {
                if (!string.IsNullOrEmpty(fromDateToConvert) && !string.IsNullOrEmpty(toDateToConvert))
                {
                    var fromDate = GetDateFormat(fromDateToConvert);
                    var toDate = GetDateFormat(toDateToConvert);

                    var strFromDate = GetDateFormat
                    (
                        fromDate,
                        false,
                        false,
                        DateFormats.DefaultFormat
                    );

                    var strToDate = GetDateFormat
                    (
                        toDate,
                        false,
                        false,
                        DateFormats.DefaultFormat
                    );

                    var dateFormat = string.Concat
                    (
                        "From ",
                        strFromDate,
                        " To ",
                        strToDate
                    );

                    return dateFormat;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return "From To";
        }
    }

    public static class DateFormats
    {
        public static readonly string DefaultFormat = "MMM dd yyyy";
        public static readonly string RdlcReportFormat = "MM/dd/yyyy";
        public static readonly string SqlDateFormat = "MM/dd/yyyy";
    }
}