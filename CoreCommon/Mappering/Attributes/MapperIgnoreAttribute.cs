using CoreCommon.Mappering.Base;
using CoreCommon.Mappering.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Mappering.Attributes
{
    /// <summary>
    /// 映射忽略属性
    /// </summary>
    public class MapperIgnoreAttribute : MapperBaseAttribute, IMapperAttribute
    {
        public bool IsRunEachTime
        {
            get
            {
                return false;
            }
        }

        public void Apply<TSource, TDestination>(AutoMapper.IMappingExpression<TSource, TDestination> mappingExpression, string name)
        {
            mappingExpression.ForMember(name, opt => opt.Ignore());
        }
    }
}
