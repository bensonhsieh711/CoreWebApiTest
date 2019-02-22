using CoreWebApiTest.Model;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using SybaseFramework;
using NLog;
using Microsoft.Extensions.Options;

namespace CoreWebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private RedisConfig _RedisConfig { get; }

        public RedisController(IOptions<RedisConfig> redisConfig)
        {
            _RedisConfig = redisConfig.Value;
        }

        [HttpGet("{val}")]
        public string Get(string val)
        {
            try
            {
                using (var redisConn = ConnectionMultiplexer.Connect($"{_RedisConfig.IP},password={_RedisConfig.Pass}"))
                {
                    IDatabase resDb = redisConn.GetDatabase();

                    var result = resDb.StringGet("testKey");

                    while (result.IsNull)
                    {
                        resDb.StringSet("testKey", val, new TimeSpan(0, 0, 30));
                        result = resDb.StringGet("testKey");
                    }
                    var ttl = resDb.KeyTimeToLive("testKey");
                    return $"Key:{result.ToString()}, TimeToLive:{ttl.Value.ToString()}";
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex.ToString());
                throw ex;
            }
        }        
    }
}