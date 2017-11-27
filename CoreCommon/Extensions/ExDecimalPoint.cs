using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Extensions
{
    public static class ExDecimalPoint
    {
        /// <summary>
        /// 将小数点乘以100转为整数
        /// </summary>
        /// <param name="v">float</param>
        /// <returns>int</returns>
        public static int ConvertToIntMinuteMultiply100(this float v)
        {
            int i;
            Int32.TryParse((v * 100).ToStringSafe(), out i);
            return i;
        }

        /// <summary>
        /// 将小数点乘以100转为整数
        /// </summary>
        /// <param name="v">decimal</param>
        /// <returns>int</returns>
        public static int ConvertToIntMinuteMultiply100(this decimal v)
        {
            return Convert.ToInt32(v * 100);
        }

        /// <summary>
        /// 将整数除以100转为小数点2位
        /// </summary>
        /// <param name="v">decimal</param>
        /// <returns>int</returns>
        public static decimal ConvertToIntDivided100(this int v)
        {
            return v / 100.00M;
        }

        /// <summary>
        /// 将小数点乘以100转为整数
        /// </summary>
        /// <param name="v">double</param>
        /// <returns>int</returns>
        public static int ConvertToIntMinuteMultiply100(this double v)
        {
            int i;
            Int32.TryParse((v * 100).ToStringSafe(), out i);
            return i;
        }
    }
}
