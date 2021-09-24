using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public static class Utils
    {
        public static string ValToString(object val)
        {
            return (val != DBNull.Value ? val.ToString().Replace(Config.ValueDelimeter(), string.Empty) : string.Empty);
        }

        public static string DateTimeToString(object val)
        {
            if (val == DBNull.Value)
                return string.Empty;

            DateTime dateTime = Convert.ToDateTime(val);
            string dt = dateTime.ToString();

            return dt.Substring(6, 4) + "-" + dt.Substring(3, 2) + "-" + dt.Substring(0, 2) + " " +
                (dateTime.Hour < 10 ? "0" + dateTime.Hour.ToString() : dateTime.Hour.ToString()) + ":" +
                (dateTime.Minute < 10 ? "0" + dateTime.Minute.ToString() : dateTime.Minute.ToString()) + ":" +
                (dateTime.Second < 10 ? "0" + dateTime.Second.ToString() : dateTime.Second.ToString()) + "." +
                (dateTime.Millisecond < 10
                    ? "00" + dateTime.Millisecond.ToString()
                    : dateTime.Millisecond < 100
                        ? "0" + dateTime.Millisecond.ToString()
                        : dateTime.Millisecond.ToString()
                );
        }

        public static string DateToString(object val)
        {
            string dt = Convert.ToDateTime(val).ToString();

            return dt.Substring(6, 4) + "-" + dt.Substring(3, 2) + "-" + dt.Substring(0, 2);
        }

        public static string NumberToString(object val)
        {
            return ValToString(val).Replace(",", ".");
        }

        public static string BoolToString(object val)
        {
            return (val != DBNull.Value ? (Convert.ToBoolean(val) ? "1" : "0") : string.Empty);
        }
    }
}
