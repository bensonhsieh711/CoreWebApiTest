using CoreWebApiTest.Model;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using SybaseFramework;
using NLog;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

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
                    return $"Key:testKey, Value:{result.ToString()}, TimeToLive:{ttl.Value.ToString()}";
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex.ToString());
                throw ex;
            }
        } 
        
        public async Task<int> Set(string key, string value, int expireSeconds = 0)
        {
            int result = 0;

            try
            {
                using (var redisConn = ConnectionMultiplexer.Connect($"{_RedisConfig.IP},password={_RedisConfig.Pass}"))
                {
                    IDatabase resDb = redisConn.GetDatabase();
                    var b = await resDb.StringSetAsync(key, value, new TimeSpan(0, 0, expireSeconds));
                    result = 1;
                    
                }
            }
            catch (Exception ex)
            {
                logger.Warn(ex.ToString());
                throw ex;
            }

            return result;
        }
    }
}