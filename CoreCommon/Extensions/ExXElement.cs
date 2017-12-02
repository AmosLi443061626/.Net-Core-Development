using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CoreCommon.Extensions
{
    /// <summary>
    ///扩展XML XElement
    /// </summary>
    public static class ExXElement
    {
        #region 私有方法
        private static void ElmentToConfJsonAdd(XElement xElement, Dictionary<string, string> dictionary)
        {
            if (!xElement.HasElements)
            {
                var sb = new StringBuilder();
                XElmentByParentName(xElement, sb);
                sb.Append(xElement.Name.LocalName);
                dictionary.Add(sb.ToString(), xElement.Value);
            }
            using (IEnumerator<XElement> enumerator = xElement.Elements().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var xlment = enumerator.Current;
                    ElmentToConfJsonAdd(xlment, dictionary);
                }
            }
        }

        private static void XElmentByParentName(XElement xElement, StringBuilder sb)
        {
            var xLement = xElement.Parent;
            if (xLement == null || xLement.Name.LocalName == "root")
                return;
            sb.Insert(0, xLement.Name.LocalName + ":");
            XElmentByParentName(xLement, sb);
        }

        #endregion
        /// <summary>
        /// XElment扩展为 Dictionary: Key=XElment:XElment2:XElmentN  | value = XElmentN.Value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, string> XElmentToConfJson(this string value)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.Unicode.GetBytes(value), XmlDictionaryReaderQuotas.Max))
            {
                var xlement = XElement.Load(reader);
                using (IEnumerator<XElement> enumerator = xlement.Elements().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var xlment = enumerator.Current;
                        ElmentToConfJsonAdd(xlment, dictionary);
                    }
                }
                return dictionary;
            }
        }
    }
}
