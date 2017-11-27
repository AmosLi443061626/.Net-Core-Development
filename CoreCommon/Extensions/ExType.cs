using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Extensions
{
    static public class ExType
    {
        public static Type[] GetChildTypes(this Type parentType)
        {
            List<Type> lstType = new List<Type>();
            Assembly assem = Assembly.GetAssembly(parentType);
            foreach (Type tChild in assem.GetTypes())
            {
                if (tChild.BaseType == parentType)
                {
                    lstType.Add(tChild);
                }
            }
            return lstType.ToArray();
        }
    }
}
