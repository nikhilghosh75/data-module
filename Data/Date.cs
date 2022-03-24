using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Data
{
    [Serializable]
    public struct Date : IComparable<Date>, IComparable<DateTime>,
        IFormattable
    {
        public readonly int year;
        public readonly int month;
        public readonly int day;

        public readonly static string[] MONTHS = new string[] { "", "January", "February", "March", 
            "April", "May", "June", "July", "August", "September", "October", "November", "December"};

        public readonly static Date EPOCH = new Date(0);

        public Date(int newYear)
        {
            year = newYear;
            month = 0;
            day = 0;
        }

        public Date(int newYear, int newMonth)
        {
            year = newYear;
            month = newMonth;
            day = 0;
        }

        public Date(int newYear, int newMonth, int newDay)
        {
            year = newYear;
            month = newMonth;
            day = newDay;
        }

        public Date(DateTime dateTime)
        {
            year = dateTime.Year;
            month = dateTime.Month;
            day = dateTime.Day;
        }

        public int CompareTo(Date other)
        {
            if (year != other.year)
                return year.CompareTo(other.year);

            if (month == 0 || other.month == 0)
                return 0;

            if (month != other.month)
                return month.CompareTo(other.month);

            if (day == 0 || other.day == 0)
                return 0;

            return day.CompareTo(other.day);
        }

        public int CompareTo(DateTime other)
        {
            if (year != other.Year)
                return year.CompareTo(other.Year);

            if (month == 0)
                return 0;

            if (month != other.Month)
                return month.CompareTo(other.Month);

            if (day == 0)
                return 0;

            return day.CompareTo(other.Day);
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (string.IsNullOrEmpty(format)) 
                format = "Simple";

            if (formatProvider == null)
                formatProvider = CultureInfo.CurrentCulture;

            if(format == "Simple")
            {
                return MonthToString(month) + " " + day.ToString() + ", " + year.ToString();
            }

            throw new FormatException("Date is not in a valid string");
        }

        public static bool operator > (Date date1, Date date2)
        {
            return date1.CompareTo(date2) > 0;
        }

        public static bool operator < (Date date1, Date date2)
        {
            return date1.CompareTo(date2) < 0;
        }

        public static bool operator >= (Date date1, Date date2)
        {
            return date1.CompareTo(date2) >= 0;
        }

        public static bool operator <= (Date date1, Date date2)
        {
            return date1.CompareTo(date2) <= 0;
        }

        public static bool operator == (Date date1, Date date2)
        {
            return date1.CompareTo(date2) == 0;
        }

        public static bool operator != (Date date1, Date date2)
        {
            return date1.CompareTo(date2) != 0;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;

            return CompareTo((Date)obj) == 0;
        }

        public override int GetHashCode()
        {
            return DaysBetween(this, EPOCH);
        }

        public static bool IsValidDate(string str)
        {
            Regex shortFullDateRegex = new Regex("\\d{1,2}-\\d{1,2}-\\d\\d\\d\\d");
            Regex shortHalfDateRegex = new Regex("\\d{1,2}-\\d\\d\\d\\d");
            Regex shortYearRegex = new Regex("\\d{1,5}");
            Regex dashedFullDateRegex = new Regex("\\d{1,2}/\\d{1,2}/\\d\\d\\d\\d");
            Regex dashedHalfDateRegex = new Regex("\\d{1,2}/\\d\\d\\d\\d");

            if (shortFullDateRegex.IsMatch(str))
                return true;
            if (shortHalfDateRegex.IsMatch(str))
                return true;
            if (shortYearRegex.IsMatch(str))
                return true;
            if (dashedFullDateRegex.IsMatch(str))
                return true;
            if (dashedHalfDateRegex.IsMatch(str))
                return true;

            return false;
        }

        public static Date Parse(string str)
        {
            Regex shortFullDateRegex = new Regex("\\d{1,2}-\\d{1,2}-\\d\\d\\d\\d");
            Regex shortHalfDateRegex = new Regex("\\d{1,2}-\\d\\d\\d\\d");
            Regex shortYearRegex = new Regex("\\d{1,5}");

            Regex dashedFullDateRegex = new Regex("\\d{1,2}/\\d{1,2}/\\d\\d\\d\\d");
            Regex dashedHalfDateRegex = new Regex("\\d{1,2}/\\d\\d\\d\\d");

            if (shortFullDateRegex.IsMatch(str) || dashedFullDateRegex.IsMatch(str))
            {
                string[] strs = str.Split('-');
                if (dashedFullDateRegex.IsMatch(str))
                    strs = str.Split('/');

                int month = int.Parse(strs[0]);
                int day = int.Parse(strs[1]);
                int year = int.Parse(strs[2]);
                return new Date(year, month, day);
            }
            if (shortHalfDateRegex.IsMatch(str) || dashedHalfDateRegex.IsMatch(str))
            {
                string[] strs = str.Split('-');
                if (dashedHalfDateRegex.IsMatch(str))
                    strs = str.Split('/');

                int month = int.Parse(strs[0]);
                int year = int.Parse(strs[1]);
                return new Date(year, month);
            }
            if (shortYearRegex.IsMatch(str))
                return new Date(int.Parse(str));

            return EPOCH;
        }

        public static string MonthToString(int month)
        {
            switch(month)
            {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
            }
            return "";
        }

        public static int DaysBetween(Date date1, Date date2)
        {
            int year1 = date1.month <= 2 ? date1.year - 1: date1.year;
            int month1 = date1.month <= 2 ? date1.month + 13 : date1.month + 1;
            int day1 = date1.day;

            int days1 = (int)Math.Floor((decimal)1461 * year1 / 4)
                + (int)Math.Floor((decimal)153 * month1 / 5) + day1;

            int year2 = date2.month <= 2 ? date2.year - 1 : date2.year;
            int month2 = date2.month <= 2 ? date2.month + 13 : date2.month + 1;
            int day2 = date2.day;

            int days2 = (int)Math.Floor((decimal)1461 * year2 / 4)
                + (int)Math.Floor((decimal)153 * month2 / 5) + day2;

            return days1 - days2;
        }
    }
}
