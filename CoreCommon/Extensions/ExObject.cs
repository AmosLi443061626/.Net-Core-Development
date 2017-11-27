using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Extensions
{
    public static class ExObject
    {
        /// <summary>
        /// Object转Int
        /// </summary>
        /// <param name="v">Object</param>
        /// <returns>int</returns>
        public static int ConvertToIntSafe(this object v)
        {
            int i;
            Int32.TryParse(v.ToStringSafe(), out i);
            return i;
        }


        /// <summary>
        /// Object转int
        /// </summary>
        /// <param name="v">Object</param>
        /// <param name="defaultvalue">转换失败默认返回值</param>
        /// <returns>int</returns>
        public static int ConvertToIntSafe(this object v, int defaultvalue)
        {
            int i;
            if (!Int32.TryParse(v.ToStringSafe(), out i))
            {
                i = defaultvalue;
            }
            return i;
        }

        /// <summary>
        /// Object转Long
        /// </summary>
        /// <param name="v">Object</param>
        /// <returns>Long</returns>
        public static long ConvertToLongSafe(this object v)
        {
            long i;
            Int64.TryParse(v.ToStringSafe(), out i);
            return i;
        }

        /// <summary>
        /// Object转换成String
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>string</returns>
        public static string ToStringSafe(this object obj)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Object转换成String
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="defaultValue">转换失败或NULL时返回默认值</param>
        /// <returns>string</returns>
        public static string ToStringSafe(this object obj, string defaultValue)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            return defaultValue;
        }

        /// <summary>
        /// Object转换成bool
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>bool</returns>
        public static bool ToBool(this object obj)
        {
            bool b;
            Boolean.TryParse(obj.ToStringSafe(), out b);
            return b;
        }


        /// <summary>
        /// Object转换成decimal
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>decimal</returns>
        public static decimal ToDecimalSafe(this object obj)
        {
            decimal d;
            Decimal.TryParse(obj.ToStringSafe(), out d);
            return d;
        }

        /// <summary>
        /// Object转换成decimal
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>decimal</returns>
        public static double ToDoubleSafe(this object obj)
        {
            double d = 0.0;
            Double.TryParse(obj.ToStringSafe(), out d);
            return d;
        }

        /// <summary>
        /// Object转换成DateTime
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTimeSafe(this object obj)
        {
            DateTime dt = Convert.ToDateTime("1900-01-01");
            if (null == obj) return dt;
            DateTime.TryParse(obj.ToStringSafe(), out dt);
            return dt;
        }

        public static string ToStringDateStart(this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd 00:00:00");
        }

        public static string ToStringDateEnd(this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd 23:59:59");
        }

        public static string ToStringDateStart(this DateTime? obj)
        {
            if (obj.HasValue)
            {
                return obj.Value.ToString("yyyy-MM-dd 00:00:00");
            }
            return "1949-10-01";
        }

        public static string ToStringDateEnd(this DateTime? obj)
        {
            if (obj.HasValue)
            {
                return obj.Value.ToString("yyyy-MM-dd 23:59:59");
            }
            return "1949-10-01";
        }
    }
}
