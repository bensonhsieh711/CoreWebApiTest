using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApiTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreWebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private DBConfig _DBConfig { get; }

        public ValuesController(IOptions<DBConfig> dbConfig)
        {
            _DBConfig = dbConfig.Value;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //return new string[] { "value1", "value2" };
            return new string[] { _DBConfig.IP, _DBConfig.UID, _DBConfig.Pass, _DBConfig.DB };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
