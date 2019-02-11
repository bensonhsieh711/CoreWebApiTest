using CoreWebApiTest.Model;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace CoreWebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        public RedisConfig _RedisConfig { get; }
        public RedisController(Microsoft.Extensions.Options.IOptions<RedisConfig> redisConfig)
        {
            _RedisConfig = redisConfig.Value;
        } //Action Controller
        [HttpGet("{val}")]
        public string Get(string val)
        {
            ConnectionMultiplexer redisConn = ConnectionMultiplexer.Connect($"{_RedisConfig.IP},password={_RedisConfig.Pass}");
            //ConnectionMultiplexer redisConn = ConnectionMultiplexer.Connect("127.0.0.1:6379,password=syscom#1@2");
            IDatabase resDb = redisConn.GetDatabase();
            resDb.StringSet("testKey", val);
            var result = resDb.StringGet("testKey");
            return result.ToString();
        }
    }
}