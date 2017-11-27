using System;
using System.Collections.Generic;
using System.Linq;
using CoreCommon.Extensions;
using Microsoft.AspNetCore.Mvc;
using CoreCommon.Ioc;
using IStandard.Examples;
using startup.Filters;
using CoreCommon.Configs;

namespace startup.Controllers
{
    [Route("api/[action]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new List<string> { "" };
        }

        // GET api/values/5
        [LogFilter]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return IocContainer.Container.Get<IExampleServices>().Get().msg;
        }

        // POST api/values
        [HttpPost]
        public void PostTest(string username,string password)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
