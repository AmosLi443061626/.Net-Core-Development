using System;

namespace CoreCommon.Extensions
{
    public static class ExGuid
    {
        /// <summary>
        /// 根据GUID获取16位的唯一字符串
        /// </summary>
        /// <returns></returns>
        public static string To16String(this Guid guid)
        {
            long i = 1;
            foreach (byte b in guid.ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        /// <summary>
        /// 根据GUID获取19位的唯一数字序列 
        /// </summary>
        /// <returns></returns>
        public static long ToLongID(this Guid guid)
        {
            byte[] buffer = guid.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 生成22位唯一的数字 并发可用
        /// </summary>
        /// <returns></returns>
        public static string GenerateUniqueID(this Guid guid)
        {
            System.Threading.Thread.Sleep(1); //保证yyyyMMddHHmmssffff唯一
            Random d = new Random(BitConverter.ToInt32(guid.ToByteArray(), 0));
            string strUnique = DateTime.Now.ToString("yyyyMMddHHmmssffff") + d.Next(1000, 9999);
            return strUnique;
        }
    }
}
