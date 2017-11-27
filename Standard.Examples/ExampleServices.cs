using IStandard.Examples;
using System;
using CoreCommon.Results;
using CoreCommon.Checks;
using DBContextEntity;
using AntData.ORM;
using DBContextEntity.Mysql;

namespace Standard.Examples
{
    public class ExampleServices : IExampleServices
    {
        public Result Get()
        {
            //验证参数
            //Check.CheckCondition(() => 2 == 2, "验证失败");


            DbContext.Container.Context.Insert(new Mytest { Name=DateTime.Now.ToString()});


            return Result.Success();
        }
    }
}
