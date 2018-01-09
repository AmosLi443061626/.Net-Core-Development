using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreCommon.Extensions
{
    public static class ExEnums
    {
        private static ConcurrentDictionary<Enum, DescriptionAttribute> senumCache = new ConcurrentDictionary<Enum, DescriptionAttribute>();

        public static string GetEnumDescription(this Enum senum)
        {
            DescriptionAttribute descriptionAttribute = null;
            senumCache.TryGetValue(senum, out descriptionAttribute);
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
            var attris = senum.GetType().GetField(senum.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attris.Length > 0)
            {
                descriptionAttribute = ((DescriptionAttribute)attris[0]);
            }
            if (descriptionAttribute == null)
            {
                descriptionAttribute = new DescriptionAttribute();
            }
            senumCache.TryAdd(senum, descriptionAttribute);
            return descriptionAttribute.Description;
        }
    }
}
